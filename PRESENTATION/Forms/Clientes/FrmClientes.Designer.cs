namespace UI.Forms.Clientes
{
    partial class FrmClientes
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
            this.LBL_Nombre = new System.Windows.Forms.Label();
            this.TXT_Nombre = new System.Windows.Forms.TextBox();
            this.LBL_Apellido = new System.Windows.Forms.Label();
            this.TXT_Apellido = new System.Windows.Forms.TextBox();
            this.LBL_Documento = new System.Windows.Forms.Label();
            this.TXT_Documento = new System.Windows.Forms.TextBox();
            this.CHK_Inactivos = new System.Windows.Forms.CheckBox();
            this.DGV_Clientes = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Editar = new System.Windows.Forms.Button();
            this.BTN_Desactivar = new System.Windows.Forms.Button();
            this.BTN_Reactivar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Clientes)).BeginInit();
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
            this.LBL_Titulo.Tag = "Clientes.Titulo";
            this.LBL_Titulo.Text = "Administracion de clientes";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Nombre);
            this.PNL_Filtros.Controls.Add(this.TXT_Nombre);
            this.PNL_Filtros.Controls.Add(this.LBL_Apellido);
            this.PNL_Filtros.Controls.Add(this.TXT_Apellido);
            this.PNL_Filtros.Controls.Add(this.LBL_Documento);
            this.PNL_Filtros.Controls.Add(this.TXT_Documento);
            this.PNL_Filtros.Controls.Add(this.CHK_Inactivos);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(1020, 50);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Nombre
            //
            this.LBL_Nombre.AutoSize = true;
            this.LBL_Nombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Nombre.Location = new System.Drawing.Point(10, 16);
            this.LBL_Nombre.Name = "LBL_Nombre";
            this.LBL_Nombre.Size = new System.Drawing.Size(54, 15);
            this.LBL_Nombre.TabIndex = 0;
            this.LBL_Nombre.Tag = "Clientes.FiltroNombre";
            this.LBL_Nombre.Text = "Nombre:";
            //
            // TXT_Nombre
            //
            this.TXT_Nombre.Location = new System.Drawing.Point(70, 13);
            this.TXT_Nombre.Name = "TXT_Nombre";
            this.TXT_Nombre.Size = new System.Drawing.Size(140, 22);
            this.TXT_Nombre.TabIndex = 1;
            this.TXT_Nombre.TextChanged += new System.EventHandler(this.Filtros_TextChanged);
            //
            // LBL_Apellido
            //
            this.LBL_Apellido.AutoSize = true;
            this.LBL_Apellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Apellido.Location = new System.Drawing.Point(225, 16);
            this.LBL_Apellido.Name = "LBL_Apellido";
            this.LBL_Apellido.Size = new System.Drawing.Size(54, 15);
            this.LBL_Apellido.TabIndex = 2;
            this.LBL_Apellido.Tag = "Clientes.FiltroApellido";
            this.LBL_Apellido.Text = "Apellido:";
            //
            // TXT_Apellido
            //
            this.TXT_Apellido.Location = new System.Drawing.Point(285, 13);
            this.TXT_Apellido.Name = "TXT_Apellido";
            this.TXT_Apellido.Size = new System.Drawing.Size(140, 22);
            this.TXT_Apellido.TabIndex = 3;
            this.TXT_Apellido.TextChanged += new System.EventHandler(this.Filtros_TextChanged);
            //
            // LBL_Documento
            //
            this.LBL_Documento.AutoSize = true;
            this.LBL_Documento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Documento.Location = new System.Drawing.Point(440, 16);
            this.LBL_Documento.Name = "LBL_Documento";
            this.LBL_Documento.Size = new System.Drawing.Size(74, 15);
            this.LBL_Documento.TabIndex = 4;
            this.LBL_Documento.Tag = "Clientes.FiltroDocumento";
            this.LBL_Documento.Text = "Documento:";
            //
            // TXT_Documento
            //
            this.TXT_Documento.Location = new System.Drawing.Point(520, 13);
            this.TXT_Documento.Name = "TXT_Documento";
            this.TXT_Documento.Size = new System.Drawing.Size(140, 22);
            this.TXT_Documento.TabIndex = 5;
            this.TXT_Documento.TextChanged += new System.EventHandler(this.Filtros_TextChanged);
            //
            // CHK_Inactivos
            //
            this.CHK_Inactivos.AutoSize = true;
            this.CHK_Inactivos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_Inactivos.Location = new System.Drawing.Point(690, 15);
            this.CHK_Inactivos.Name = "CHK_Inactivos";
            this.CHK_Inactivos.Size = new System.Drawing.Size(95, 19);
            this.CHK_Inactivos.TabIndex = 6;
            this.CHK_Inactivos.Tag = "Clientes.VerInactivos";
            this.CHK_Inactivos.Text = "Ver inactivos";
            this.CHK_Inactivos.UseVisualStyleBackColor = true;
            this.CHK_Inactivos.CheckedChanged += new System.EventHandler(this.CHK_Inactivos_CheckedChanged);
            //
            // DGV_Clientes
            //
            this.DGV_Clientes.AllowUserToAddRows = false;
            this.DGV_Clientes.AllowUserToDeleteRows = false;
            this.DGV_Clientes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Clientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Clientes.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Clientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Clientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Clientes.Location = new System.Drawing.Point(15, 140);
            this.DGV_Clientes.MultiSelect = false;
            this.DGV_Clientes.Name = "DGV_Clientes";
            this.DGV_Clientes.ReadOnly = true;
            this.DGV_Clientes.RowHeadersVisible = false;
            this.DGV_Clientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Clientes.Size = new System.Drawing.Size(1020, 330);
            this.DGV_Clientes.TabIndex = 2;
            this.DGV_Clientes.SelectionChanged += new System.EventHandler(this.DGV_Clientes_SelectionChanged);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_Crear);
            this.PNL_Botones.Controls.Add(this.BTN_Editar);
            this.PNL_Botones.Controls.Add(this.BTN_Desactivar);
            this.PNL_Botones.Controls.Add(this.BTN_Reactivar);
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
            this.BTN_Crear.Tag = "Clientes.Crear";
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
            this.BTN_Editar.Location = new System.Drawing.Point(135, 10);
            this.BTN_Editar.Name = "BTN_Editar";
            this.BTN_Editar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Editar.TabIndex = 1;
            this.BTN_Editar.Tag = "Clientes.Editar";
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
            this.BTN_Desactivar.Location = new System.Drawing.Point(265, 10);
            this.BTN_Desactivar.Name = "BTN_Desactivar";
            this.BTN_Desactivar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Desactivar.TabIndex = 2;
            this.BTN_Desactivar.Tag = "Clientes.Desactivar";
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
            this.BTN_Reactivar.Location = new System.Drawing.Point(395, 10);
            this.BTN_Reactivar.Name = "BTN_Reactivar";
            this.BTN_Reactivar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Reactivar.TabIndex = 3;
            this.BTN_Reactivar.Tag = "Clientes.Reactivar";
            this.BTN_Reactivar.Text = "Reactivar";
            this.BTN_Reactivar.UseVisualStyleBackColor = false;
            this.BTN_Reactivar.Click += new System.EventHandler(this.BTN_Reactivar_Click);
            //
            // FrmClientes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Clientes);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmClientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmClientes.Text";
            this.Text = "Administracion de clientes";
            this.Load += new System.EventHandler(this.FrmClientes_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Clientes)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Nombre;
        private System.Windows.Forms.TextBox TXT_Nombre;
        private System.Windows.Forms.Label LBL_Apellido;
        private System.Windows.Forms.TextBox TXT_Apellido;
        private System.Windows.Forms.Label LBL_Documento;
        private System.Windows.Forms.TextBox TXT_Documento;
        private System.Windows.Forms.CheckBox CHK_Inactivos;
        private System.Windows.Forms.DataGridView DGV_Clientes;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Editar;
        private System.Windows.Forms.Button BTN_Desactivar;
        private System.Windows.Forms.Button BTN_Reactivar;
    }
}
