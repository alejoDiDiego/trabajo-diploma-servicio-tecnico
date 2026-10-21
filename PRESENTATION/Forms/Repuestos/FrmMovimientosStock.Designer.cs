namespace UI.Forms.Repuestos
{
    partial class FrmMovimientosStock
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
            this.LBL_Repuesto = new System.Windows.Forms.Label();
            this.CBO_Repuesto = new System.Windows.Forms.ComboBox();
            this.LBL_Tipo = new System.Windows.Forms.Label();
            this.CBO_Tipo = new System.Windows.Forms.ComboBox();
            this.LBL_Desde = new System.Windows.Forms.Label();
            this.DT_Desde = new System.Windows.Forms.DateTimePicker();
            this.LBL_Hasta = new System.Windows.Forms.Label();
            this.DT_Hasta = new System.Windows.Forms.DateTimePicker();
            this.LBL_Observacion = new System.Windows.Forms.Label();
            this.TXT_Observacion = new System.Windows.Forms.TextBox();
            this.BTN_Buscar = new System.Windows.Forms.Button();
            this.DGV_Movimientos = new System.Windows.Forms.DataGridView();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Movimientos)).BeginInit();
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
            this.LBL_Titulo.Size = new System.Drawing.Size(220, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Movimientos.Titulo";
            this.LBL_Titulo.Text = "Movimientos de stock";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Repuesto);
            this.PNL_Filtros.Controls.Add(this.CBO_Repuesto);
            this.PNL_Filtros.Controls.Add(this.LBL_Tipo);
            this.PNL_Filtros.Controls.Add(this.CBO_Tipo);
            this.PNL_Filtros.Controls.Add(this.LBL_Desde);
            this.PNL_Filtros.Controls.Add(this.DT_Desde);
            this.PNL_Filtros.Controls.Add(this.LBL_Hasta);
            this.PNL_Filtros.Controls.Add(this.DT_Hasta);
            this.PNL_Filtros.Controls.Add(this.LBL_Observacion);
            this.PNL_Filtros.Controls.Add(this.TXT_Observacion);
            this.PNL_Filtros.Controls.Add(this.BTN_Buscar);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(1020, 78);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Repuesto
            //
            this.LBL_Repuesto.AutoSize = true;
            this.LBL_Repuesto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Repuesto.Location = new System.Drawing.Point(10, 14);
            this.LBL_Repuesto.Name = "LBL_Repuesto";
            this.LBL_Repuesto.Size = new System.Drawing.Size(61, 15);
            this.LBL_Repuesto.TabIndex = 0;
            this.LBL_Repuesto.Tag = "Movimientos.FiltroRepuesto";
            this.LBL_Repuesto.Text = "Repuesto:";
            //
            // CBO_Repuesto
            //
            this.CBO_Repuesto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Repuesto.Location = new System.Drawing.Point(80, 11);
            this.CBO_Repuesto.Name = "CBO_Repuesto";
            this.CBO_Repuesto.Size = new System.Drawing.Size(220, 21);
            this.CBO_Repuesto.TabIndex = 1;
            //
            // LBL_Tipo
            //
            this.LBL_Tipo.AutoSize = true;
            this.LBL_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Tipo.Location = new System.Drawing.Point(315, 14);
            this.LBL_Tipo.Name = "LBL_Tipo";
            this.LBL_Tipo.Size = new System.Drawing.Size(33, 15);
            this.LBL_Tipo.TabIndex = 2;
            this.LBL_Tipo.Tag = "Movimientos.FiltroTipo";
            this.LBL_Tipo.Text = "Tipo:";
            //
            // CBO_Tipo
            //
            this.CBO_Tipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Tipo.Location = new System.Drawing.Point(354, 11);
            this.CBO_Tipo.Name = "CBO_Tipo";
            this.CBO_Tipo.Size = new System.Drawing.Size(170, 21);
            this.CBO_Tipo.TabIndex = 3;
            //
            // LBL_Desde
            //
            this.LBL_Desde.AutoSize = true;
            this.LBL_Desde.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Desde.Location = new System.Drawing.Point(539, 14);
            this.LBL_Desde.Name = "LBL_Desde";
            this.LBL_Desde.Size = new System.Drawing.Size(44, 15);
            this.LBL_Desde.TabIndex = 4;
            this.LBL_Desde.Tag = "Movimientos.Desde";
            this.LBL_Desde.Text = "Desde:";
            //
            // DT_Desde
            //
            this.DT_Desde.Checked = false;
            this.DT_Desde.CustomFormat = "dd/MM/yyyy HH:mm";
            this.DT_Desde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Desde.Location = new System.Drawing.Point(589, 11);
            this.DT_Desde.Name = "DT_Desde";
            this.DT_Desde.ShowCheckBox = true;
            this.DT_Desde.Size = new System.Drawing.Size(150, 22);
            this.DT_Desde.TabIndex = 5;
            //
            // LBL_Hasta
            //
            this.LBL_Hasta.AutoSize = true;
            this.LBL_Hasta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Hasta.Location = new System.Drawing.Point(754, 14);
            this.LBL_Hasta.Name = "LBL_Hasta";
            this.LBL_Hasta.Size = new System.Drawing.Size(41, 15);
            this.LBL_Hasta.TabIndex = 6;
            this.LBL_Hasta.Tag = "Movimientos.Hasta";
            this.LBL_Hasta.Text = "Hasta:";
            //
            // DT_Hasta
            //
            this.DT_Hasta.Checked = false;
            this.DT_Hasta.CustomFormat = "dd/MM/yyyy HH:mm";
            this.DT_Hasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DT_Hasta.Location = new System.Drawing.Point(801, 11);
            this.DT_Hasta.Name = "DT_Hasta";
            this.DT_Hasta.ShowCheckBox = true;
            this.DT_Hasta.Size = new System.Drawing.Size(150, 22);
            this.DT_Hasta.TabIndex = 7;
            //
            // LBL_Observacion
            //
            this.LBL_Observacion.AutoSize = true;
            this.LBL_Observacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Observacion.Location = new System.Drawing.Point(10, 48);
            this.LBL_Observacion.Name = "LBL_Observacion";
            this.LBL_Observacion.Size = new System.Drawing.Size(76, 15);
            this.LBL_Observacion.TabIndex = 8;
            this.LBL_Observacion.Tag = "Movimientos.Observacion";
            this.LBL_Observacion.Text = "Observacion:";
            //
            // TXT_Observacion
            //
            this.TXT_Observacion.Location = new System.Drawing.Point(92, 45);
            this.TXT_Observacion.Name = "TXT_Observacion";
            this.TXT_Observacion.Size = new System.Drawing.Size(432, 22);
            this.TXT_Observacion.TabIndex = 9;
            //
            // BTN_Buscar
            //
            this.BTN_Buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Buscar.FlatAppearance.BorderSize = 0;
            this.BTN_Buscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Buscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Buscar.ForeColor = System.Drawing.Color.White;
            this.BTN_Buscar.Location = new System.Drawing.Point(851, 42);
            this.BTN_Buscar.Name = "BTN_Buscar";
            this.BTN_Buscar.Size = new System.Drawing.Size(100, 28);
            this.BTN_Buscar.TabIndex = 10;
            this.BTN_Buscar.Tag = "Movimientos.Buscar";
            this.BTN_Buscar.Text = "Buscar";
            this.BTN_Buscar.UseVisualStyleBackColor = false;
            this.BTN_Buscar.Click += new System.EventHandler(this.BTN_Buscar_Click);
            //
            // DGV_Movimientos
            //
            this.DGV_Movimientos.AllowUserToAddRows = false;
            this.DGV_Movimientos.AllowUserToDeleteRows = false;
            this.DGV_Movimientos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Movimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Movimientos.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Movimientos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Movimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Movimientos.Location = new System.Drawing.Point(15, 165);
            this.DGV_Movimientos.MultiSelect = false;
            this.DGV_Movimientos.Name = "DGV_Movimientos";
            this.DGV_Movimientos.ReadOnly = true;
            this.DGV_Movimientos.RowHeadersVisible = false;
            this.DGV_Movimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Movimientos.Size = new System.Drawing.Size(1020, 360);
            this.DGV_Movimientos.TabIndex = 2;
            //
            // FrmMovimientosStock
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.DGV_Movimientos);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmMovimientosStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmMovimientosStock.Text";
            this.Text = "Movimientos de stock";
            this.Load += new System.EventHandler(this.FrmMovimientosStock_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Movimientos)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Repuesto;
        private System.Windows.Forms.ComboBox CBO_Repuesto;
        private System.Windows.Forms.Label LBL_Tipo;
        private System.Windows.Forms.ComboBox CBO_Tipo;
        private System.Windows.Forms.Label LBL_Desde;
        private System.Windows.Forms.DateTimePicker DT_Desde;
        private System.Windows.Forms.Label LBL_Hasta;
        private System.Windows.Forms.DateTimePicker DT_Hasta;
        private System.Windows.Forms.Label LBL_Observacion;
        private System.Windows.Forms.TextBox TXT_Observacion;
        private System.Windows.Forms.Button BTN_Buscar;
        private System.Windows.Forms.DataGridView DGV_Movimientos;
    }
}
