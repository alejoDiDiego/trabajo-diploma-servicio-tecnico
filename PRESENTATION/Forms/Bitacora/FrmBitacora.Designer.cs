namespace UI.Forms.Bitacora
{
    partial class FrmBitacora
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
            this.PNL_Header = new System.Windows.Forms.Panel();
            this.LBL_Titulo = new System.Windows.Forms.Label();
            this.PNL_Filtros = new System.Windows.Forms.Panel();
            this.LBL_UsuarioFiltro = new System.Windows.Forms.Label();
            this.TXT_Usuario = new System.Windows.Forms.TextBox();
            this.LBL_Desde = new System.Windows.Forms.Label();
            this.DT_Desde = new System.Windows.Forms.DateTimePicker();
            this.LBL_Hasta = new System.Windows.Forms.Label();
            this.DT_Hasta = new System.Windows.Forms.DateTimePicker();
            this.LBL_TipoFiltro = new System.Windows.Forms.Label();
            this.CBO_TipoActividad = new System.Windows.Forms.ComboBox();
            this.BTN_Buscar = new System.Windows.Forms.Button();
            this.DGV_Bitacora = new System.Windows.Forms.DataGridView();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Bitacora)).BeginInit();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(1050, 60);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Titulo
            //
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(256, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Bitacora.Titulo";
            this.LBL_Titulo.Text = "Bitacora de actividades";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_UsuarioFiltro);
            this.PNL_Filtros.Controls.Add(this.TXT_Usuario);
            this.PNL_Filtros.Controls.Add(this.LBL_Desde);
            this.PNL_Filtros.Controls.Add(this.DT_Desde);
            this.PNL_Filtros.Controls.Add(this.LBL_Hasta);
            this.PNL_Filtros.Controls.Add(this.DT_Hasta);
            this.PNL_Filtros.Controls.Add(this.LBL_TipoFiltro);
            this.PNL_Filtros.Controls.Add(this.CBO_TipoActividad);
            this.PNL_Filtros.Controls.Add(this.BTN_Buscar);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(1020, 50);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_UsuarioFiltro
            //
            this.LBL_UsuarioFiltro.AutoSize = true;
            this.LBL_UsuarioFiltro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_UsuarioFiltro.Location = new System.Drawing.Point(10, 16);
            this.LBL_UsuarioFiltro.Name = "LBL_UsuarioFiltro";
            this.LBL_UsuarioFiltro.Size = new System.Drawing.Size(50, 15);
            this.LBL_UsuarioFiltro.TabIndex = 0;
            this.LBL_UsuarioFiltro.Tag = "Bitacora.UsuarioFiltro";
            this.LBL_UsuarioFiltro.Text = "Usuario:";
            //
            // TXT_Usuario
            //
            this.TXT_Usuario.Location = new System.Drawing.Point(65, 13);
            this.TXT_Usuario.Name = "TXT_Usuario";
            this.TXT_Usuario.Size = new System.Drawing.Size(120, 22);
            this.TXT_Usuario.TabIndex = 1;
            //
            // LBL_Desde
            //
            this.LBL_Desde.AutoSize = true;
            this.LBL_Desde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Desde.Location = new System.Drawing.Point(200, 16);
            this.LBL_Desde.Name = "LBL_Desde";
            this.LBL_Desde.Size = new System.Drawing.Size(44, 15);
            this.LBL_Desde.TabIndex = 2;
            this.LBL_Desde.Tag = "Bitacora.Desde";
            this.LBL_Desde.Text = "Desde:";
            //
            // DT_Desde
            //
            this.DT_Desde.Checked = false;
            this.DT_Desde.CustomFormat = "dd/MM/yyyy HH:mm";
            this.DT_Desde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Desde.Location = new System.Drawing.Point(250, 13);
            this.DT_Desde.Name = "DT_Desde";
            this.DT_Desde.ShowCheckBox = true;
            this.DT_Desde.Size = new System.Drawing.Size(150, 22);
            this.DT_Desde.TabIndex = 3;
            //
            // LBL_Hasta
            //
            this.LBL_Hasta.AutoSize = true;
            this.LBL_Hasta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Hasta.Location = new System.Drawing.Point(415, 16);
            this.LBL_Hasta.Name = "LBL_Hasta";
            this.LBL_Hasta.Size = new System.Drawing.Size(41, 15);
            this.LBL_Hasta.TabIndex = 4;
            this.LBL_Hasta.Tag = "Bitacora.Hasta";
            this.LBL_Hasta.Text = "Hasta:";
            //
            // DT_Hasta
            //
            this.DT_Hasta.Checked = false;
            this.DT_Hasta.CustomFormat = "dd/MM/yyyy HH:mm";
            this.DT_Hasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Hasta.Location = new System.Drawing.Point(460, 13);
            this.DT_Hasta.Name = "DT_Hasta";
            this.DT_Hasta.ShowCheckBox = true;
            this.DT_Hasta.Size = new System.Drawing.Size(150, 22);
            this.DT_Hasta.TabIndex = 5;
            //
            // LBL_TipoFiltro
            //
            this.LBL_TipoFiltro.AutoSize = true;
            this.LBL_TipoFiltro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_TipoFiltro.Location = new System.Drawing.Point(625, 16);
            this.LBL_TipoFiltro.Name = "LBL_TipoFiltro";
            this.LBL_TipoFiltro.Size = new System.Drawing.Size(34, 15);
            this.LBL_TipoFiltro.TabIndex = 6;
            this.LBL_TipoFiltro.Tag = "Bitacora.TipoFiltro";
            this.LBL_TipoFiltro.Text = "Tipo:";
            //
            // CBO_TipoActividad
            //
            this.CBO_TipoActividad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_TipoActividad.Location = new System.Drawing.Point(665, 13);
            this.CBO_TipoActividad.Name = "CBO_TipoActividad";
            this.CBO_TipoActividad.Size = new System.Drawing.Size(160, 21);
            this.CBO_TipoActividad.TabIndex = 7;
            //
            // BTN_Buscar
            //
            this.BTN_Buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Buscar.FlatAppearance.BorderSize = 0;
            this.BTN_Buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Buscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Buscar.ForeColor = System.Drawing.Color.White;
            this.BTN_Buscar.Location = new System.Drawing.Point(845, 10);
            this.BTN_Buscar.Name = "BTN_Buscar";
            this.BTN_Buscar.Size = new System.Drawing.Size(100, 28);
            this.BTN_Buscar.TabIndex = 8;
            this.BTN_Buscar.Tag = "Bitacora.Buscar";
            this.BTN_Buscar.Text = "Buscar";
            this.BTN_Buscar.UseVisualStyleBackColor = false;
            this.BTN_Buscar.Click += new System.EventHandler(this.BTN_Buscar_Click);
            //
            // DGV_Bitacora
            //
            this.DGV_Bitacora.AllowUserToAddRows = false;
            this.DGV_Bitacora.AllowUserToDeleteRows = false;
            this.DGV_Bitacora.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Bitacora.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Bitacora.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Bitacora.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Bitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Bitacora.Location = new System.Drawing.Point(15, 140);
            this.DGV_Bitacora.MultiSelect = false;
            this.DGV_Bitacora.Name = "DGV_Bitacora";
            this.DGV_Bitacora.ReadOnly = true;
            this.DGV_Bitacora.RowHeadersVisible = false;
            this.DGV_Bitacora.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Bitacora.Size = new System.Drawing.Size(1020, 380);
            this.DGV_Bitacora.TabIndex = 2;
            //
            // FrmBitacora
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.DGV_Bitacora);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmBitacora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmBitacora.Text";
            this.Text = "Bitacora de actividades";
            this.Load += new System.EventHandler(this.FrmBitacora_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Bitacora)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_UsuarioFiltro;
        private System.Windows.Forms.TextBox TXT_Usuario;
        private System.Windows.Forms.Label LBL_Desde;
        private System.Windows.Forms.DateTimePicker DT_Desde;
        private System.Windows.Forms.Label LBL_Hasta;
        private System.Windows.Forms.DateTimePicker DT_Hasta;
        private System.Windows.Forms.Label LBL_TipoFiltro;
        private System.Windows.Forms.ComboBox CBO_TipoActividad;
        private System.Windows.Forms.Button BTN_Buscar;
        private System.Windows.Forms.DataGridView DGV_Bitacora;
    }
}
