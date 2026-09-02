namespace UI.Forms.ControlCambios
{
    partial class FrmControlCambios
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
            this.DGV_Cambios = new System.Windows.Forms.DataGridView();
            this.BTN_Restaurar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Cambios)).BeginInit();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(950, 60);
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
            this.LBL_Titulo.Tag = "ControlCambios.Titulo";
            this.LBL_Titulo.Text = "Control de cambios - Traducciones";
            //
            // DGV_Cambios
            //
            this.DGV_Cambios.AllowUserToAddRows = false;
            this.DGV_Cambios.AllowUserToDeleteRows = false;
            this.DGV_Cambios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Cambios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Cambios.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Cambios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Cambios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Cambios.Location = new System.Drawing.Point(15, 80);
            this.DGV_Cambios.MultiSelect = false;
            this.DGV_Cambios.Name = "DGV_Cambios";
            this.DGV_Cambios.ReadOnly = true;
            this.DGV_Cambios.RowHeadersVisible = false;
            this.DGV_Cambios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Cambios.Size = new System.Drawing.Size(920, 400);
            this.DGV_Cambios.TabIndex = 1;
            this.DGV_Cambios.SelectionChanged += new System.EventHandler(this.DGV_Cambios_SelectionChanged);
            //
            // BTN_Restaurar
            //
            this.BTN_Restaurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Restaurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Restaurar.FlatAppearance.BorderSize = 0;
            this.BTN_Restaurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Restaurar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Restaurar.ForeColor = System.Drawing.Color.White;
            this.BTN_Restaurar.Location = new System.Drawing.Point(795, 495);
            this.BTN_Restaurar.Name = "BTN_Restaurar";
            this.BTN_Restaurar.Size = new System.Drawing.Size(140, 35);
            this.BTN_Restaurar.TabIndex = 2;
            this.BTN_Restaurar.Tag = "ControlCambios.Restaurar";
            this.BTN_Restaurar.Text = "Restaurar";
            this.BTN_Restaurar.UseVisualStyleBackColor = false;
            this.BTN_Restaurar.Click += new System.EventHandler(this.BTN_Restaurar_Click);
            //
            // FrmControlCambios
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(950, 545);
            this.Controls.Add(this.BTN_Restaurar);
            this.Controls.Add(this.DGV_Cambios);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmControlCambios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmControlCambios.Text";
            this.Text = "Control de cambios";
            this.Load += new System.EventHandler(this.FrmControlCambios_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Cambios)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.DataGridView DGV_Cambios;
        private System.Windows.Forms.Button BTN_Restaurar;
    }
}
