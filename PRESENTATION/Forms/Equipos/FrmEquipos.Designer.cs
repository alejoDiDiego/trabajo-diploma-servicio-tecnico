namespace UI.Forms.Equipos
{
    partial class FrmEquipos
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
            this.LBL_Busqueda = new System.Windows.Forms.Label();
            this.TXT_Busqueda = new System.Windows.Forms.TextBox();
            this.CHK_Inactivos = new System.Windows.Forms.CheckBox();
            this.DGV_Equipos = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Editar = new System.Windows.Forms.Button();
            this.BTN_Desactivar = new System.Windows.Forms.Button();
            this.BTN_Reactivar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Equipos)).BeginInit();
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
            this.LBL_Titulo.Tag = "Equipos.Titulo";
            this.LBL_Titulo.Text = "Administracion de equipos";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Cliente);
            this.PNL_Filtros.Controls.Add(this.CBO_Cliente);
            this.PNL_Filtros.Controls.Add(this.LBL_Busqueda);
            this.PNL_Filtros.Controls.Add(this.TXT_Busqueda);
            this.PNL_Filtros.Controls.Add(this.CHK_Inactivos);
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
            this.LBL_Cliente.Tag = "Equipos.FiltroCliente";
            this.LBL_Cliente.Text = "Cliente:";
            //
            // CBO_Cliente
            //
            this.CBO_Cliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Cliente.Location = new System.Drawing.Point(63, 13);
            this.CBO_Cliente.Name = "CBO_Cliente";
            this.CBO_Cliente.Size = new System.Drawing.Size(210, 21);
            this.CBO_Cliente.TabIndex = 1;
            this.CBO_Cliente.SelectedIndexChanged += new System.EventHandler(this.CBO_Cliente_SelectedIndexChanged);
            //
            // LBL_Busqueda
            //
            this.LBL_Busqueda.AutoSize = true;
            this.LBL_Busqueda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Busqueda.Location = new System.Drawing.Point(290, 16);
            this.LBL_Busqueda.Name = "LBL_Busqueda";
            this.LBL_Busqueda.Size = new System.Drawing.Size(88, 15);
            this.LBL_Busqueda.TabIndex = 2;
            this.LBL_Busqueda.Tag = "Equipos.FiltroTexto";
            this.LBL_Busqueda.Text = "Modelo / Serie:";
            //
            // TXT_Busqueda
            //
            this.TXT_Busqueda.Location = new System.Drawing.Point(384, 13);
            this.TXT_Busqueda.Name = "TXT_Busqueda";
            this.TXT_Busqueda.Size = new System.Drawing.Size(180, 22);
            this.TXT_Busqueda.TabIndex = 3;
            this.TXT_Busqueda.TextChanged += new System.EventHandler(this.TXT_Busqueda_TextChanged);
            //
            // CHK_Inactivos
            //
            this.CHK_Inactivos.AutoSize = true;
            this.CHK_Inactivos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_Inactivos.Location = new System.Drawing.Point(590, 15);
            this.CHK_Inactivos.Name = "CHK_Inactivos";
            this.CHK_Inactivos.Size = new System.Drawing.Size(95, 19);
            this.CHK_Inactivos.TabIndex = 4;
            this.CHK_Inactivos.Tag = "Equipos.VerInactivos";
            this.CHK_Inactivos.Text = "Ver inactivos";
            this.CHK_Inactivos.UseVisualStyleBackColor = true;
            this.CHK_Inactivos.CheckedChanged += new System.EventHandler(this.CHK_Inactivos_CheckedChanged);
            //
            // DGV_Equipos
            //
            this.DGV_Equipos.AllowUserToAddRows = false;
            this.DGV_Equipos.AllowUserToDeleteRows = false;
            this.DGV_Equipos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Equipos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Equipos.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Equipos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Equipos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Equipos.Location = new System.Drawing.Point(15, 140);
            this.DGV_Equipos.MultiSelect = false;
            this.DGV_Equipos.Name = "DGV_Equipos";
            this.DGV_Equipos.ReadOnly = true;
            this.DGV_Equipos.RowHeadersVisible = false;
            this.DGV_Equipos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Equipos.Size = new System.Drawing.Size(1020, 330);
            this.DGV_Equipos.TabIndex = 2;
            this.DGV_Equipos.SelectionChanged += new System.EventHandler(this.DGV_Equipos_SelectionChanged);
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
            this.BTN_Crear.Tag = "Equipos.Crear";
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
            this.BTN_Editar.Tag = "Equipos.Editar";
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
            this.BTN_Desactivar.Tag = "Equipos.Desactivar";
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
            this.BTN_Reactivar.Tag = "Equipos.Reactivar";
            this.BTN_Reactivar.Text = "Reactivar";
            this.BTN_Reactivar.UseVisualStyleBackColor = false;
            this.BTN_Reactivar.Click += new System.EventHandler(this.BTN_Reactivar_Click);
            //
            // FrmEquipos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1050, 545);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Equipos);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmEquipos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmEquipos.Text";
            this.Text = "Administracion de equipos";
            this.Load += new System.EventHandler(this.FrmEquipos_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Equipos)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Cliente;
        private System.Windows.Forms.ComboBox CBO_Cliente;
        private System.Windows.Forms.Label LBL_Busqueda;
        private System.Windows.Forms.TextBox TXT_Busqueda;
        private System.Windows.Forms.CheckBox CHK_Inactivos;
        private System.Windows.Forms.DataGridView DGV_Equipos;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Editar;
        private System.Windows.Forms.Button BTN_Desactivar;
        private System.Windows.Forms.Button BTN_Reactivar;
    }
}
