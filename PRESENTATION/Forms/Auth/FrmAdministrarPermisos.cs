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
        private const int IdRaiz = 0;

        private readonly PermisoService _permisoService;
        private readonly SesionIdioma _sesionIdioma;
        private List<PermisoComponent> _permisos;
        private List<int> _idsPadres;

        public FrmAdministrarPermisos()
        {
            _permisoService = new PermisoService();
            _sesionIdioma = SesionIdioma.GetInstance();
            _permisos = new List<PermisoComponent>();
            _idsPadres = new List<int>();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            GBX_Arbol.Text = idiomaObservado.BuscarTraduccion(GBX_Arbol.Tag.ToString());
            GBX_Detalle.Text = idiomaObservado.BuscarTraduccion(GBX_Detalle.Tag.ToString());
            LBL_Nombre.Text = idiomaObservado.BuscarTraduccion(LBL_Nombre.Tag.ToString());
            LBL_Codigo.Text = idiomaObservado.BuscarTraduccion(LBL_Codigo.Tag.ToString());
            LBL_Descripcion.Text = idiomaObservado.BuscarTraduccion(LBL_Descripcion.Tag.ToString());
            LBL_Tipo.Text = idiomaObservado.BuscarTraduccion(LBL_Tipo.Tag.ToString());
            LBL_Padre.Text = idiomaObservado.BuscarTraduccion(LBL_Padre.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Eliminar.Text = idiomaObservado.BuscarTraduccion(BTN_Eliminar.Tag.ToString());
            BTN_Mover.Text = idiomaObservado.BuscarTraduccion(BTN_Mover.Tag.ToString());
            BTN_Limpiar.Text = idiomaObservado.BuscarTraduccion(BTN_Limpiar.Tag.ToString());

            CargarTipos();
            CargarPadres(ObtenerPermisoSeleccionado());
        }

        private void FrmAdministrarPermisos_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!SessionManager.HaySesionActiva())
            {
                MessageBox.Show(
                    T("Mensaje.SinPermisos"),
                    T("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CargarArbol();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            try
            {
                bool esFamilia = CBX_Tipo.SelectedIndex == 1;
                int? idPadre = ObtenerIdPadreSeleccionado();
                PermisoComponent permiso;

                if (esFamilia)
                    permiso = _permisoService.CrearFamilia(TBX_Nombre.Text, TBX_Codigo.Text, TBX_Descripcion.Text, idPadre);
                else
                    permiso = _permisoService.CrearPermiso(TBX_Nombre.Text, TBX_Codigo.Text, TBX_Descripcion.Text, idPadre);

                CargarArbol(permiso.Id);
                MessageBox.Show(
                    T("Mensaje.PermisoCreado"),
                    T("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            PermisoComponent permiso = ObtenerPermisoSeleccionado();

            if (permiso == null)
            {
                MostrarSeleccionePermiso();
                return;
            }

            try
            {
                bool esFamilia = CBX_Tipo.SelectedIndex == 1;
                _permisoService.Modificar(permiso.Id, TBX_Nombre.Text, TBX_Codigo.Text, TBX_Descripcion.Text, esFamilia);
                CargarArbol(permiso.Id);
                MessageBox.Show(
                    T("Mensaje.PermisoEditado"),
                    T("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Eliminar_Click(object sender, EventArgs e)
        {
            PermisoComponent permiso = ObtenerPermisoSeleccionado();

            if (permiso == null)
            {
                MostrarSeleccionePermiso();
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                T("Mensaje.ConfirmarEliminarPermiso").Replace("{0}", permiso.Nombre),
                T("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                _permisoService.Eliminar(permiso.Id);
                LimpiarDetalle();
                CargarArbol();
                MessageBox.Show(
                    T("Mensaje.PermisoEliminado"),
                    T("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Mover_Click(object sender, EventArgs e)
        {
            PermisoComponent permiso = ObtenerPermisoSeleccionado();

            if (permiso == null)
            {
                MostrarSeleccionePermiso();
                return;
            }

            try
            {
                _permisoService.Mover(permiso.Id, ObtenerIdPadreSeleccionado());
                CargarArbol(permiso.Id);
                MessageBox.Show(
                    T("Mensaje.PermisoMovido"),
                    T("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Limpiar_Click(object sender, EventArgs e)
        {
            TVW_Permisos.SelectedNode = null;
            LimpiarDetalle();
        }

        private void TVW_Permisos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PermisoComponent permiso = ObtenerPermisoSeleccionado();

            if (permiso == null)
            {
                LimpiarDetalle();
                return;
            }

            TBX_Nombre.Text = permiso.Nombre;
            TBX_Codigo.Text = permiso.Codigo;
            TBX_Descripcion.Text = permiso.Descripcion;
            SeleccionarTipo(permiso.EsFamilia);
            CargarPadres(permiso);
            ActualizarBotones();
        }

        private void CargarArbol(int? idSeleccionar = null)
        {
            _permisos = _permisoService.ListarArbol();

            TVW_Permisos.BeginUpdate();
            TVW_Permisos.Nodes.Clear();

            foreach (PermisoComponent permiso in _permisos)
                TVW_Permisos.Nodes.Add(CrearNodo(permiso));

            TVW_Permisos.ExpandAll();
            TVW_Permisos.EndUpdate();

            if (idSeleccionar.HasValue)
                SeleccionarNodo(idSeleccionar.Value);
            else
                LimpiarDetalle();

            CargarPadres(ObtenerPermisoSeleccionado());
            ActualizarBotones();
        }

        private TreeNode CrearNodo(PermisoComponent permiso)
        {
            TreeNode nodo = new TreeNode(FormatearNodo(permiso));
            nodo.Tag = permiso;

            foreach (PermisoComponent hijo in permiso.Hijos.OfType<PermisoComponent>().OrderBy(x => x.Nombre))
                nodo.Nodes.Add(CrearNodo(hijo));

            return nodo;
        }

        private string FormatearNodo(PermisoComponent permiso)
        {
            string tipo = permiso.EsFamilia ? "[F]" : "[P]";
            return tipo + " " + permiso.Nombre + " (" + permiso.Codigo + ")";
        }

        private void CargarTipos()
        {
            bool esFamilia = CBX_Tipo.SelectedIndex == 1;

            CBX_Tipo.Items.Clear();
            CBX_Tipo.Items.Add(T("Permisos.TipoPermiso"));
            CBX_Tipo.Items.Add(T("Permisos.TipoFamilia"));
            SeleccionarTipo(esFamilia);
        }

        private void SeleccionarTipo(bool esFamilia)
        {
            if (CBX_Tipo.Items.Count <= 0)
                return;

            CBX_Tipo.SelectedIndex = esFamilia ? 1 : 0;
        }

        private void CargarPadres(PermisoComponent permisoSeleccionado)
        {
            int idPadreActual = permisoSeleccionado == null
                ? IdRaiz
                : ObtenerIdPadre(permisoSeleccionado.Id);

            _idsPadres.Clear();

            CBX_Padre.Items.Clear();
            CBX_Padre.Items.Add(T("Permisos.Raiz"));
            _idsPadres.Add(IdRaiz);

            CargarFamiliasEnCombo(_permisos, string.Empty);

            SeleccionarPadre(idPadreActual);
        }

        private void CargarFamiliasEnCombo(IEnumerable<PermisoComponent> permisos, string prefijo)
        {
            foreach (PermisoComponent permiso in permisos.OrderBy(x => x.Nombre))
            {
                if (permiso.EsFamilia)
                {
                    CBX_Padre.Items.Add(prefijo + permiso.Nombre);
                    _idsPadres.Add(permiso.Id);
                    CargarFamiliasEnCombo(permiso.Hijos.OfType<PermisoComponent>(), prefijo + "  ");
                }
            }
        }

        private void SeleccionarPadre(int idPadre)
        {
            for (int i = 0; i < _idsPadres.Count; i++)
            {
                if (_idsPadres[i] == idPadre)
                {
                    CBX_Padre.SelectedIndex = i;
                    return;
                }
            }

            if (CBX_Padre.Items.Count > 0)
                CBX_Padre.SelectedIndex = 0;
        }

        private int? ObtenerIdPadreSeleccionado()
        {
            if (CBX_Padre.SelectedIndex < 0 || CBX_Padre.SelectedIndex >= _idsPadres.Count)
                return null;

            int idPadre = _idsPadres[CBX_Padre.SelectedIndex];

            if (idPadre == IdRaiz)
                return null;

            return idPadre;
        }

        private int ObtenerIdPadre(int idPermiso)
        {
            int? idPadre = BuscarIdPadre(_permisos, idPermiso, null);
            return idPadre.HasValue ? idPadre.Value : IdRaiz;
        }

        private int? BuscarIdPadre(IEnumerable<PermisoComponent> permisos, int idPermiso, int? idPadre)
        {
            foreach (PermisoComponent permiso in permisos)
            {
                if (permiso.Id == idPermiso)
                    return idPadre;

                int? idPadreEncontrado = BuscarIdPadre(permiso.Hijos.OfType<PermisoComponent>(), idPermiso, permiso.Id);

                if (idPadreEncontrado.HasValue)
                    return idPadreEncontrado;
            }

            return null;
        }

        private void SeleccionarNodo(int idPermiso)
        {
            TreeNode nodo = BuscarNodo(TVW_Permisos.Nodes, idPermiso);

            if (nodo == null)
                return;

            TVW_Permisos.SelectedNode = nodo;
            nodo.EnsureVisible();
        }

        private TreeNode BuscarNodo(TreeNodeCollection nodos, int idPermiso)
        {
            foreach (TreeNode nodo in nodos)
            {
                PermisoComponent permiso = nodo.Tag as PermisoComponent;

                if (permiso != null && permiso.Id == idPermiso)
                    return nodo;

                TreeNode nodoEncontrado = BuscarNodo(nodo.Nodes, idPermiso);

                if (nodoEncontrado != null)
                    return nodoEncontrado;
            }

            return null;
        }

        private PermisoComponent ObtenerPermisoSeleccionado()
        {
            if (TVW_Permisos.SelectedNode == null)
                return null;

            return TVW_Permisos.SelectedNode.Tag as PermisoComponent;
        }

        private void LimpiarDetalle()
        {
            TBX_Nombre.Clear();
            TBX_Codigo.Clear();
            TBX_Descripcion.Clear();
            SeleccionarTipo(false);
            CargarPadres(null);
            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            bool haySeleccion = ObtenerPermisoSeleccionado() != null;

            BTN_Editar.Enabled = haySeleccion;
            BTN_Eliminar.Enabled = haySeleccion;
            BTN_Mover.Enabled = haySeleccion;
        }

        private void MostrarSeleccionePermiso()
        {
            MessageBox.Show(
                T("Mensaje.SeleccionePermiso"),
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

        private string T(string clave)
        {
            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }

    }
}
