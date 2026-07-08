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

        private void InitializeComponent()
        {
            this.MSP_Principal = new System.Windows.Forms.MenuStrip();
            this.TSMI_Usuario = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_IniciarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_CerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarPermisos = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AsignarPermisosUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_ControlCambios = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Bitacora = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_RecalcularDV = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Idioma = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_AdministrarTraducciones = new System.Windows.Forms.ToolStripMenuItem();
            this.MSP_Principal.SuspendLayout();
            this.SuspendLayout();
            // 
            // MSP_Principal
            // 
            this.MSP_Principal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Usuario,
            this.TSMI_Idioma});
            this.MSP_Principal.Location = new System.Drawing.Point(0, 0);
            this.MSP_Principal.Name = "MSP_Principal";
            this.MSP_Principal.Size = new System.Drawing.Size(984, 24);
            this.MSP_Principal.TabIndex = 0;
            this.MSP_Principal.Text = "menuStrip1";
            // 
            // TSMI_ControlCambios
            // 
            this.TSMI_ControlCambios.Name = "TSMI_ControlCambios";
            this.TSMI_ControlCambios.Size = new System.Drawing.Size(220, 22);
            this.TSMI_ControlCambios.Tag = "Menu.ControlCambios";
            this.TSMI_ControlCambios.Text = "Control de cambios";
            this.TSMI_ControlCambios.Click += new System.EventHandler(this.TSMI_ControlCambios_Click);
            // 
            // TSMI_Usuario
            // 
            this.TSMI_Usuario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_IniciarSesion,
            this.TSMI_CerrarSesion,
            this.TSMI_AdministrarUsuarios,
            this.TSMI_AdministrarPermisos,
            this.TSMI_AsignarPermisosUsuarios,
            this.TSMI_ControlCambios,
            this.TSMI_Bitacora,
            this.TSMI_RecalcularDV});
            this.TSMI_Usuario.Name = "TSMI_Usuario";
            this.TSMI_Usuario.Size = new System.Drawing.Size(59, 20);
            this.TSMI_Usuario.Tag = "Menu.Usuario";
            this.TSMI_Usuario.Text = "Usuario";
            // 
            // TSMI_IniciarSesion
            // 
            this.TSMI_IniciarSesion.Name = "TSMI_IniciarSesion";
            this.TSMI_IniciarSesion.Size = new System.Drawing.Size(180, 22);
            this.TSMI_IniciarSesion.Tag = "Menu.IniciarSesion";
            this.TSMI_IniciarSesion.Text = "Iniciar sesion";
            this.TSMI_IniciarSesion.Click += new System.EventHandler(this.TSMI_IniciarSesion_Click);
            // 
            // TSMI_CerrarSesion
            // 
            this.TSMI_CerrarSesion.Name = "TSMI_CerrarSesion";
            this.TSMI_CerrarSesion.Size = new System.Drawing.Size(180, 22);
            this.TSMI_CerrarSesion.Tag = "Menu.CerrarSesion";
            this.TSMI_CerrarSesion.Text = "Cerrar sesion";
            this.TSMI_CerrarSesion.Click += new System.EventHandler(this.TSMI_CerrarSesion_Click);
            // 
            // TSMI_AdministrarUsuarios
            // 
            this.TSMI_AdministrarUsuarios.Name = "TSMI_AdministrarUsuarios";
            this.TSMI_AdministrarUsuarios.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrarUsuarios.Tag = "Menu.AdministrarUsuarios";
            this.TSMI_AdministrarUsuarios.Text = "Administrar usuarios";
            this.TSMI_AdministrarUsuarios.Click += new System.EventHandler(this.TSMI_AdministrarUsuarios_Click);
            // 
            // TSMI_AdministrarPermisos
            // 
            this.TSMI_AdministrarPermisos.Name = "TSMI_AdministrarPermisos";
            this.TSMI_AdministrarPermisos.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrarPermisos.Tag = "Menu.AdministrarPermisos";
            this.TSMI_AdministrarPermisos.Text = "Administrar permisos";
            this.TSMI_AdministrarPermisos.Click += new System.EventHandler(this.TSMI_AdministrarPermisos_Click);
            // 
            // TSMI_AsignarPermisosUsuarios
            // 
            this.TSMI_AsignarPermisosUsuarios.Name = "TSMI_AsignarPermisosUsuarios";
            this.TSMI_AsignarPermisosUsuarios.Size = new System.Drawing.Size(220, 22);
            this.TSMI_AsignarPermisosUsuarios.Tag = "Menu.AsignarPermisosUsuarios";
            this.TSMI_AsignarPermisosUsuarios.Text = "Asignar permisos a usuarios";
            this.TSMI_AsignarPermisosUsuarios.Click += new System.EventHandler(this.TSMI_AsignarPermisosUsuarios_Click);
            // 
            // TSMI_Bitacora
            // 
            this.TSMI_Bitacora.Name = "TSMI_Bitacora";
            this.TSMI_Bitacora.Size = new System.Drawing.Size(220, 22);
            this.TSMI_Bitacora.Tag = "Menu.Bitacora";
            this.TSMI_Bitacora.Text = "Bitacora de actividades";
            this.TSMI_Bitacora.Click += new System.EventHandler(this.TSMI_Bitacora_Click);
            // 
            // TSMI_RecalcularDV
            // 
            this.TSMI_RecalcularDV.Size = new System.Drawing.Size(220, 22);
            this.TSMI_RecalcularDV.Tag = "Menu.RecalcularDV";
            this.TSMI_RecalcularDV.Text = "Recalcular DV";
            this.TSMI_RecalcularDV.Click += new System.EventHandler(this.TSMI_RecalcularDV_Click);
            // 
            // TSMI_Idioma
            // 
            this.TSMI_Idioma.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_AdministrarTraducciones});
            this.TSMI_Idioma.Name = "TSMI_Idioma";
            this.TSMI_Idioma.Size = new System.Drawing.Size(56, 20);
            this.TSMI_Idioma.Tag = "Menu.Idioma";
            this.TSMI_Idioma.Text = "Idioma";
            // 
            // TSMI_AdministrarTraducciones
            // 
            this.TSMI_AdministrarTraducciones.Name = "TSMI_AdministrarTraducciones";
            this.TSMI_AdministrarTraducciones.Size = new System.Drawing.Size(220, 22);
            this.TSMI_AdministrarTraducciones.Tag = "Menu.AdministrarTraducciones";
            this.TSMI_AdministrarTraducciones.Text = "Administrar traducciones";
            this.TSMI_AdministrarTraducciones.Click += new System.EventHandler(this.TSMI_AdministrarTraducciones_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.MSP_Principal);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MSP_Principal;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmPrincipal.Text";
            this.Text = "Sistema";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.MSP_Principal.ResumeLayout(false);
            this.MSP_Principal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
        private System.Windows.Forms.ToolStripMenuItem TSMI_Idioma;
        private System.Windows.Forms.ToolStripMenuItem TSMI_AdministrarTraducciones;
    }
}
