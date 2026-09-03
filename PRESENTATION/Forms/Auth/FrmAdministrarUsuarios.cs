using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Auth
{
    public partial class FrmAdministrarUsuarios : Form, IObservador
    {
        private BindingList<Usuario> _usuariosBindingList = null;
        private readonly SesionIdioma _sesionIdioma;
        // T1: boton creado por codigo para no tocar el Designer. No usa claves i18n nuevas.
        private Button _btnReactivar;

        public FrmAdministrarUsuarios()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            BTN_CerrarSesion.Text = idiomaObservado.BuscarTraduccion(BTN_CerrarSesion.Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            BTN_CrearUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_CrearUsuario.Tag.ToString());
            label2.Text = idiomaObservado.BuscarTraduccion(label2.Tag.ToString());
            label1.Text = idiomaObservado.BuscarTraduccion(label1.Tag.ToString());
            LBL_NuevoUsuario.Text = idiomaObservado.BuscarTraduccion(LBL_NuevoUsuario.Tag.ToString());
            BTN_EliminarUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarUsuario.Tag.ToString());
            BTN_EditarUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_EditarUsuario.Tag.ToString());

            ActualizarDatosSesion();
            ConfigurarColumnasUsuarios();
            ActualizarBotonReactivar();
        }

        private void ActualizarLista() {
            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
            ConfigurarColumnasUsuarios();
            AplicarPermisos();
        }

        private void FrmAdministrarCuentas_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            AsegurarBotonReactivar();
            Actualizar(_sesionIdioma.idioma);

            BTN_CerrarSesion.Visible = false;

            if (!PuedeVerUsuarios())
            {
                PNL_Permisos.Visible = false;
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            PNL_Permisos.Visible = true;
            ActualizarDatosSesion();

            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
            ConfigurarColumnasUsuarios();
            AplicarPermisos();
        }

        private void BTN_CrearUsuario_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.UsuariosCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                Usuario usuario = usuarioService.Crear(TBX_Username.Text, TBX_Password.Text);
                _usuariosBindingList.Add(usuario);

                TBX_Username.Clear();
                TBX_Password.Clear();

                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioCreado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorCrearUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BTN_CerrarSesion_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DGV_Usuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_Usuarios.SelectedRows.Count == 0)
            {
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;
                if (_btnReactivar != null)
                    _btnReactivar.Enabled = false;
                return;
            }

            BTN_EliminarUsuario.Enabled = true;
            BTN_EditarUsuario.Enabled = true;

            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            TBX_Username.Text = usuarioSeleccionado.Username;
            AplicarPermisos();
        }

        private void BTN_EliminarUsuario_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.UsuariosEliminar))
            {
                MostrarAccesoDenegado();
                return;
            }

            SessionManager sesion = SessionManager.GetInstance();
            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            if (sesion.Usuario.Id == usuarioSeleccionado.Id)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.NoEliminarPropio"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            var confirmResult = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEliminarUsuario").Replace("{0}", usuarioSeleccionado.Username),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                // T1: baja logica, la fila sigue existiendo con activo=0.
                usuarioService.Eliminar(usuarioSeleccionado.Username);

                ActualizarLista();

                TBX_Username.Clear();
                TBX_Password.Clear();

                DGV_Usuarios.ClearSelection();
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;
                AplicarPermisos();

                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioEliminado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorEliminarUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BTN_ReactivarUsuario_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.UsuariosEliminar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (DGV_Usuarios.SelectedRows.Count == 0)
                return;

            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                usuarioService.Reactivar(usuarioSeleccionado.Username);

                ActualizarLista();

                bool esIngles = EsIngles();
                MessageBox.Show(
                    esIngles ? "User reactivated successfully." : "Usuario reactivado exitosamente.",
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool esIngles = EsIngles();
                string detalle = esIngles ? "Error reactivating user: {0}" : "Error al reactivar usuario: {0}";
                MessageBox.Show(
                    detalle.Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BTN_EditarUsuario_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.UsuariosEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            var confirmResult = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEditarUsuario").Replace("{0}", usuarioSeleccionado.Username),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEdicion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                Usuario usuarioModificado = usuarioService.Modificar(usuarioSeleccionado.Id, TBX_Username.Text, TBX_Password.Text);

                TBX_Username.Clear();
                TBX_Password.Clear();

                DGV_Usuarios.ClearSelection();
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;
                ActualizarLista();
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioEditado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorEditarUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        private void ActualizarDatosSesion()
        {
            if (!SessionManager.HaySesionActiva())
                return;

            SessionManager sesion = SessionManager.GetInstance();
            Usuario usuario = (Usuario)sesion.Usuario;

            LBL_Username.Text = _sesionIdioma.idioma.BuscarTraduccion("AdministrarUsuarios.UsuarioActual").Replace("{0}", usuario.Username);
            LBL_FechaInicio.Text = _sesionIdioma.idioma.BuscarTraduccion("AdministrarUsuarios.SesionIniciada").Replace("{0}", sesion.FechaInicio.ToString());
        }

        private void ConfigurarColumnasUsuarios()
        {
            // T1: Password es el hash, se oculta siempre sin romper el binding.
            if (DGV_Usuarios.Columns.Contains("Password"))
                DGV_Usuarios.Columns["Password"].Visible = false;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Username", "Columna.Username");

            // T1: columna Activo visible. "Columna.Activo" no tiene seed (prohibido agregar),
            // BuscarTraduccion devuelve la clave si falta, por eso hay fallback por codigo.
            if (DGV_Usuarios.Columns.Contains("Activo"))
            {
                DGV_Usuarios.Columns["Activo"].Visible = true;
                string header = _sesionIdioma.idioma.BuscarTraduccion("Columna.Activo");
                if (string.Equals(header, "Columna.Activo", StringComparison.OrdinalIgnoreCase))
                    header = EsIngles() ? "Active" : "Activo";
                DGV_Usuarios.Columns["Activo"].Tag = "Columna.Activo";
                DGV_Usuarios.Columns["Activo"].HeaderText = header;
            }

            // DVH interno: no se muestra al operador.
            if (DGV_Usuarios.Columns.Contains("DVH"))
                DGV_Usuarios.Columns["DVH"].Visible = false;
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Usuarios.Columns.Contains(nombreColumna))
                return;

            DGV_Usuarios.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Usuarios.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void AsegurarBotonReactivar()
        {
            // T1: boton creado por codigo para no tocar el Designer. Reutiliza Tag existente.
            if (_btnReactivar != null)
                return;

            _btnReactivar = new Button();
            _btnReactivar.Name = "BTN_ReactivarUsuario";
            _btnReactivar.Size = BTN_EliminarUsuario.Size;
            // T1: misma fila que Eliminar, a su izquierda (594,383): no solapa con Editar (661,438) ni Crear (495,439).
            _btnReactivar.Location = new System.Drawing.Point(594, 383);
            _btnReactivar.FlatStyle = BTN_EliminarUsuario.FlatStyle;
            _btnReactivar.FlatAppearance.BorderSize = 0;
            _btnReactivar.Font = BTN_EliminarUsuario.Font;
            _btnReactivar.ForeColor = BTN_EliminarUsuario.ForeColor;
            _btnReactivar.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            _btnReactivar.Tag = BTN_EliminarUsuario.Tag;
            _btnReactivar.UseVisualStyleBackColor = false;
            _btnReactivar.Click += new EventHandler(BTN_ReactivarUsuario_Click);
            PNL_Permisos.Controls.Add(_btnReactivar);
            ActualizarBotonReactivar();
        }

        private void ActualizarBotonReactivar()
        {
            if (_btnReactivar == null)
                return;

            Usuario seleccionado = UsuarioSeleccionado();

            bool puedeCambiar = TienePermiso(CodigosPermiso.UsuariosEliminar);
            _btnReactivar.Visible = puedeCambiar;

            if (seleccionado == null)
            {
                _btnReactivar.Enabled = false;
                _btnReactivar.Text = EsIngles() ? "Reactivate user" : "Reactivar usuario";
                return;
            }

            bool esPropio = SessionManager.HaySesionActiva()
                && SessionManager.GetInstance().Usuario.Id == seleccionado.Id;
            _btnReactivar.Enabled = puedeCambiar && !esPropio && seleccionado.Activo == false;
            _btnReactivar.Text = EsIngles() ? "Reactivate user" : "Reactivar usuario";
        }

        private Usuario UsuarioSeleccionado()
        {
            if (DGV_Usuarios.SelectedRows.Count == 0)
                return null;

            return DGV_Usuarios.SelectedRows[0].DataBoundItem as Usuario;
        }

        private bool EsIngles()
        {
            return _sesionIdioma.idioma != null
                && string.Equals(_sesionIdioma.idioma.Nombre, "Ingles", StringComparison.OrdinalIgnoreCase);
        }

        private void AplicarPermisos()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.UsuariosCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.UsuariosEditar);
            bool puedeEliminar = TienePermiso(CodigosPermiso.UsuariosEliminar);
            bool haySeleccion = DGV_Usuarios.SelectedRows.Count > 0;

            BTN_CrearUsuario.Visible = puedeCrear;
            BTN_EditarUsuario.Visible = puedeEditar;
            BTN_EliminarUsuario.Visible = puedeEliminar;

            BTN_CrearUsuario.Enabled = puedeCrear;
            BTN_EditarUsuario.Enabled = puedeEditar && haySeleccion;
            BTN_EliminarUsuario.Enabled = puedeEliminar && haySeleccion;

            TBX_Username.Enabled = puedeCrear || puedeEditar;
            TBX_Password.Enabled = puedeCrear || puedeEditar;

            ActualizarBotonReactivar();
        }

        private bool PuedeVerUsuarios()
        {
            return TienePermiso(CodigosPermiso.UsuariosVer);
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
