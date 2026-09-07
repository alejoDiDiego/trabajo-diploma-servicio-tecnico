namespace UI.Forms.Reportes
{
    partial class FrmReportes
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
            this.LBL_Tipo = new System.Windows.Forms.Label();
            this.CBO_TipoReporte = new System.Windows.Forms.ComboBox();
            this.LBL_Desde = new System.Windows.Forms.Label();
            this.DT_Desde = new System.Windows.Forms.DateTimePicker();
            this.LBL_Hasta = new System.Windows.Forms.Label();
            this.DT_Hasta = new System.Windows.Forms.DateTimePicker();
            this.LBL_Top = new System.Windows.Forms.Label();
            this.NUM_Top = new System.Windows.Forms.NumericUpDown();
            this.BTN_Buscar = new System.Windows.Forms.Button();
            this.DGV_Reporte = new System.Windows.Forms.DataGridView();
            this.LBL_Resultado = new System.Windows.Forms.Label();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Reporte)).BeginInit();
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
            this.LBL_Titulo.Size = new System.Drawing.Size(200, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Reportes.Titulo";
            this.LBL_Titulo.Text = "Reportes";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Tipo);
            this.PNL_Filtros.Controls.Add(this.CBO_TipoReporte);
            this.PNL_Filtros.Controls.Add(this.LBL_Desde);
            this.PNL_Filtros.Controls.Add(this.DT_Desde);
            this.PNL_Filtros.Controls.Add(this.LBL_Hasta);
            this.PNL_Filtros.Controls.Add(this.DT_Hasta);
            this.PNL_Filtros.Controls.Add(this.LBL_Top);
            this.PNL_Filtros.Controls.Add(this.NUM_Top);
            this.PNL_Filtros.Controls.Add(this.BTN_Buscar);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(1020, 78);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Tipo
            //
            this.LBL_Tipo.AutoSize = true;
            this.LBL_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Tipo.Location = new System.Drawing.Point(10, 14);
            this.LBL_Tipo.Name = "LBL_Tipo";
            this.LBL_Tipo.Size = new System.Drawing.Size(49, 15);
            this.LBL_Tipo.TabIndex = 0;
            this.LBL_Tipo.Tag = "Reportes.Tipo";
            this.LBL_Tipo.Text = "Reporte:";
            //
            // CBO_TipoReporte
            //
            this.CBO_TipoReporte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_TipoReporte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CBO_TipoReporte.Location = new System.Drawing.Point(70, 11);
            this.CBO_TipoReporte.Name = "CBO_TipoReporte";
            this.CBO_TipoReporte.Size = new System.Drawing.Size(290, 23);
            this.CBO_TipoReporte.TabIndex = 1;
            this.CBO_TipoReporte.SelectedIndexChanged += new System.EventHandler(this.CBO_TipoReporte_SelectedIndexChanged);
            //
            // LBL_Desde
            //
            this.LBL_Desde.AutoSize = true;
            this.LBL_Desde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Desde.Location = new System.Drawing.Point(375, 14);
            this.LBL_Desde.Name = "LBL_Desde";
            this.LBL_Desde.Size = new System.Drawing.Size(44, 15);
            this.LBL_Desde.TabIndex = 2;
            this.LBL_Desde.Tag = "Reportes.Desde";
            this.LBL_Desde.Text = "Desde:";
            //
            // DT_Desde
            //
            this.DT_Desde.Checked = false;
            this.DT_Desde.CustomFormat = "dd/MM/yyyy";
            this.DT_Desde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DT_Desde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Desde.Location = new System.Drawing.Point(425, 11);
            this.DT_Desde.Name = "DT_Desde";
            this.DT_Desde.ShowCheckBox = true;
            this.DT_Desde.Size = new System.Drawing.Size(140, 23);
            this.DT_Desde.TabIndex = 3;
            //
            // LBL_Hasta
            //
            this.LBL_Hasta.AutoSize = true;
            this.LBL_Hasta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Hasta.Location = new System.Drawing.Point(580, 14);
            this.LBL_Hasta.Name = "LBL_Hasta";
            this.LBL_Hasta.Size = new System.Drawing.Size(41, 15);
            this.LBL_Hasta.TabIndex = 4;
            this.LBL_Hasta.Tag = "Reportes.Hasta";
            this.LBL_Hasta.Text = "Hasta:";
            //
            // DT_Hasta
            //
            this.DT_Hasta.Checked = false;
            this.DT_Hasta.CustomFormat = "dd/MM/yyyy";
            this.DT_Hasta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.DT_Hasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Hasta.Location = new System.Drawing.Point(627, 11);
            this.DT_Hasta.Name = "DT_Hasta";
            this.DT_Hasta.ShowCheckBox = true;
            this.DT_Hasta.Size = new System.Drawing.Size(140, 23);
            this.DT_Hasta.TabIndex = 5;
            //
            // LBL_Top
            //
            this.LBL_Top.AutoSize = true;
            this.LBL_Top.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Top.Location = new System.Drawing.Point(10, 48);
            this.LBL_Top.Name = "LBL_Top";
            this.LBL_Top.Size = new System.Drawing.Size(30, 15);
            this.LBL_Top.TabIndex = 6;
            this.LBL_Top.Tag = "Reportes.Top";
            this.LBL_Top.Text = "Top:";
            //
            // NUM_Top
            //
            this.NUM_Top.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.NUM_Top.Location = new System.Drawing.Point(70, 45);
            this.NUM_Top.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUM_Top.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_Top.Name = "NUM_Top";
            this.NUM_Top.Size = new System.Drawing.Size(80, 23);
            this.NUM_Top.TabIndex = 7;
            this.NUM_Top.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // BTN_Buscar
            //
            this.BTN_Buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Buscar.FlatAppearance.BorderSize = 0;
            this.BTN_Buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Buscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Buscar.ForeColor = System.Drawing.Color.White;
            this.BTN_Buscar.Location = new System.Drawing.Point(891, 40);
            this.BTN_Buscar.Name = "BTN_Buscar";
            this.BTN_Buscar.Size = new System.Drawing.Size(100, 28);
            this.BTN_Buscar.TabIndex = 8;
            this.BTN_Buscar.Tag = "Reportes.Buscar";
            this.BTN_Buscar.Text = "Buscar";
            this.BTN_Buscar.UseVisualStyleBackColor = false;
            this.BTN_Buscar.Click += new System.EventHandler(this.BTN_Buscar_Click);
            //
            // DGV_Reporte
            //
            this.DGV_Reporte.AllowUserToAddRows = false;
            this.DGV_Reporte.AllowUserToDeleteRows = false;
            this.DGV_Reporte.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Reporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Reporte.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Reporte.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Reporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Reporte.Location = new System.Drawing.Point(15, 165);
            this.DGV_Reporte.MultiSelect = false;
            this.DGV_Reporte.Name = "DGV_Reporte";
            this.DGV_Reporte.ReadOnly = true;
            this.DGV_Reporte.RowHeadersVisible = false;
            this.DGV_Reporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Reporte.Size = new System.Drawing.Size(1020, 330);
            this.DGV_Reporte.TabIndex = 2;
            //
            // LBL_Resultado
            //
            this.LBL_Resultado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_Resultado.AutoSize = false;
            this.LBL_Resultado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.LBL_Resultado.Location = new System.Drawing.Point(15, 505);
            this.LBL_Resultado.Name = "LBL_Resultado";
            this.LBL_Resultado.Size = new System.Drawing.Size(1020, 25);
            this.LBL_Resultado.TabIndex = 3;
            this.LBL_Resultado.Tag = "Reportes.ResultadoVacio";
            this.LBL_Resultado.Text = "";
            //
            // FrmReportes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.LBL_Resultado);
            this.Controls.Add(this.DGV_Reporte);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmReportes.Text";
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.FrmReportes_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Reporte)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Tipo;
        private System.Windows.Forms.ComboBox CBO_TipoReporte;
        private System.Windows.Forms.Label LBL_Desde;
        private System.Windows.Forms.DateTimePicker DT_Desde;
        private System.Windows.Forms.Label LBL_Hasta;
        private System.Windows.Forms.DateTimePicker DT_Hasta;
        private System.Windows.Forms.Label LBL_Top;
        private System.Windows.Forms.NumericUpDown NUM_Top;
        private System.Windows.Forms.Button BTN_Buscar;
        private System.Windows.Forms.DataGridView DGV_Reporte;
        private System.Windows.Forms.Label LBL_Resultado;
    }
}
