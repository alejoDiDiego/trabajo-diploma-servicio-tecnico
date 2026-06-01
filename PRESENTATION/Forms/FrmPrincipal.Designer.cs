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
            this.MSP_Principal.SuspendLayout();
            this.SuspendLayout();
            // 
            // MSP_Principal
            // 
            this.MSP_Principal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Usuario});
            this.MSP_Principal.Location = new System.Drawing.Point(0, 0);
            this.MSP_Principal.Name = "MSP_Principal";
            this.MSP_Principal.Size = new System.Drawing.Size(984, 24);
            this.MSP_Principal.TabIndex = 0;
            this.MSP_Principal.Text = "menuStrip1";
            // 
            // TSMI_Usuario
            // 
            this.TSMI_Usuario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_IniciarSesion,
            this.TSMI_CerrarSesion,
            this.TSMI_AdministrarUsuarios});
            this.TSMI_Usuario.Name = "TSMI_Usuario";
            this.TSMI_Usuario.Size = new System.Drawing.Size(59, 20);
            this.TSMI_Usuario.Text = "Usuario";
            // 
            // TSMI_IniciarSesion
            // 
            this.TSMI_IniciarSesion.Name = "TSMI_IniciarSesion";
            this.TSMI_IniciarSesion.Size = new System.Drawing.Size(180, 22);
            this.TSMI_IniciarSesion.Text = "Iniciar sesion";
            this.TSMI_IniciarSesion.Click += new System.EventHandler(this.TSMI_IniciarSesion_Click);
            // 
            // TSMI_CerrarSesion
            // 
            this.TSMI_CerrarSesion.Name = "TSMI_CerrarSesion";
            this.TSMI_CerrarSesion.Size = new System.Drawing.Size(180, 22);
            this.TSMI_CerrarSesion.Text = "Cerrar sesion";
            this.TSMI_CerrarSesion.Click += new System.EventHandler(this.TSMI_CerrarSesion_Click);
            // 
            // TSMI_AdministrarUsuarios
            // 
            this.TSMI_AdministrarUsuarios.Name = "TSMI_AdministrarUsuarios";
            this.TSMI_AdministrarUsuarios.Size = new System.Drawing.Size(180, 22);
            this.TSMI_AdministrarUsuarios.Text = "Administrar usuarios";
            this.TSMI_AdministrarUsuarios.Click += new System.EventHandler(this.TSMI_AdministrarUsuarios_Click);
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
    }
}
