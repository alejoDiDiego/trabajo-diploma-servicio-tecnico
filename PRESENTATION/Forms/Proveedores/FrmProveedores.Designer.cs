namespace UI.Forms.Proveedores
{
    partial class FrmProveedores
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
            this.LBL_RazonSocial = new System.Windows.Forms.Label();
            this.TXT_RazonSocial = new System.Windows.Forms.TextBox();
            this.LBL_Cuit = new System.Windows.Forms.Label();
            this.TXT_Cuit = new System.Windows.Forms.TextBox();
            this.CHK_Inactivos = new System.Windows.Forms.CheckBox();
            this.DGV_Proveedores = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Editar = new System.Windows.Forms.Button();
            this.BTN_Desactivar = new System.Windows.Forms.Button();
            this.BTN_Reactivar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Proveedores)).BeginInit();
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
            this.PNL_Header.Size = new System.Drawing.Size(920, 60);
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
            this.LBL_Titulo.Tag = "Proveedores.Titulo";
            this.LBL_Titulo.Text = "Proveedores";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_RazonSocial);
            this.PNL_Filtros.Controls.Add(this.TXT_RazonSocial);
            this.PNL_Filtros.Controls.Add(this.LBL_Cuit);
            this.PNL_Filtros.Controls.Add(this.TXT_Cuit);
            this.PNL_Filtros.Controls.Add(this.CHK_Inactivos);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(890, 78);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_RazonSocial
            //
            this.LBL_RazonSocial.AutoSize = true;
            this.LBL_RazonSocial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RazonSocial.Location = new System.Drawing.Point(10, 14);
            this.LBL_RazonSocial.Name = "LBL_RazonSocial";
            this.LBL_RazonSocial.Size = new System.Drawing.Size(80, 15);
            this.LBL_RazonSocial.TabIndex = 0;
            this.LBL_RazonSocial.Tag = "Proveedores.FiltroRazonSocial";
            this.LBL_RazonSocial.Text = "Razon social:";
            //
            // TXT_RazonSocial
            //
            this.TXT_RazonSocial.Location = new System.Drawing.Point(110, 11);
            this.TXT_RazonSocial.Name = "TXT_RazonSocial";
            this.TXT_RazonSocial.Size = new System.Drawing.Size(220, 22);
            this.TXT_RazonSocial.TabIndex = 1;
            this.TXT_RazonSocial.TextChanged += new System.EventHandler(this.TXT_Filtro_TextChanged);
            //
            // LBL_Cuit
            //
            this.LBL_Cuit.AutoSize = true;
            this.LBL_Cuit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cuit.Location = new System.Drawing.Point(350, 14);
            this.LBL_Cuit.Name = "LBL_Cuit";
            this.LBL_Cuit.Size = new System.Drawing.Size(35, 15);
            this.LBL_Cuit.TabIndex = 2;
            this.LBL_Cuit.Tag = "Proveedores.FiltroCuit";
            this.LBL_Cuit.Text = "Cuit:";
            //
            // TXT_Cuit
            //
            this.TXT_Cuit.Location = new System.Drawing.Point(395, 11);
            this.TXT_Cuit.Name = "TXT_Cuit";
            this.TXT_Cuit.Size = new System.Drawing.Size(180, 22);
            this.TXT_Cuit.TabIndex = 3;
            this.TXT_Cuit.TextChanged += new System.EventHandler(this.TXT_Filtro_TextChanged);
            //
            // CHK_Inactivos
            //
            this.CHK_Inactivos.AutoSize = true;
            this.CHK_Inactivos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_Inactivos.Location = new System.Drawing.Point(13, 46);
            this.CHK_Inactivos.Name = "CHK_Inactivos";
            this.CHK_Inactivos.Size = new System.Drawing.Size(95, 19);
            this.CHK_Inactivos.TabIndex = 4;
            this.CHK_Inactivos.Tag = "Proveedores.VerInactivos";
            this.CHK_Inactivos.Text = "Ver inactivos";
            this.CHK_Inactivos.UseVisualStyleBackColor = true;
            this.CHK_Inactivos.CheckedChanged += new System.EventHandler(this.CHK_Inactivos_CheckedChanged);
            //
            // DGV_Proveedores
            //
            this.DGV_Proveedores.AllowUserToAddRows = false;
            this.DGV_Proveedores.AllowUserToDeleteRows = false;
            this.DGV_Proveedores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Proveedores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Proveedores.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Proveedores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Proveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Proveedores.Location = new System.Drawing.Point(15, 165);
            this.DGV_Proveedores.MultiSelect = false;
            this.DGV_Proveedores.Name = "DGV_Proveedores";
            this.DGV_Proveedores.ReadOnly = true;
            this.DGV_Proveedores.RowHeadersVisible = false;
            this.DGV_Proveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Proveedores.Size = new System.Drawing.Size(890, 280);
            this.DGV_Proveedores.TabIndex = 2;
            this.DGV_Proveedores.SelectionChanged += new System.EventHandler(this.DGV_Proveedores_SelectionChanged);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_Crear);
            this.PNL_Botones.Controls.Add(this.BTN_Editar);
            this.PNL_Botones.Controls.Add(this.BTN_Desactivar);
            this.PNL_Botones.Controls.Add(this.BTN_Reactivar);
            this.PNL_Botones.Location = new System.Drawing.Point(15, 455);
            this.PNL_Botones.Name = "PNL_Botones";
            this.PNL_Botones.Size = new System.Drawing.Size(890, 50);
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
            this.BTN_Crear.Size = new System.Drawing.Size(110, 30);
            this.BTN_Crear.TabIndex = 0;
            this.BTN_Crear.Tag = "Proveedores.Crear";
            this.BTN_Crear.Text = "Crear";
            this.BTN_Crear.UseVisualStyleBackColor = false;
            this.BTN_Crear.Click += new System.EventHandler(this.BTN_Crear_Click);
            //
            // BTN_Editar
            //
            this.BTN_Editar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Editar.FlatAppearance.BorderSize = 0;
            this.BTN_Editar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Editar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Editar.ForeColor = System.Drawing.Color.White;
            this.BTN_Editar.Location = new System.Drawing.Point(125, 10);
            this.BTN_Editar.Name = "BTN_Editar";
            this.BTN_Editar.Size = new System.Drawing.Size(110, 30);
            this.BTN_Editar.TabIndex = 1;
            this.BTN_Editar.Tag = "Proveedores.Editar";
            this.BTN_Editar.Text = "Editar";
            this.BTN_Editar.UseVisualStyleBackColor = false;
            this.BTN_Editar.Click += new System.EventHandler(this.BTN_Editar_Click);
            //
            // BTN_Desactivar
            //
            this.BTN_Desactivar.BackColor = System.Drawing.Color.Maroon;
            this.BTN_Desactivar.FlatAppearance.BorderSize = 0;
            this.BTN_Desactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Desactivar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Desactivar.ForeColor = System.Drawing.Color.White;
            this.BTN_Desactivar.Location = new System.Drawing.Point(245, 10);
            this.BTN_Desactivar.Name = "BTN_Desactivar";
            this.BTN_Desactivar.Size = new System.Drawing.Size(110, 30);
            this.BTN_Desactivar.TabIndex = 2;
            this.BTN_Desactivar.Tag = "Proveedores.Desactivar";
            this.BTN_Desactivar.Text = "Desactivar";
            this.BTN_Desactivar.UseVisualStyleBackColor = false;
            this.BTN_Desactivar.Click += new System.EventHandler(this.BTN_Desactivar_Click);
            //
            // BTN_Reactivar
            //
            this.BTN_Reactivar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.BTN_Reactivar.FlatAppearance.BorderSize = 0;
            this.BTN_Reactivar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Reactivar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Reactivar.ForeColor = System.Drawing.Color.White;
            this.BTN_Reactivar.Location = new System.Drawing.Point(365, 10);
            this.BTN_Reactivar.Name = "BTN_Reactivar";
            this.BTN_Reactivar.Size = new System.Drawing.Size(110, 30);
            this.BTN_Reactivar.TabIndex = 3;
            this.BTN_Reactivar.Tag = "Proveedores.Reactivar";
            this.BTN_Reactivar.Text = "Reactivar";
            this.BTN_Reactivar.UseVisualStyleBackColor = false;
            this.BTN_Reactivar.Click += new System.EventHandler(this.BTN_Reactivar_Click);
            //
            // FrmProveedores
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(920, 520);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Proveedores);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmProveedores.Text";
            this.Text = "Proveedores";
            this.Load += new System.EventHandler(this.FrmProveedores_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Proveedores)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_RazonSocial;
        private System.Windows.Forms.TextBox TXT_RazonSocial;
        private System.Windows.Forms.Label LBL_Cuit;
        private System.Windows.Forms.TextBox TXT_Cuit;
        private System.Windows.Forms.CheckBox CHK_Inactivos;
        private System.Windows.Forms.DataGridView DGV_Proveedores;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Editar;
        private System.Windows.Forms.Button BTN_Desactivar;
        private System.Windows.Forms.Button BTN_Reactivar;
    }
}
