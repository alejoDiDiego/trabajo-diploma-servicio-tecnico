namespace UI.Forms.Repuestos
{
    partial class FrmRepuestos
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
            this.LBL_Codigo = new System.Windows.Forms.Label();
            this.TXT_Codigo = new System.Windows.Forms.TextBox();
            this.LBL_Descripcion = new System.Windows.Forms.Label();
            this.TXT_Descripcion = new System.Windows.Forms.TextBox();
            this.CHK_Inactivos = new System.Windows.Forms.CheckBox();
            this.CHK_BajoMinimo = new System.Windows.Forms.CheckBox();
            this.DGV_Repuestos = new System.Windows.Forms.DataGridView();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.BTN_Editar = new System.Windows.Forms.Button();
            this.BTN_Desactivar = new System.Windows.Forms.Button();
            this.BTN_Reactivar = new System.Windows.Forms.Button();
            this.BTN_Ajustar = new System.Windows.Forms.Button();
            this.BTN_Movimientos = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Filtros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Repuestos)).BeginInit();
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
            this.LBL_Titulo.Tag = "Repuestos.Titulo";
            this.LBL_Titulo.Text = "Repuestos";
            //
            // PNL_Filtros
            //
            this.PNL_Filtros.BackColor = System.Drawing.Color.White;
            this.PNL_Filtros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Filtros.Controls.Add(this.LBL_Codigo);
            this.PNL_Filtros.Controls.Add(this.TXT_Codigo);
            this.PNL_Filtros.Controls.Add(this.LBL_Descripcion);
            this.PNL_Filtros.Controls.Add(this.TXT_Descripcion);
            this.PNL_Filtros.Controls.Add(this.CHK_Inactivos);
            this.PNL_Filtros.Controls.Add(this.CHK_BajoMinimo);
            this.PNL_Filtros.Location = new System.Drawing.Point(15, 75);
            this.PNL_Filtros.Name = "PNL_Filtros";
            this.PNL_Filtros.Size = new System.Drawing.Size(890, 78);
            this.PNL_Filtros.TabIndex = 1;
            //
            // LBL_Codigo
            //
            this.LBL_Codigo.AutoSize = true;
            this.LBL_Codigo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Codigo.Location = new System.Drawing.Point(10, 14);
            this.LBL_Codigo.Name = "LBL_Codigo";
            this.LBL_Codigo.Size = new System.Drawing.Size(49, 15);
            this.LBL_Codigo.TabIndex = 0;
            this.LBL_Codigo.Tag = "Repuestos.FiltroCodigo";
            this.LBL_Codigo.Text = "Codigo:";
            //
            // TXT_Codigo
            //
            this.TXT_Codigo.Location = new System.Drawing.Point(95, 11);
            this.TXT_Codigo.Name = "TXT_Codigo";
            this.TXT_Codigo.Size = new System.Drawing.Size(150, 22);
            this.TXT_Codigo.TabIndex = 1;
            this.TXT_Codigo.TextChanged += new System.EventHandler(this.TXT_Filtro_TextChanged);
            //
            // LBL_Descripcion
            //
            this.LBL_Descripcion.AutoSize = true;
            this.LBL_Descripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Descripcion.Location = new System.Drawing.Point(260, 14);
            this.LBL_Descripcion.Name = "LBL_Descripcion";
            this.LBL_Descripcion.Size = new System.Drawing.Size(72, 15);
            this.LBL_Descripcion.TabIndex = 2;
            this.LBL_Descripcion.Tag = "Repuestos.FiltroDescripcion";
            this.LBL_Descripcion.Text = "Descripcion:";
            //
            // TXT_Descripcion
            //
            this.TXT_Descripcion.Location = new System.Drawing.Point(338, 11);
            this.TXT_Descripcion.Name = "TXT_Descripcion";
            this.TXT_Descripcion.Size = new System.Drawing.Size(200, 22);
            this.TXT_Descripcion.TabIndex = 3;
            this.TXT_Descripcion.TextChanged += new System.EventHandler(this.TXT_Filtro_TextChanged);
            //
            // CHK_Inactivos
            //
            this.CHK_Inactivos.AutoSize = true;
            this.CHK_Inactivos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_Inactivos.Location = new System.Drawing.Point(13, 46);
            this.CHK_Inactivos.Name = "CHK_Inactivos";
            this.CHK_Inactivos.Size = new System.Drawing.Size(95, 19);
            this.CHK_Inactivos.TabIndex = 4;
            this.CHK_Inactivos.Tag = "Repuestos.VerInactivos";
            this.CHK_Inactivos.Text = "Ver inactivos";
            this.CHK_Inactivos.UseVisualStyleBackColor = true;
            this.CHK_Inactivos.CheckedChanged += new System.EventHandler(this.CHK_Inactivos_CheckedChanged);
            //
            // CHK_BajoMinimo
            //
            this.CHK_BajoMinimo.AutoSize = true;
            this.CHK_BajoMinimo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_BajoMinimo.Location = new System.Drawing.Point(180, 46);
            this.CHK_BajoMinimo.Name = "CHK_BajoMinimo";
            this.CHK_BajoMinimo.Size = new System.Drawing.Size(120, 19);
            this.CHK_BajoMinimo.TabIndex = 5;
            this.CHK_BajoMinimo.Tag = "Repuestos.SoloBajoMinimo";
            this.CHK_BajoMinimo.Text = "Solo bajo minimo";
            this.CHK_BajoMinimo.UseVisualStyleBackColor = true;
            this.CHK_BajoMinimo.CheckedChanged += new System.EventHandler(this.CHK_BajoMinimo_CheckedChanged);
            //
            // DGV_Repuestos
            //
            this.DGV_Repuestos.AllowUserToAddRows = false;
            this.DGV_Repuestos.AllowUserToDeleteRows = false;
            this.DGV_Repuestos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Repuestos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Repuestos.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Repuestos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Repuestos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Repuestos.Location = new System.Drawing.Point(15, 165);
            this.DGV_Repuestos.MultiSelect = false;
            this.DGV_Repuestos.Name = "DGV_Repuestos";
            this.DGV_Repuestos.ReadOnly = true;
            this.DGV_Repuestos.RowHeadersVisible = false;
            this.DGV_Repuestos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Repuestos.Size = new System.Drawing.Size(890, 280);
            this.DGV_Repuestos.TabIndex = 2;
            this.DGV_Repuestos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_Repuestos_CellFormatting);
            this.DGV_Repuestos.SelectionChanged += new System.EventHandler(this.DGV_Repuestos_SelectionChanged);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_Crear);
            this.PNL_Botones.Controls.Add(this.BTN_Editar);
            this.PNL_Botones.Controls.Add(this.BTN_Desactivar);
            this.PNL_Botones.Controls.Add(this.BTN_Reactivar);
            this.PNL_Botones.Controls.Add(this.BTN_Ajustar);
            this.PNL_Botones.Controls.Add(this.BTN_Movimientos);
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
            this.BTN_Crear.Tag = "Repuestos.Crear";
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
            this.BTN_Editar.Tag = "Repuestos.Editar";
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
            this.BTN_Desactivar.Tag = "Repuestos.Desactivar";
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
            this.BTN_Reactivar.Tag = "Repuestos.Reactivar";
            this.BTN_Reactivar.Text = "Reactivar";
            this.BTN_Reactivar.UseVisualStyleBackColor = false;
            this.BTN_Reactivar.Click += new System.EventHandler(this.BTN_Reactivar_Click);
            //
            // BTN_Ajustar
            //
            this.BTN_Ajustar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.BTN_Ajustar.FlatAppearance.BorderSize = 0;
            this.BTN_Ajustar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Ajustar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Ajustar.ForeColor = System.Drawing.Color.White;
            this.BTN_Ajustar.Location = new System.Drawing.Point(485, 10);
            this.BTN_Ajustar.Name = "BTN_Ajustar";
            this.BTN_Ajustar.Size = new System.Drawing.Size(130, 30);
            this.BTN_Ajustar.TabIndex = 4;
            this.BTN_Ajustar.Tag = "Repuestos.Ajustar";
            this.BTN_Ajustar.Text = "Ajustar stock";
            this.BTN_Ajustar.UseVisualStyleBackColor = false;
            this.BTN_Ajustar.Click += new System.EventHandler(this.BTN_Ajustar_Click);
            //
            // BTN_Movimientos
            //
            this.BTN_Movimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.BTN_Movimientos.FlatAppearance.BorderSize = 0;
            this.BTN_Movimientos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Movimientos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Movimientos.ForeColor = System.Drawing.Color.White;
            this.BTN_Movimientos.Location = new System.Drawing.Point(625, 10);
            this.BTN_Movimientos.Name = "BTN_Movimientos";
            this.BTN_Movimientos.Size = new System.Drawing.Size(140, 30);
            this.BTN_Movimientos.TabIndex = 5;
            this.BTN_Movimientos.Tag = "Repuestos.Movimientos";
            this.BTN_Movimientos.Text = "Ver movimientos";
            this.BTN_Movimientos.UseVisualStyleBackColor = false;
            this.BTN_Movimientos.Click += new System.EventHandler(this.BTN_Movimientos_Click);
            //
            // FrmRepuestos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(920, 520);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.DGV_Repuestos);
            this.Controls.Add(this.PNL_Filtros);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmRepuestos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmRepuestos.Text";
            this.Text = "Repuestos";
            this.Load += new System.EventHandler(this.FrmRepuestos_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Filtros.ResumeLayout(false);
            this.PNL_Filtros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Repuestos)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_Filtros;
        private System.Windows.Forms.Label LBL_Codigo;
        private System.Windows.Forms.TextBox TXT_Codigo;
        private System.Windows.Forms.Label LBL_Descripcion;
        private System.Windows.Forms.TextBox TXT_Descripcion;
        private System.Windows.Forms.CheckBox CHK_Inactivos;
        private System.Windows.Forms.CheckBox CHK_BajoMinimo;
        private System.Windows.Forms.DataGridView DGV_Repuestos;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Editar;
        private System.Windows.Forms.Button BTN_Desactivar;
        private System.Windows.Forms.Button BTN_Reactivar;
        private System.Windows.Forms.Button BTN_Ajustar;
        private System.Windows.Forms.Button BTN_Movimientos;
    }
}
