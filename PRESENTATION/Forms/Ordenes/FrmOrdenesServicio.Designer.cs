namespace UI.Forms.Ordenes
{
    partial class FrmOrdenesServicio
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
            this.LBL_Cliente = new System.Windows.Forms.Label();
            this.CBO_Cliente = new System.Windows.Forms.ComboBox();
            this.LBL_Estado = new System.Windows.Forms.Label();
            this.CBO_Estado = new System.Windows.Forms.ComboBox();
            this.LBL_Busqueda = new System.Windows.Forms.Label();
            this.TXT_Busqueda = new System.Windows.Forms.TextBox();
            this.CHK_Entregadas = new System.Windows.Forms.CheckBox();
            this.DGV_Ordenes = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Detalle = new System.Windows.Forms.Button();
            this.BTN_Cancelar = new System.Windows.Forms.Button();
            this.BTN_Entregar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Ordenes)).BeginInit();
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
            this.LBL_Titulo.Tag = "Ordenes.Titulo";
            this.LBL_Titulo.Text = "Administracion de ordenes";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Cliente);
            this.PNL_Filtros.Controls.Add(this.CBO_Cliente);
            this.PNL_Filtros.Controls.Add(this.LBL_Estado);
            this.PNL_Filtros.Controls.Add(this.CBO_Estado);
            this.PNL_Filtros.Controls.Add(this.LBL_Busqueda);
            this.PNL_Filtros.Controls.Add(this.TXT_Busqueda);
            this.PNL_Filtros.Controls.Add(this.CHK_Entregadas);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(1020, 50);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Cliente
            //
            this.LBL_Cliente.AutoSize = true;
            this.LBL_Cliente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cliente.Location = new System.Drawing.Point(10, 16);
            this.LBL_Cliente.Name = "LBL_Cliente";
            this.LBL_Cliente.Size = new System.Drawing.Size(47, 15);
            this.LBL_Cliente.TabIndex = 0;
            this.LBL_Cliente.Tag = "Ordenes.FiltroCliente";
            this.LBL_Cliente.Text = "Cliente:";
            //
            // CBO_Cliente
            //
            this.CBO_Cliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Cliente.Location = new System.Drawing.Point(63, 13);
            this.CBO_Cliente.Name = "CBO_Cliente";
            this.CBO_Cliente.Size = new System.Drawing.Size(180, 21);
            this.CBO_Cliente.TabIndex = 1;
            this.CBO_Cliente.SelectedIndexChanged += new System.EventHandler(this.CBO_Cliente_SelectedIndexChanged);
            //
            // LBL_Estado
            //
            this.LBL_Estado.AutoSize = true;
            this.LBL_Estado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Estado.Location = new System.Drawing.Point(255, 16);
            this.LBL_Estado.Name = "LBL_Estado";
            this.LBL_Estado.Size = new System.Drawing.Size(45, 15);
            this.LBL_Estado.TabIndex = 2;
            this.LBL_Estado.Tag = "Ordenes.FiltroEstado";
            this.LBL_Estado.Text = "Estado:";
            //
            // CBO_Estado
            //
            this.CBO_Estado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Estado.Location = new System.Drawing.Point(306, 13);
            this.CBO_Estado.Name = "CBO_Estado";
            this.CBO_Estado.Size = new System.Drawing.Size(165, 21);
            this.CBO_Estado.TabIndex = 3;
            this.CBO_Estado.SelectedIndexChanged += new System.EventHandler(this.CBO_Estado_SelectedIndexChanged);
            //
            // LBL_Busqueda
            //
            this.LBL_Busqueda.AutoSize = true;
            this.LBL_Busqueda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Busqueda.Location = new System.Drawing.Point(483, 16);
            this.LBL_Busqueda.Name = "LBL_Busqueda";
            this.LBL_Busqueda.Size = new System.Drawing.Size(110, 15);
            this.LBL_Busqueda.TabIndex = 4;
            this.LBL_Busqueda.Tag = "Ordenes.FiltroTexto";
            this.LBL_Busqueda.Text = "Numero / Problema:";
            //
            // TXT_Busqueda
            //
            this.TXT_Busqueda.Location = new System.Drawing.Point(599, 13);
            this.TXT_Busqueda.Name = "TXT_Busqueda";
            this.TXT_Busqueda.Size = new System.Drawing.Size(180, 22);
            this.TXT_Busqueda.TabIndex = 5;
            this.TXT_Busqueda.TextChanged += new System.EventHandler(this.TXT_Busqueda_TextChanged);
            //
            // CHK_Entregadas
            //
            this.CHK_Entregadas.AutoSize = true;
            this.CHK_Entregadas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_Entregadas.Location = new System.Drawing.Point(795, 15);
            this.CHK_Entregadas.Name = "CHK_Entregadas";
            this.CHK_Entregadas.Size = new System.Drawing.Size(110, 19);
            this.CHK_Entregadas.TabIndex = 6;
            this.CHK_Entregadas.Tag = "Ordenes.VerEntregadas";
            this.CHK_Entregadas.Text = "Ver entregadas";
            this.CHK_Entregadas.UseVisualStyleBackColor = true;
            this.CHK_Entregadas.CheckedChanged += new System.EventHandler(this.CHK_Entregadas_CheckedChanged);
            //
            // DGV_Ordenes
            //
            this.DGV_Ordenes.AllowUserToAddRows = false;
            this.DGV_Ordenes.AllowUserToDeleteRows = false;
            this.DGV_Ordenes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Ordenes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Ordenes.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Ordenes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Ordenes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Ordenes.Location = new System.Drawing.Point(15, 140);
            this.DGV_Ordenes.MultiSelect = false;
            this.DGV_Ordenes.Name = "DGV_Ordenes";
            this.DGV_Ordenes.ReadOnly = true;
            this.DGV_Ordenes.RowHeadersVisible = false;
            this.DGV_Ordenes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Ordenes.Size = new System.Drawing.Size(1020, 330);
            this.DGV_Ordenes.TabIndex = 2;
            this.DGV_Ordenes.SelectionChanged += new System.EventHandler(this.DGV_Ordenes_SelectionChanged);
            this.DGV_Ordenes.DoubleClick += new System.EventHandler(this.DGV_Ordenes_DoubleClick);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_Crear);
            this.PNL_Botones.Controls.Add(this.BTN_Detalle);
            this.PNL_Botones.Controls.Add(this.BTN_Cancelar);
            this.PNL_Botones.Controls.Add(this.BTN_Entregar);
            this.PNL_Botones.Location = new System.Drawing.Point(15, 480);
            this.PNL_Botones.Name = "PNL_Botones";
            this.PNL_Botones.Size = new System.Drawing.Size(1020, 50);
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
            this.BTN_Crear.Tag = "Ordenes.Crear";
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
            this.BTN_Detalle.Tag = "Ordenes.VerDetalle";
            this.BTN_Detalle.Text = "Ver detalle";
            this.BTN_Detalle.UseVisualStyleBackColor = false;
            this.BTN_Detalle.Click += new System.EventHandler(this.BTN_Detalle_Click);
            //
            // BTN_Cancelar
            //
            this.BTN_Cancelar.BackColor = System.Drawing.Color.Maroon;
            this.BTN_Cancelar.FlatAppearance.BorderSize = 0;
            this.BTN_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Cancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Cancelar.ForeColor = System.Drawing.Color.White;
            this.BTN_Cancelar.Location = new System.Drawing.Point(265, 10);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Cancelar.TabIndex = 2;
            this.BTN_Cancelar.Tag = "Ordenes.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            this.BTN_Cancelar.Click += new System.EventHandler(this.BTN_Cancelar_Click);
            //
            // BTN_Entregar
            //
            this.BTN_Entregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.BTN_Entregar.FlatAppearance.BorderSize = 0;
            this.BTN_Entregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Entregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Entregar.ForeColor = System.Drawing.Color.White;
            this.BTN_Entregar.Location = new System.Drawing.Point(395, 10);
            this.BTN_Entregar.Name = "BTN_Entregar";
            this.BTN_Entregar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Entregar.TabIndex = 3;
            this.BTN_Entregar.Tag = "Ordenes.Entregar";
            this.BTN_Entregar.Text = "Entregar";
            this.BTN_Entregar.UseVisualStyleBackColor = false;
            this.BTN_Entregar.Click += new System.EventHandler(this.BTN_Entregar_Click);
            //
            // FrmOrdenesServicio
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Ordenes);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmOrdenesServicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmOrdenesServicio.Text";
            this.Text = "Administracion de ordenes";
            this.Load += new System.EventHandler(this.FrmOrdenesServicio_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Ordenes)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Cliente;
        private System.Windows.Forms.ComboBox CBO_Cliente;
        private System.Windows.Forms.Label LBL_Estado;
        private System.Windows.Forms.ComboBox CBO_Estado;
        private System.Windows.Forms.Label LBL_Busqueda;
        private System.Windows.Forms.TextBox TXT_Busqueda;
        private System.Windows.Forms.CheckBox CHK_Entregadas;
        private System.Windows.Forms.DataGridView DGV_Ordenes;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Detalle;
        private System.Windows.Forms.Button BTN_Cancelar;
        private System.Windows.Forms.Button BTN_Entregar;
    }
}
