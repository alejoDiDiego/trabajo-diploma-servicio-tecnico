namespace UI.Forms.Compras
{
    partial class FrmCompras
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
            this.LBL_Proveedor = new System.Windows.Forms.Label();
            this.CBO_Proveedor = new System.Windows.Forms.ComboBox();
            this.LBL_Estado = new System.Windows.Forms.Label();
            this.CBO_Estado = new System.Windows.Forms.ComboBox();
            this.DGV_Compras = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Detalle = new System.Windows.Forms.Button();
            this.BTN_Confirmar = new System.Windows.Forms.Button();
            this.BTN_Cancelar = new System.Windows.Forms.Button();
            this.BTN_Anular = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Compras)).BeginInit();
            this.PNL_Botones.SuspendLayout();
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
            this.LBL_Titulo.Tag = "Compras.Titulo";
            this.LBL_Titulo.Text = "Compras";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Proveedor);
            this.PNL_Filtros.Controls.Add(this.CBO_Proveedor);
            this.PNL_Filtros.Controls.Add(this.LBL_Estado);
            this.PNL_Filtros.Controls.Add(this.CBO_Estado);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(950, 50);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Proveedor
            //
            this.LBL_Proveedor.AutoSize = true;
            this.LBL_Proveedor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Proveedor.Location = new System.Drawing.Point(10, 16);
            this.LBL_Proveedor.Name = "LBL_Proveedor";
            this.LBL_Proveedor.Size = new System.Drawing.Size(64, 15);
            this.LBL_Proveedor.TabIndex = 0;
            this.LBL_Proveedor.Tag = "Compras.FiltroProveedor";
            this.LBL_Proveedor.Text = "Proveedor:";
            //
            // CBO_Proveedor
            //
            this.CBO_Proveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Proveedor.Location = new System.Drawing.Point(85, 13);
            this.CBO_Proveedor.Name = "CBO_Proveedor";
            this.CBO_Proveedor.Size = new System.Drawing.Size(280, 21);
            this.CBO_Proveedor.TabIndex = 1;
            this.CBO_Proveedor.SelectedIndexChanged += new System.EventHandler(this.CBO_Proveedor_SelectedIndexChanged);
            //
            // LBL_Estado
            //
            this.LBL_Estado.AutoSize = true;
            this.LBL_Estado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Estado.Location = new System.Drawing.Point(385, 16);
            this.LBL_Estado.Name = "LBL_Estado";
            this.LBL_Estado.Size = new System.Drawing.Size(45, 15);
            this.LBL_Estado.TabIndex = 2;
            this.LBL_Estado.Tag = "Compras.FiltroEstado";
            this.LBL_Estado.Text = "Estado:";
            //
            // CBO_Estado
            //
            this.CBO_Estado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Estado.Location = new System.Drawing.Point(436, 13);
            this.CBO_Estado.Name = "CBO_Estado";
            this.CBO_Estado.Size = new System.Drawing.Size(180, 21);
            this.CBO_Estado.TabIndex = 3;
            this.CBO_Estado.SelectedIndexChanged += new System.EventHandler(this.CBO_Estado_SelectedIndexChanged);
            //
            // DGV_Compras
            //
            this.DGV_Compras.AllowUserToAddRows = false;
            this.DGV_Compras.AllowUserToDeleteRows = false;
            this.DGV_Compras.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Compras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Compras.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Compras.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Compras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Compras.Location = new System.Drawing.Point(15, 140);
            this.DGV_Compras.MultiSelect = false;
            this.DGV_Compras.Name = "DGV_Compras";
            this.DGV_Compras.ReadOnly = true;
            this.DGV_Compras.RowHeadersVisible = false;
            this.DGV_Compras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Compras.Size = new System.Drawing.Size(950, 330);
            this.DGV_Compras.TabIndex = 2;
            this.DGV_Compras.SelectionChanged += new System.EventHandler(this.DGV_Compras_SelectionChanged);
            this.DGV_Compras.DoubleClick += new System.EventHandler(this.DGV_Compras_DoubleClick);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_Crear);
            this.PNL_Botones.Controls.Add(this.BTN_Detalle);
            this.PNL_Botones.Controls.Add(this.BTN_Confirmar);
            this.PNL_Botones.Controls.Add(this.BTN_Cancelar);
            this.PNL_Botones.Controls.Add(this.BTN_Anular);
            this.PNL_Botones.Location = new System.Drawing.Point(15, 480);
            this.PNL_Botones.Name = "PNL_Botones";
            this.PNL_Botones.Size = new System.Drawing.Size(950, 50);
            this.PNL_Botones.TabIndex = 3;
            //
            // BTN_Crear
            //
            this.BTN_Crear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Crear.FlatAppearance.BorderSize = 0;
            this.BTN_Crear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Crear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Crear.ForeColor = System.Drawing.Color.White;
            this.BTN_Crear.Location = new System.Drawing.Point(5, 10);
            this.BTN_Crear.Name = "BTN_Crear";
            this.BTN_Crear.Size = new System.Drawing.Size(120, 30);
            this.BTN_Crear.TabIndex = 0;
            this.BTN_Crear.Tag = "Compras.Crear";
            this.BTN_Crear.Text = "Crear";
            this.BTN_Crear.UseVisualStyleBackColor = false;
            this.BTN_Crear.Click += new System.EventHandler(this.BTN_Crear_Click);
            //
            // BTN_Detalle
            //
            this.BTN_Detalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Detalle.FlatAppearance.BorderSize = 0;
            this.BTN_Detalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Detalle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Detalle.ForeColor = System.Drawing.Color.White;
            this.BTN_Detalle.Location = new System.Drawing.Point(135, 10);
            this.BTN_Detalle.Name = "BTN_Detalle";
            this.BTN_Detalle.Size = new System.Drawing.Size(120, 30);
            this.BTN_Detalle.TabIndex = 1;
            this.BTN_Detalle.Tag = "Compras.VerDetalle";
            this.BTN_Detalle.Text = "Ver detalle";
            this.BTN_Detalle.UseVisualStyleBackColor = false;
            this.BTN_Detalle.Click += new System.EventHandler(this.BTN_Detalle_Click);
            //
            // BTN_Confirmar
            //
            this.BTN_Confirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Confirmar.FlatAppearance.BorderSize = 0;
            this.BTN_Confirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Confirmar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Confirmar.ForeColor = System.Drawing.Color.White;
            this.BTN_Confirmar.Location = new System.Drawing.Point(265, 10);
            this.BTN_Confirmar.Name = "BTN_Confirmar";
            this.BTN_Confirmar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Confirmar.TabIndex = 2;
            this.BTN_Confirmar.Tag = "Compras.Confirmar";
            this.BTN_Confirmar.Text = "Confirmar";
            this.BTN_Confirmar.UseVisualStyleBackColor = false;
            this.BTN_Confirmar.Click += new System.EventHandler(this.BTN_Confirmar_Click);
            //
            // BTN_Cancelar
            //
            this.BTN_Cancelar.BackColor = System.Drawing.Color.Maroon;
            this.BTN_Cancelar.FlatAppearance.BorderSize = 0;
            this.BTN_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Cancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Cancelar.ForeColor = System.Drawing.Color.White;
            this.BTN_Cancelar.Location = new System.Drawing.Point(395, 10);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Cancelar.TabIndex = 3;
            this.BTN_Cancelar.Tag = "Compras.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            this.BTN_Cancelar.Click += new System.EventHandler(this.BTN_Cancelar_Click);
            //
            // BTN_Anular
            //
            this.BTN_Anular.BackColor = System.Drawing.Color.Maroon;
            this.BTN_Anular.FlatAppearance.BorderSize = 0;
            this.BTN_Anular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Anular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Anular.ForeColor = System.Drawing.Color.White;
            this.BTN_Anular.Location = new System.Drawing.Point(525, 10);
            this.BTN_Anular.Name = "BTN_Anular";
            this.BTN_Anular.Size = new System.Drawing.Size(120, 30);
            this.BTN_Anular.TabIndex = 4;
            this.BTN_Anular.Tag = "Compras.Anular";
            this.BTN_Anular.Text = "Anular";
            this.BTN_Anular.UseVisualStyleBackColor = false;
            this.BTN_Anular.Click += new System.EventHandler(this.BTN_Anular_Click);
            //
            // FrmCompras
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(980, 545);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Compras);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmCompras";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmCompras.Text";
            this.Text = "Compras";
            this.Load += new System.EventHandler(this.FrmCompras_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Compras)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Proveedor;
        private System.Windows.Forms.ComboBox CBO_Proveedor;
        private System.Windows.Forms.Label LBL_Estado;
        private System.Windows.Forms.ComboBox CBO_Estado;
        private System.Windows.Forms.DataGridView DGV_Compras;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Detalle;
        private System.Windows.Forms.Button BTN_Confirmar;
        private System.Windows.Forms.Button BTN_Cancelar;
        private System.Windows.Forms.Button BTN_Anular;
    }
}
