namespace UI.Forms.Ayuda
{
    partial class FrmAyuda
    {
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();
        private System.Windows.Forms.WebBrowser WEB_Manual;
        private System.Windows.Forms.Button BTN_Indice;
        private System.Windows.Forms.Button BTN_Reintentar;
        private System.Windows.Forms.Button BTN_Cerrar;
        private System.Windows.Forms.Label LBL_Aviso;
        private System.Windows.Forms.Label LBL_Error;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var pie = new System.Windows.Forms.FlowLayoutPanel();
            WEB_Manual = new System.Windows.Forms.WebBrowser();
            BTN_Indice = new System.Windows.Forms.Button();
            BTN_Reintentar = new System.Windows.Forms.Button();
            BTN_Cerrar = new System.Windows.Forms.Button();
            LBL_Aviso = new System.Windows.Forms.Label();
            LBL_Error = new System.Windows.Forms.Label();
            SuspendLayout();
            WEB_Manual.Dock = System.Windows.Forms.DockStyle.Fill;
            WEB_Manual.Name = "WEB_Manual";
            WEB_Manual.AllowWebBrowserDrop = false;
            WEB_Manual.IsWebBrowserContextMenuEnabled = false;
            WEB_Manual.WebBrowserShortcutsEnabled = false;
            WEB_Manual.ScriptErrorsSuppressed = true;
            WEB_Manual.Navigating += WEB_Manual_Navigating;
            WEB_Manual.NewWindow += WEB_Manual_NewWindow;
            WEB_Manual.DocumentCompleted += WEB_Manual_DocumentCompleted;
            WEB_Manual.FileDownload += WEB_Manual_FileDownload;
            LBL_Aviso.Dock = System.Windows.Forms.DockStyle.Top;
            LBL_Aviso.Height = 36;
            LBL_Aviso.Tag = "Ayuda.ContenidoEspanol";
            LBL_Aviso.Text = "El contenido del manual está en español.";
            LBL_Aviso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            LBL_Error.Dock = System.Windows.Forms.DockStyle.Top;
            LBL_Error.Height = 52;
            LBL_Error.Visible = false;
            LBL_Error.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            pie.Dock = System.Windows.Forms.DockStyle.Bottom;
            pie.Height = 46;
            pie.Padding = new System.Windows.Forms.Padding(8);
            BTN_Indice.Name = "BTN_Indice";
            BTN_Indice.Tag = "Ayuda.Indice";
            BTN_Indice.Text = "Índice";
            BTN_Indice.Width = 120;
            BTN_Indice.CausesValidation = false;
            BTN_Indice.Click += BTN_Indice_Click;
            BTN_Reintentar.Name = "BTN_Reintentar";
            BTN_Reintentar.Tag = "Ayuda.Reintentar";
            BTN_Reintentar.Text = "Reintentar";
            BTN_Reintentar.Width = 120;
            BTN_Reintentar.CausesValidation = false;
            BTN_Reintentar.Click += BTN_Reintentar_Click;
            BTN_Cerrar.Name = "BTN_Cerrar";
            BTN_Cerrar.Tag = "Accion.Cerrar";
            BTN_Cerrar.Text = "Cerrar";
            BTN_Cerrar.Width = 120;
            BTN_Cerrar.CausesValidation = false;
            BTN_Cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            pie.Controls.Add(BTN_Indice);
            pie.Controls.Add(BTN_Reintentar);
            pie.Controls.Add(BTN_Cerrar);
            Controls.Add(WEB_Manual);
            Controls.Add(LBL_Error);
            Controls.Add(LBL_Aviso);
            Controls.Add(pie);
            CancelButton = BTN_Cerrar;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 650);
            MinimumSize = new System.Drawing.Size(480, 360);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            Name = "FrmAyuda";
            Tag = "Ayuda.Titulo";
            Text = "Manual de ayuda";
            ResumeLayout(false);
        }
    }
}