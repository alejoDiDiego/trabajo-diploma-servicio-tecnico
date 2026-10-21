namespace UI.Forms.Garantias
{
    partial class FrmGarantias
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
            this.LBL_Vigencia = new System.Windows.Forms.Label();
            this.CBO_Vigencia = new System.Windows.Forms.ComboBox();
            this.LBL_Orden = new System.Windows.Forms.Label();
            this.TXT_Orden = new System.Windows.Forms.TextBox();
            this.DGV_Garantias = new System.Windows.Forms.DataGridView();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Garantias)).BeginInit();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(980, 60);
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
            this.LBL_Titulo.Tag = "Garantias.Titulo";
            this.LBL_Titulo.Text = "Garantias";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Vigencia);
            this.PNL_Filtros.Controls.Add(this.CBO_Vigencia);
            this.PNL_Filtros.Controls.Add(this.LBL_Orden);
            this.PNL_Filtros.Controls.Add(this.TXT_Orden);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(950, 50);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Vigencia
            //
            this.LBL_Vigencia.AutoSize = true;
            this.LBL_Vigencia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Vigencia.Location = new System.Drawing.Point(10, 16);
            this.LBL_Vigencia.Name = "LBL_Vigencia";
            this.LBL_Vigencia.Size = new System.Drawing.Size(57, 15);
            this.LBL_Vigencia.TabIndex = 0;
            this.LBL_Vigencia.Tag = "Garantias.FiltroVigencia";
            this.LBL_Vigencia.Text = "Vigencia:";
            //
            // CBO_Vigencia
            //
            this.CBO_Vigencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Vigencia.Location = new System.Drawing.Point(80, 13);
            this.CBO_Vigencia.Name = "CBO_Vigencia";
            this.CBO_Vigencia.Size = new System.Drawing.Size(180, 21);
            this.CBO_Vigencia.TabIndex = 1;
            this.CBO_Vigencia.SelectedIndexChanged += new System.EventHandler(this.CBO_Vigencia_SelectedIndexChanged);
            //
            // LBL_Orden
            //
            this.LBL_Orden.AutoSize = true;
            this.LBL_Orden.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Orden.Location = new System.Drawing.Point(280, 16);
            this.LBL_Orden.Name = "LBL_Orden";
            this.LBL_Orden.Size = new System.Drawing.Size(48, 15);
            this.LBL_Orden.TabIndex = 2;
            this.LBL_Orden.Tag = "Garantias.FiltroOrden";
            this.LBL_Orden.Text = "Orden:";
            //
            // TXT_Orden
            //
            this.TXT_Orden.Location = new System.Drawing.Point(334, 13);
            this.TXT_Orden.Name = "TXT_Orden";
            this.TXT_Orden.Size = new System.Drawing.Size(150, 22);
            this.TXT_Orden.TabIndex = 3;
            this.TXT_Orden.TextChanged += new System.EventHandler(this.TXT_Orden_TextChanged);
            //
            // DGV_Garantias
            //
            this.DGV_Garantias.AllowUserToAddRows = false;
            this.DGV_Garantias.AllowUserToDeleteRows = false;
            this.DGV_Garantias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Garantias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Garantias.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Garantias.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Garantias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Garantias.Location = new System.Drawing.Point(15, 140);
            this.DGV_Garantias.MultiSelect = false;
            this.DGV_Garantias.Name = "DGV_Garantias";
            this.DGV_Garantias.ReadOnly = true;
            this.DGV_Garantias.RowHeadersVisible = false;
            this.DGV_Garantias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Garantias.Size = new System.Drawing.Size(950, 390);
            this.DGV_Garantias.TabIndex = 2;
            //
            // FrmGarantias
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(980, 545);
            this.Controls.Add(this.DGV_Garantias);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmGarantias";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmGarantias.Text";
            this.Text = "Garantias";
            this.Load += new System.EventHandler(this.FrmGarantias_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Garantias)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Vigencia;
        private System.Windows.Forms.ComboBox CBO_Vigencia;
        private System.Windows.Forms.Label LBL_Orden;
        private System.Windows.Forms.TextBox TXT_Orden;
        private System.Windows.Forms.DataGridView DGV_Garantias;
    }
}
