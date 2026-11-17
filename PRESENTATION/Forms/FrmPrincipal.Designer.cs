namespace UI.Forms
{
    partial class FrmPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button BTN_Ayuda;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Ayuda;

        private void InitializeComponent()
        {
            this.BTN_Ayuda = new System.Windows.Forms.Button();
            this.MSP_Principal = new System.Windows.Forms.MenuStrip();
            this.TSMI_Usuario = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarBackups = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_IniciarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_CerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AsignarPermisosUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ControlCambios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Bitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_RecalcularDV = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Gestion = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Clientes = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Equipos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Ordenes = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Garantias = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Dashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Reportes = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Catalogos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_TiposEquipo = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Marcas = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Inventario = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Repuestos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Movimientos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Proveedores = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Compras = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Idioma = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarTraducciones = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Ayuda = new System.Windows.Forms.ToolStripMenuItem();
            this.pieAyuda = new System.Windows.Forms.Panel();
            this.MSP_Principal.SuspendLayout();
            this.pieAyuda.SuspendLayout();
            this.SuspendLayout();
            //
            // BTN_Ayuda
            //
            this.BTN_Ayuda.CausesValidation = false;
            this.BTN_Ayuda.Location = new System.Drawing.Point(18, 8);
            this.BTN_Ayuda.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BTN_Ayuda.Name = "BTN_Ayuda";
            this.BTN_Ayuda.Size = new System.Drawing.Size(165, 46);
            this.BTN_Ayuda.TabIndex = 99;
            this.BTN_Ayuda.Tag = "Ayuda.Boton";
            this.BTN_Ayuda.Text = "? Ayuda";
            this.BTN_Ayuda.UseVisualStyleBackColor = true;
            this.BTN_Ayuda.Click += new System.EventHandler(this.BTN_Ayuda_Click);
            //
            // MSP_Principal
            //
            this.MSP_Principal.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.MSP_Principal.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MSP_Principal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Usuario,
            this.TSMI_Gestion,
            this.TSMI_Idioma,
            this.TSMI_Ayuda});
            this.MSP_Principal.Location = new System.Drawing.Point(0, 0);
            this.MSP_Principal.Name = "MSP_Principal";
            this.MSP_Principal.Size = new System.Drawing.Size(1476, 35);
            this.MSP_Principal.TabIndex = 0;
            this.MSP_Principal.Text = "menuStrip1";
            //
            // TSMI_Usuario
            //
            this.TSMI_Usuario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrarBackups,
            this.TSMI_IniciarSesion,
            this.TSMI_CerrarSesion,
            this.TSMI_AdministrarUsuarios,
            this.TSMI_AdministrarPermisos,
            this.TSMI_AsignarPermisosUsuarios,
            this.TSMI_ControlCambios,
            this.TSMI_Bitacora,
            this.TSMI_RecalcularDV});
            this.TSMI_Usuario.Name = "TSMI_Usuario";
            this.TSMI_Usuario.Size = new System.Drawing.Size(88, 29);
            this.TSMI_Usuario.Tag = "Menu.Usuario";
            this.TSMI_Usuario.Text = "Usuario";
            //
            // TSMI_AdministrarBackups
            //
            this.TSMI_AdministrarBackups.Name = "TSMI_AdministrarBackups";
            this.TSMI_AdministrarBackups.Size = new System.Drawing.Size(337, 34);
            this.TSMI_AdministrarBackups.Tag = "Menu.AdministrarBackups";
            this.TSMI_AdministrarBackups.Text = "Administrar Backups";
            this.TSMI_AdministrarBackups.Click += new System.EventHandler(this.TSMI_AdministrarBackups_Click);
            //
            // TSMI_IniciarSesion
            //
            this.TSMI_IniciarSesion.Name = "TSMI_IniciarSesion";
            this.TSMI_IniciarSesion.Size = new System.Drawing.Size(337, 34);
            this.TSMI_IniciarSesion.Tag = "Menu.IniciarSesion";
            this.TSMI_IniciarSesion.Text = "Iniciar sesion";
            this.TSMI_IniciarSesion.Click += new System.EventHandler(this.TSMI_IniciarSesion_Click);
            //
            // TSMI_CerrarSesion
            //
            this.TSMI_CerrarSesion.Name = "TSMI_CerrarSesion";
            this.TSMI_CerrarSesion.Size = new System.Drawing.Size(337, 34);
            this.TSMI_CerrarSesion.Tag = "Menu.CerrarSesion";
            this.TSMI_CerrarSesion.Text = "Cerrar sesion";
            this.TSMI_CerrarSesion.Click += new System.EventHandler(this.TSMI_CerrarSesion_Click);
            //
            // TSMI_AdministrarUsuarios
            //
            this.TSMI_AdministrarUsuarios.Name = "TSMI_AdministrarUsuarios";
            this.TSMI_AdministrarUsuarios.Size = new System.Drawing.Size(337, 34);
            this.TSMI_AdministrarUsuarios.Tag = "Menu.AdministrarUsuarios";
            this.TSMI_AdministrarUsuarios.Text = "Administrar usuarios";
            this.TSMI_AdministrarUsuarios.Click += new System.EventHandler(this.TSMI_AdministrarUsuarios_Click);
            //
            // TSMI_AdministrarPermisos
            //
            this.TSMI_AdministrarPermisos.Name = "TSMI_AdministrarPermisos";
            this.TSMI_AdministrarPermisos.Size = new System.Drawing.Size(337, 34);
            this.TSMI_AdministrarPermisos.Tag = "Menu.AdministrarPermisos";
            this.TSMI_AdministrarPermisos.Text = "Administrar permisos";
            this.TSMI_AdministrarPermisos.Click += new System.EventHandler(this.TSMI_AdministrarPermisos_Click);
            //
            // TSMI_AsignarPermisosUsuarios
            //
            this.TSMI_AsignarPermisosUsuarios.Name = "TSMI_AsignarPermisosUsuarios";
            this.TSMI_AsignarPermisosUsuarios.Size = new System.Drawing.Size(337, 34);
            this.TSMI_AsignarPermisosUsuarios.Tag = "Menu.AsignarPermisosUsuarios";
            this.TSMI_AsignarPermisosUsuarios.Text = "Asignar permisos a usuarios";
            this.TSMI_AsignarPermisosUsuarios.Click += new System.EventHandler(this.TSMI_AsignarPermisosUsuarios_Click);
            //
            // TSMI_ControlCambios
            //
            this.TSMI_ControlCambios.Name = "TSMI_ControlCambios";
            this.TSMI_ControlCambios.Size = new System.Drawing.Size(337, 34);
            this.TSMI_ControlCambios.Tag = "Menu.ControlCambios";
            this.TSMI_ControlCambios.Text = "Control de cambios";
            this.TSMI_ControlCambios.Click += new System.EventHandler(this.TSMI_ControlCambios_Click);
            //
            // TSMI_Bitacora
            //
            this.TSMI_Bitacora.Name = "TSMI_Bitacora";
            this.TSMI_Bitacora.Size = new System.Drawing.Size(337, 34);
            this.TSMI_Bitacora.Tag = "Menu.Bitacora";
            this.TSMI_Bitacora.Text = "Bitacora de actividades";
            this.TSMI_Bitacora.Click += new System.EventHandler(this.TSMI_Bitacora_Click);
            //
            // TSMI_RecalcularDV
            //
            this.TSMI_RecalcularDV.Name = "TSMI_RecalcularDV";
            this.TSMI_RecalcularDV.Size = new System.Drawing.Size(337, 34);
            this.TSMI_RecalcularDV.Tag = "Menu.RecalcularDV";
            this.TSMI_RecalcularDV.Text = "Recalcular DV";
            this.TSMI_RecalcularDV.Click += new System.EventHandler(this.TSMI_RecalcularDV_Click);
            //
            // TSMI_Gestion
            //
            this.TSMI_Gestion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Clientes,
            this.TSMI_Equipos,
            this.TSMI_Ordenes,
            this.TSMI_Garantias,
            this.TSMI_Dashboard,
            this.TSMI_Reportes,
            this.TSMI_Catalogos,
            this.TSMI_Inventario});
            this.TSMI_Gestion.Name = "TSMI_Gestion";
            this.TSMI_Gestion.Size = new System.Drawing.Size(88, 29);
            this.TSMI_Gestion.Tag = "Menu.Gestion";
            this.TSMI_Gestion.Text = "Gestion";
            //
            // TSMI_Clientes
            //
            this.TSMI_Clientes.Name = "TSMI_Clientes";
            this.TSMI_Clientes.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Clientes.Tag = "Menu.Clientes";
            this.TSMI_Clientes.Text = "Clientes";
            this.TSMI_Clientes.Click += new System.EventHandler(this.TSMI_Clientes_Click);
            //
            // TSMI_Equipos
            //
            this.TSMI_Equipos.Name = "TSMI_Equipos";
            this.TSMI_Equipos.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Equipos.Tag = "Menu.Equipos";
            this.TSMI_Equipos.Text = "Equipos";
            this.TSMI_Equipos.Click += new System.EventHandler(this.TSMI_Equipos_Click);
            //
            // TSMI_Ordenes
            //
            this.TSMI_Ordenes.Name = "TSMI_Ordenes";
            this.TSMI_Ordenes.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Ordenes.Tag = "Menu.Ordenes";
            this.TSMI_Ordenes.Text = "Ordenes";
            this.TSMI_Ordenes.Click += new System.EventHandler(this.TSMI_Ordenes_Click);
            //
            // TSMI_Garantias
            //
            this.TSMI_Garantias.Name = "TSMI_Garantias";
            this.TSMI_Garantias.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Garantias.Tag = "Menu.Garantias";
            this.TSMI_Garantias.Text = "Garantias";
            this.TSMI_Garantias.Click += new System.EventHandler(this.TSMI_Garantias_Click);
            //
            // TSMI_Dashboard
            //
            this.TSMI_Dashboard.Name = "TSMI_Dashboard";
            this.TSMI_Dashboard.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Dashboard.Tag = "Menu.Dashboard";
            this.TSMI_Dashboard.Text = "Dashboard";
            this.TSMI_Dashboard.Click += new System.EventHandler(this.TSMI_Dashboard_Click);
            //
            // TSMI_Reportes
            //
            this.TSMI_Reportes.Name = "TSMI_Reportes";
            this.TSMI_Reportes.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Reportes.Tag = "Menu.Reportes";
            this.TSMI_Reportes.Text = "Reportes";
            this.TSMI_Reportes.Click += new System.EventHandler(this.TSMI_Reportes_Click);
            //
            // TSMI_Catalogos
            //
            this.TSMI_Catalogos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_TiposEquipo,
            this.TSMI_Marcas});
            this.TSMI_Catalogos.Name = "TSMI_Catalogos";
            this.TSMI_Catalogos.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Catalogos.Tag = "Menu.Catalogos";
            this.TSMI_Catalogos.Text = "Catalogos";
            //
            // TSMI_TiposEquipo
            //
            this.TSMI_TiposEquipo.Name = "TSMI_TiposEquipo";
            this.TSMI_TiposEquipo.Size = new System.Drawing.Size(243, 34);
            this.TSMI_TiposEquipo.Tag = "Menu.TiposEquipo";
            this.TSMI_TiposEquipo.Text = "Tipos de equipo";
            this.TSMI_TiposEquipo.Click += new System.EventHandler(this.TSMI_TiposEquipo_Click);
            //
            // TSMI_Marcas
            //
            this.TSMI_Marcas.Name = "TSMI_Marcas";
            this.TSMI_Marcas.Size = new System.Drawing.Size(243, 34);
            this.TSMI_Marcas.Tag = "Menu.Marcas";
            this.TSMI_Marcas.Text = "Marcas";
            this.TSMI_Marcas.Click += new System.EventHandler(this.TSMI_Marcas_Click);
            //
            // TSMI_Inventario
            //
            this.TSMI_Inventario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Repuestos,
            this.TSMI_Movimientos,
            this.TSMI_Proveedores,
            this.TSMI_Compras});
            this.TSMI_Inventario.Name = "TSMI_Inventario";
            this.TSMI_Inventario.Size = new System.Drawing.Size(202, 34);
            this.TSMI_Inventario.Tag = "Menu.Inventario";
            this.TSMI_Inventario.Text = "Inventario";
            //
            // TSMI_Repuestos
            //
            this.TSMI_Repuestos.Name = "TSMI_Repuestos";
            this.TSMI_Repuestos.Size = new System.Drawing.Size(290, 34);
            this.TSMI_Repuestos.Tag = "Menu.Repuestos";
            this.TSMI_Repuestos.Text = "Repuestos";
            this.TSMI_Repuestos.Click += new System.EventHandler(this.TSMI_Repuestos_Click);
            //
            // TSMI_Movimientos
            //
            this.TSMI_Movimientos.Name = "TSMI_Movimientos";
            this.TSMI_Movimientos.Size = new System.Drawing.Size(290, 34);
            this.TSMI_Movimientos.Tag = "Menu.MovimientosStock";
            this.TSMI_Movimientos.Text = "Movimientos de stock";
            this.TSMI_Movimientos.Click += new System.EventHandler(this.TSMI_Movimientos_Click);
            //
            // TSMI_Proveedores
            //
            this.TSMI_Proveedores.Name = "TSMI_Proveedores";
            this.TSMI_Proveedores.Size = new System.Drawing.Size(290, 34);
            this.TSMI_Proveedores.Tag = "Menu.Proveedores";
            this.TSMI_Proveedores.Text = "Proveedores";
            this.TSMI_Proveedores.Click += new System.EventHandler(this.TSMI_Proveedores_Click);
            //
            // TSMI_Compras
            //
            this.TSMI_Compras.Name = "TSMI_Compras";
            this.TSMI_Compras.Size = new System.Drawing.Size(290, 34);
            this.TSMI_Compras.Tag = "Menu.Compras";
            this.TSMI_Compras.Text = "Compras";
            this.TSMI_Compras.Click += new System.EventHandler(this.TSMI_Compras_Click);
            //
            // TSMI_Idioma
            //
            this.TSMI_Idioma.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrarTraducciones});
            this.TSMI_Idioma.Name = "TSMI_Idioma";
            this.TSMI_Idioma.Size = new System.Drawing.Size(84, 29);
            this.TSMI_Idioma.Tag = "Menu.Idioma";
            this.TSMI_Idioma.Text = "Idioma";
            //
            // TSMI_AdministrarTraducciones
            //
            this.TSMI_AdministrarTraducciones.Name = "TSMI_AdministrarTraducciones";
            this.TSMI_AdministrarTraducciones.Size = new System.Drawing.Size(311, 34);
            this.TSMI_AdministrarTraducciones.Tag = "Menu.AdministrarTraducciones";
            this.TSMI_AdministrarTraducciones.Text = "Administrar traducciones";
            this.TSMI_AdministrarTraducciones.Click += new System.EventHandler(this.TSMI_AdministrarTraducciones_Click);
            //
            // TSMI_Ayuda
            //
            this.TSMI_Ayuda.Checked = true;
            this.TSMI_Ayuda.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TSMI_Ayuda.Name = "TSMI_Ayuda";
            this.TSMI_Ayuda.Size = new System.Drawing.Size(79, 29);
            this.TSMI_Ayuda.Tag = "Ayuda.Menu";
            this.TSMI_Ayuda.Text = "Ayuda";
            this.TSMI_Ayuda.Click += new System.EventHandler(this.TSMI_Ayuda_Click);
            //
            // pieAyuda
            //
            this.pieAyuda.Controls.Add(this.BTN_Ayuda);
            this.pieAyuda.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pieAyuda.Location = new System.Drawing.Point(0, 801);
            this.pieAyuda.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pieAyuda.Name = "pieAyuda";
            this.pieAyuda.Size = new System.Drawing.Size(1476, 62);
            this.pieAyuda.TabIndex = 2;
            //
            // FrmPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 863);
            this.Controls.Add(this.MSP_Principal);
            this.Controls.Add(this.pieAyuda);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MSP_Principal;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmPrincipal.Text";
            this.Text = "Sistema";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.MSP_Principal.ResumeLayout(false);
            this.MSP_Principal.PerformLayout();
            this.pieAyuda.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripMenuItem TSMI_AdministrarBackups;
        private System.Windows.Forms.MenuStrip MSP_Principal;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Usuario;
        private System.Windows.Forms.ToolStripMenuItem TSMI_IniciarSesion;
        private System.Windows.Forms.ToolStripMenuItem TSMI_CerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AdministrarUsuarios;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AdministrarPermisos;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AsignarPermisosUsuarios;
        private System.Windows.Forms.ToolStripMenuItem TSMI_ControlCambios;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Bitacora;
        private System.Windows.Forms.ToolStripMenuItem TSMI_RecalcularDV;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Gestion;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Clientes;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Equipos;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Ordenes;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Catalogos;
        private System.Windows.Forms.ToolStripMenuItem TSMI_TiposEquipo;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Marcas;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Inventario;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Repuestos;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Movimientos;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Proveedores;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Compras;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Garantias;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Dashboard;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Reportes;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Idioma;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AdministrarTraducciones;
        private System.Windows.Forms.Panel pieAyuda;
    }
}
