using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Permisos;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Auth
{
    public partial class FrmAdministrarPermisos : Form, IObservador
    {
        private readonly PermisoService _permisoService;
        private readonly SesionIdioma _sesionIdioma;
        // Arbol ya compuesto desde base: lo que se ve en el TreeView.
        private List<PermisoComponent> _permisos;
        // Catalogo disponible para seleccionar y agregar a una composicion.
        private List<FamiliaPermiso> _familias;
        private List<PermisoSimple> _permisosSimples;
        // Id de la familia que se esta creando/editando desde el panel de Familia.
        private int _idFamiliaSeleccionada;

        public FrmAdministrarPermisos()
        {
            _permisoService = new PermisoService();
            _sesionIdioma = SesionIdioma.GetInstance();
            _permisos = new List<PermisoComponent>();
            _familias = new List<FamiliaPermiso>();
            _permisosSimples = new List<PermisoSimple>();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            // Observer de idiomas: todos los controles usan su Tag como clave de traduccion.
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            GBX_Arbol.Text = idiomaObservado.BuscarTraduccion(GBX_Arbol.Tag.ToString());
            GBX_Familia.Text = idiomaObservado.BuscarTraduccion(GBX_Familia.Tag.ToString());
            GBX_Catalogo.Text = idiomaObservado.BuscarTraduccion(GBX_Catalogo.Tag.ToString());
            GBX_Composicion.Text = idiomaObservado.BuscarTraduccion(GBX_Composicion.Tag.ToString());
            LBL_NombreFamilia.Text = idiomaObservado.BuscarTraduccion(LBL_NombreFamilia.Tag.ToString());
            LBL_Familias.Text = idiomaObservado.BuscarTraduccion(LBL_Familias.Tag.ToString());
            LBL_PermisosSimples.Text = idiomaObservado.BuscarTraduccion(LBL_PermisosSimples.Tag.ToString());
            LBL_Destino.Text = idiomaObservado.BuscarTraduccion(LBL_Destino.Tag.ToString());
            BTN_CrearFamilia.Text = idiomaObservado.BuscarTraduccion(BTN_CrearFamilia.Tag.ToString());
            BTN_EditarFamilia.Text = idiomaObservado.BuscarTraduccion(BTN_EditarFamilia.Tag.ToString());
            BTN_EliminarFamilia.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarFamilia.Tag.ToString());
            BTN_AgregarFamilia.Text = idiomaObservado.BuscarTraduccion(BTN_AgregarFamilia.Tag.ToString());
            BTN_AgregarPermiso.Text = idiomaObservado.BuscarTraduccion(BTN_AgregarPermiso.Tag.ToString());
            BTN_QuitarSeleccionado.Text = idiomaObservado.BuscarTraduccion(BTN_QuitarSeleccionado.Tag.ToString());
            BTN_Limpiar.Text = idiomaObservado.BuscarTraduccion(BTN_Limpiar.Tag.ToString());

            ActualizarTextoRaiz();
            ActualizarDestino();
        }

        private void FrmAdministrarPermisos_Load(object sender, EventArgs e)
        {
            // El formulario queda escuchando cambios de idioma hasta cerrarse.
            _sesionIdioma.RegistrarObservador(this);

            if (!PuedeVerPermisos())
            {
                MessageBox.Show(
                    T("Mensaje.SinPermisos"),
                    T("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            // Carga inicial: catalogos disponibles + arbol compuesto persistido.
            CargarDatos();
            Actualizar(_sesionIdioma.idioma);
        }

        private void BTN_CrearFamilia_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                // Crear familia solo agrega una fila al catalogo Permisos.
                // No aparece en el arbol hasta usar Agregar familia.
                FamiliaPermiso familia = _permisoService.CrearFamilia(TBX_NombreFamilia.Text);
                CargarCatalogos();
                SeleccionarFamiliaEnLista(familia.Id);
                MessageBox.Show(T("Mensaje.FamiliaCreada"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EditarFamilia_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_idFamiliaSeleccionada == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneFamilia");
                return;
            }

            try
            {
                // Editar cambia el nombre de la familia del catalogo.
                // Como el arbol apunta al mismo id, todas sus apariciones muestran el nuevo nombre.
                FamiliaPermiso familia = _permisoService.EditarFamilia(_idFamiliaSeleccionada, TBX_NombreFamilia.Text);
                CargarArbol(familia.Id);
                CargarCatalogos();
                SeleccionarFamiliaEnLista(familia.Id);
                MessageBox.Show(T("Mensaje.FamiliaEditada"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EliminarFamilia_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosEliminar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FamiliaPermiso familia = ObtenerFamiliaSeleccionada();

            if (familia == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneFamilia");
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                T("Mensaje.ConfirmarEliminarFamilia").Replace("{0}", familia.Nombre),
                T("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                // Elimina la familia del catalogo y todas sus apariciones; no elimina permisos simples.
                _permisoService.EliminarFamilia(familia.Id);
                LimpiarSeleccionFamilia();
                CargarDatos();
                MessageBox.Show(T("Mensaje.FamiliaEliminada"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_AgregarFamilia_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosComponer))
            {
                MostrarAccesoDenegado();
                return;
            }

            // Origen: familia elegida desde el catalogo.
            FamiliaPermiso familia = LBX_Familias.SelectedItem as FamiliaPermiso;
            // El destino puede ser raiz o una familia seleccionada en el arbol.
            NodoPermiso destino = ObtenerDestinoSeleccionado();

            if (familia == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneFamilia");
                return;
            }

            if (destino == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneDestino");
                return;
            }

            try
            {
                // Agregar no duplica la familia en Permisos: crea un vinculo en PermisoComposicion.
                _permisoService.AgregarComponente(destino.IdPermiso.Value, familia.Id);
                CargarArbol(destino.IdPermiso);
                MessageBox.Show(T("Mensaje.ComponenteAgregado"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_AgregarPermiso_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosComponer))
            {
                MostrarAccesoDenegado();
                return;
            }

            // Origen: permiso simple del catalogo precargado por desarrolladores.
            PermisoSimple permiso = LBX_PermisosSimples.SelectedItem as PermisoSimple;
            // Los permisos simples solo pueden agregarse dentro de familias, nunca debajo de raiz.
            NodoPermiso destino = ObtenerDestinoSeleccionado();

            if (permiso == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneComponente");
                return;
            }

            if (destino == null || destino.EsRaiz)
            {
                MostrarAdvertencia("Mensaje.SeleccioneDestino");
                return;
            }

            try
            {
                // Se agrega una aparicion del permiso simple dentro de la familia destino.
                _permisoService.AgregarComponente(destino.IdPermiso.Value, permiso.Id);
                CargarArbol(destino.IdPermiso);
                MessageBox.Show(T("Mensaje.ComponenteAgregado"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_QuitarSeleccionado_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosComponer))
            {
                MostrarAccesoDenegado();
                return;
            }

            NodoPermiso nodo = ObtenerNodoSeleccionado();

            if (nodo == null || nodo.EsRaiz)
            {
                MostrarAdvertencia("Mensaje.SeleccioneComponente");
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                T("Mensaje.ConfirmarQuitarComponente").Replace("{0}", nodo.Permiso.Nombre),
                T("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                // Quitar borra solo el vinculo de composicion, no el componente del catalogo.
                if (!nodo.IdPadre.HasValue)
                    return;

                _permisoService.QuitarComponente(nodo.IdPadre.Value, nodo.Permiso.Id);
                CargarArbol(nodo.IdPadre);
                MessageBox.Show(T("Mensaje.ComponenteQuitado"), T("Titulo.Exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Limpiar_Click(object sender, EventArgs e)
        {
            // Vuelve al estado base de trabajo: sin familia en edicion y raiz seleccionada como destino.
            LimpiarSeleccionFamilia();
            SeleccionarRaiz();
            ActualizarBotones();
        }

        private void TVW_Permisos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NodoPermiso nodo = ObtenerNodoSeleccionado();

            // Seleccionar raiz no edita ninguna familia, pero si sirve como destino para agregar familias.
            if (nodo == null || nodo.EsRaiz)
            {
                LimpiarSeleccionFamilia();
                ActualizarDestino();
                ActualizarBotones();
                return;
            }

            // Si el nodo es familia, se carga en el textbox para poder editar/eliminar.
            // Si es permiso simple, se limpia porque los permisos simples no se editan desde UI.
            if (nodo.Permiso.EsFamilia)
                SeleccionarFamilia(nodo.Permiso.Id, nodo.Permiso.Nombre);
            else
                LimpiarSeleccionFamilia();

            ActualizarDestino();
            ActualizarBotones();
        }

        private void LBX_Familias_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seleccionar una familia del catalogo la prepara para crear vinculos o editar su nombre.
            FamiliaPermiso familia = LBX_Familias.SelectedItem as FamiliaPermiso;

            if (familia == null)
            {
                _idFamiliaSeleccionada = 0;
                ActualizarBotones();
                return;
            }

            SeleccionarFamilia(familia.Id, familia.Nombre);
            ActualizarBotones();
        }

        private void LBX_PermisosSimples_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Elegir un permiso simple solo afecta si el boton Agregar permiso puede activarse.
            ActualizarBotones();
        }

        private void CargarDatos()
        {
            // Refresca las dos fuentes visuales: catalogos de la derecha y composicion del arbol.
            CargarCatalogos();
            CargarArbol();
        }

        private void CargarCatalogos()
        {
            // Familias y permisos simples son catalogos: existen aunque no esten agregados al arbol.
            _familias = _permisoService.ListarFamilias();
            _permisosSimples = _permisoService.ListarPermisosSimples();

            LBX_Familias.DataSource = null;
            LBX_Familias.DisplayMember = "Nombre";
            LBX_Familias.DataSource = _familias;
            LBX_Familias.SelectedIndex = -1;

            LBX_PermisosSimples.DataSource = null;
            LBX_PermisosSimples.DisplayMember = "Nombre";
            LBX_PermisosSimples.DataSource = _permisosSimples;
            LBX_PermisosSimples.SelectedIndex = -1;

            ActualizarBotones();
        }

        private void CargarArbol(int? idSeleccionar = null)
        {
            // El servicio devuelve la composicion completa ya armada como Composite.
            _permisos = _permisoService.ListarArbol();

            TVW_Permisos.BeginUpdate();
            TVW_Permisos.Nodes.Clear();

            // La raiz ya es un componente persistido: se muestra como nodo real del Composite.
            foreach (PermisoComponent permiso in _permisos.OrderBy(x => x.Nombre))
                TVW_Permisos.Nodes.Add(CrearNodo(permiso, null));
            TVW_Permisos.ExpandAll();
            TVW_Permisos.EndUpdate();

            // Luego de refrescar, se intenta conservar el contexto del usuario.
            if (idSeleccionar.HasValue)
                SeleccionarNodo(idSeleccionar.Value);
            else
                SeleccionarRaiz();

            ActualizarDestino();
            ActualizarBotones();
        }

        private TreeNode CrearNodo(PermisoComponent permiso, int? idPadre)
        {
            TreeNode nodo = new TreeNode(FormatearNodo(permiso));
            // El Tag conserva el permiso y su padre para poder quitar exactamente este vinculo.
            nodo.Tag = NodoPermiso.CrearPermiso(permiso, idPadre);

            // Recorrido recursivo: cada familia agrega sus hijos como nodos internos.
            foreach (PermisoComponent hijo in permiso.Hijos.OfType<PermisoComponent>().OrderBy(x => x.Nombre))
                nodo.Nodes.Add(CrearNodo(hijo, permiso.Id));

            return nodo;
        }

        private string FormatearNodo(PermisoComponent permiso)
        {
            if (EsRaizSistema(permiso))
                return T("Permisos.Raiz");

            string tipo = permiso.EsFamilia ? "[F]" : "[P]";
            return tipo + " " + permiso.Nombre;
        }

        private bool EsRaizSistema(PermisoComponent permiso)
        {
            return permiso != null && string.Equals(permiso.Nombre, "Raiz", StringComparison.OrdinalIgnoreCase);
        }

        private void ActualizarTextoRaiz()
        {
            if (TVW_Permisos.Nodes.Count > 0)
                TVW_Permisos.Nodes[0].Text = T("Permisos.Raiz");
        }

        private void SeleccionarNodo(int idPermiso)
        {
            // Despues de editar/agregar, intenta dejar seleccionado el permiso/familia involucrado.
            TreeNode nodo = BuscarNodo(TVW_Permisos.Nodes, idPermiso);

            if (nodo == null)
            {
                SeleccionarRaiz();
                return;
            }

            TVW_Permisos.SelectedNode = nodo;
            nodo.EnsureVisible();
        }

        private void SeleccionarRaiz()
        {
            if (TVW_Permisos.Nodes.Count <= 0)
                return;

            TVW_Permisos.SelectedNode = TVW_Permisos.Nodes[0];
            TVW_Permisos.Nodes[0].EnsureVisible();
        }

        private TreeNode BuscarNodo(TreeNodeCollection nodos, int idPermiso)
        {
            // Busca en profundidad dentro del TreeView hasta encontrar el primer nodo con ese id.
            foreach (TreeNode nodo in nodos)
            {
                NodoPermiso nodoPermiso = nodo.Tag as NodoPermiso;

                if (nodoPermiso != null && nodoPermiso.Permiso != null && nodoPermiso.Permiso.Id == idPermiso)
                    return nodo;

                TreeNode nodoEncontrado = BuscarNodo(nodo.Nodes, idPermiso);

                if (nodoEncontrado != null)
                    return nodoEncontrado;
            }

            return null;
        }

        private NodoPermiso ObtenerNodoSeleccionado()
        {
            if (TVW_Permisos.SelectedNode == null)
                return null;

            // Todos los nodos se cargan con NodoPermiso en Tag para evitar depender del texto visual.
            return TVW_Permisos.SelectedNode.Tag as NodoPermiso;
        }

        private NodoPermiso ObtenerDestinoSeleccionado()
        {
            // Destino significa "donde voy a insertar el componente elegido del catalogo".
            NodoPermiso nodo = ObtenerNodoSeleccionado();

            if (nodo == null)
                return null;

            if (nodo.EsRaiz)
                return nodo;

            if (nodo.Permiso != null && nodo.Permiso.EsFamilia)
                return nodo;

            // Si se selecciona un permiso simple, no puede ser destino porque no contiene hijos.
            return null;
        }

        private FamiliaPermiso ObtenerFamiliaSeleccionada()
        {
            // La familia seleccionada puede venir del arbol o del ListBox de catalogo.
            if (_idFamiliaSeleccionada == 0)
                return null;

            return _familias.FirstOrDefault(x => x.Id == _idFamiliaSeleccionada);
        }

        private void SeleccionarFamilia(int idFamilia, string nombre)
        {
            // Centraliza la seleccion para que arbol y lista usen el mismo estado de edicion.
            _idFamiliaSeleccionada = idFamilia;
            TBX_NombreFamilia.Text = nombre;
        }

        private void SeleccionarFamiliaEnLista(int idFamilia)
        {
            // Sincroniza el ListBox luego de crear o editar una familia.
            for (int i = 0; i < LBX_Familias.Items.Count; i++)
            {
                FamiliaPermiso familia = LBX_Familias.Items[i] as FamiliaPermiso;

                if (familia != null && familia.Id == idFamilia)
                {
                    LBX_Familias.SelectedIndex = i;
                    return;
                }
            }
        }

        private void LimpiarSeleccionFamilia()
        {
            // Sale del modo edicion/eliminacion de familia.
            _idFamiliaSeleccionada = 0;
            TBX_NombreFamilia.Clear();

            if (LBX_Familias.Items.Count > 0)
                LBX_Familias.SelectedIndex = -1;
        }

        private void ActualizarDestino()
        {
            // Muestra al usuario donde se va a agregar una familia/permiso si presiona Agregar.
            NodoPermiso nodo = ObtenerNodoSeleccionado();

            if (nodo == null)
            {
                TBX_Destino.Text = T("Permisos.SeleccioneDestino");
                return;
            }

            if (nodo.EsRaiz)
            {
                TBX_Destino.Text = T("Permisos.Raiz");
                return;
            }

            if (nodo.Permiso.EsFamilia)
            {
                TBX_Destino.Text = nodo.Permiso.Nombre;
                return;
            }

            TBX_Destino.Text = T("Permisos.SeleccionPermisoSimple");
        }

        private void ActualizarBotones()
        {
            // Los botones se calculan desde el estado actual de seleccion, no desde reglas visuales sueltas.
            NodoPermiso destino = ObtenerDestinoSeleccionado();
            bool hayDestino = destino != null;
            bool destinoEsFamilia = hayDestino && !destino.EsRaiz;
            bool hayFamiliaDisponible = LBX_Familias.SelectedItem as FamiliaPermiso != null;
            bool hayPermisoDisponible = LBX_PermisosSimples.SelectedItem as PermisoSimple != null;
            NodoPermiso seleccionado = ObtenerNodoSeleccionado();
            bool puedeCrear = TienePermiso(CodigosPermiso.PermisosCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.PermisosEditar);
            bool puedeEliminar = TienePermiso(CodigosPermiso.PermisosEliminar);
            bool puedeComponer = TienePermiso(CodigosPermiso.PermisosComponer);
            bool familiaAsignadaAlUsuarioActual = _idFamiliaSeleccionada > 0 &&
                _permisoService.EsFamiliaAsignadaAlUsuarioActual(_idFamiliaSeleccionada);

            // Crear familia queda siempre disponible; estos botones dependen de seleccion valida.
            BTN_CrearFamilia.Visible = puedeCrear;
            BTN_EditarFamilia.Visible = puedeEditar;
            BTN_EliminarFamilia.Visible = puedeEliminar;
            BTN_AgregarFamilia.Visible = puedeComponer;
            BTN_AgregarPermiso.Visible = puedeComponer;
            BTN_QuitarSeleccionado.Visible = puedeComponer;

            BTN_CrearFamilia.Enabled = puedeCrear;
            BTN_EditarFamilia.Enabled = puedeEditar && _idFamiliaSeleccionada > 0;
            BTN_EliminarFamilia.Enabled = puedeEliminar && _idFamiliaSeleccionada > 0 && !familiaAsignadaAlUsuarioActual;
            BTN_AgregarFamilia.Enabled = puedeComponer && hayDestino && hayFamiliaDisponible;
            BTN_AgregarPermiso.Enabled = puedeComponer && destinoEsFamilia && hayPermisoDisponible;
            BTN_QuitarSeleccionado.Enabled = puedeComponer && seleccionado != null && !seleccionado.EsRaiz;

            TBX_NombreFamilia.Enabled = puedeCrear || puedeEditar;
        }

        private void MostrarAdvertencia(string claveMensaje)
        {
            MessageBox.Show(
                T(claveMensaje),
                T("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                T("Mensaje.ErrorPermiso").Replace("{0}", ex.Message),
                T("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private bool PuedeVerPermisos()
        {
            return TienePermiso(CodigosPermiso.PermisosVer);
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                T("Mensaje.SinPermisos"),
                T("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private string T(string clave)
        {
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }

        private class NodoPermiso
        {
            // Wrapper de UI: conserva el componente real y su padre para operar sobre vinculos exactos.
            public PermisoComponent Permiso { get; private set; }
            public int? IdPadre { get; private set; }
            public int? IdPermiso
            {
                get
                {
                    if (Permiso == null)
                        return null;

                    return Permiso.Id;
                }
            }

            public bool EsRaiz
            {
                get { return Permiso != null && string.Equals(Permiso.Nombre, "Raiz", StringComparison.OrdinalIgnoreCase); }
            }

            private NodoPermiso()
            {
            }

            public static NodoPermiso CrearPermiso(PermisoComponent permiso, int? idPadre)
            {
                return new NodoPermiso
                {
                    Permiso = permiso,
                    IdPadre = idPadre
                };
            }
        }
    }
}
