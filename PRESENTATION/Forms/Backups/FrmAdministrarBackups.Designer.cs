namespace UI.Forms.Backups
{
    partial class FrmAdministrarBackups
    {
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Restaurar;
        private System.Windows.Forms.Label LBL_Aviso;
        private System.Windows.Forms.Label LBL_Estado;
        private System.Windows.Forms.ProgressBar PRG_Actividad;
        private System.Windows.Forms.Button BTN_Ayuda;

        private void InitializeComponent()
        {
            this.BTN_Ayuda = new System.Windows.Forms.Button();
            var panel = new System.Windows.Forms.TableLayoutPanel();
            BTN_Crear = new System.Windows.Forms.Button();
            BTN_Restaurar = new System.Windows.Forms.Button();
            LBL_Aviso = new System.Windows.Forms.Label();
            LBL_Estado = new System.Windows.Forms.Label();
            PRG_Actividad = new System.Windows.Forms.ProgressBar();
            SuspendLayout();
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Padding = new System.Windows.Forms.Padding(18);
            panel.ColumnCount = 2; panel.RowCount = 5;
            panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
            panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46));
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22));
            LBL_Aviso.Text = "Backup completo de la base configurada. Restaurar requiere espacio para otra base y mantenimiento exclusivo. La original se conserva; SQL RESTORE no ofrece rollback transaccional. Cerrar durante una operacion espera su finalizacion segura.";
            LBL_Aviso.Dock = System.Windows.Forms.DockStyle.Fill; LBL_Aviso.Tag = "Backups.Aviso";
            panel.Controls.Add(LBL_Aviso, 0, 0); panel.SetColumnSpan(LBL_Aviso, 2);
            BTN_Crear.Text = "&Crear backup";
            BTN_Crear.Name = "BTN_Crear"; BTN_Crear.Tag = "Backups.Crear"; BTN_Crear.Dock = System.Windows.Forms.DockStyle.Fill;
            BTN_Crear.TabIndex = 0; BTN_Crear.Click += BTN_Crear_Click;
            BTN_Restaurar.Text = "&Restaurar backup";
            BTN_Restaurar.Name = "BTN_Restaurar"; BTN_Restaurar.Tag = "Backups.Restaurar"; BTN_Restaurar.Dock = System.Windows.Forms.DockStyle.Fill;
            BTN_Restaurar.TabIndex = 1; BTN_Restaurar.Click += BTN_Restaurar_Click;
            panel.Controls.Add(BTN_Crear, 0, 1); panel.Controls.Add(BTN_Restaurar, 1, 1);
            LBL_Estado.Text = "Listo.";
            LBL_Estado.Name = "LBL_Estado"; LBL_Estado.Tag = "Backups.Listo"; LBL_Estado.Dock = System.Windows.Forms.DockStyle.Fill;
            LBL_Estado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            panel.Controls.Add(LBL_Estado, 0, 2); panel.SetColumnSpan(LBL_Estado, 2);
            PRG_Actividad.Name = "PRG_Actividad"; PRG_Actividad.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Controls.Add(PRG_Actividad, 0, 3); panel.SetColumnSpan(PRG_Actividad, 2);
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(580, 310); MinimumSize = new System.Drawing.Size(500, 340);
            this.Text = "Administrar Backups";
            Controls.Add(panel); Name = "FrmAdministrarBackups"; Tag = "Menu.AdministrarBackups";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            MaximizeBox = false; MinimizeBox = false;
            this.BTN_Ayuda.Name = "BTN_Ayuda";
            this.BTN_Ayuda.Tag = "Ayuda.Boton";
            this.BTN_Ayuda.Text = "? Ayuda";
            this.BTN_Ayuda.Size = new System.Drawing.Size(110, 30);
            this.BTN_Ayuda.TabIndex = 99;
            this.BTN_Ayuda.CausesValidation = false;
            this.BTN_Ayuda.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BTN_Ayuda.UseVisualStyleBackColor = true;
            this.BTN_Ayuda.Click += this.BTN_Ayuda_Click;
            panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40));
            panel.Controls.Add(this.BTN_Ayuda, 0, 4);
            this.BTN_Ayuda.Anchor = System.Windows.Forms.AnchorStyles.Left;
            ResumeLayout(false);
        }
    }
}
