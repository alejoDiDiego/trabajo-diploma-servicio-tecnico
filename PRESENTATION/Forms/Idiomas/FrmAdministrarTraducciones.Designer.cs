namespace UI.Forms.Idiomas
{
    partial class FrmAdministrarTraducciones
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
            this.GBX_Traducciones = new System.Windows.Forms.GroupBox();
            this.BTN_LimpiarTraduccion = new System.Windows.Forms.Button();
            this.BTN_EliminarTraduccion = new System.Windows.Forms.Button();
            this.BTN_EditarTraduccion = new System.Windows.Forms.Button();
            this.BTN_CrearTraduccion = new System.Windows.Forms.Button();
            this.TBX_Texto = new System.Windows.Forms.TextBox();
            this.LBL_Texto = new System.Windows.Forms.Label();
            this.CBX_Idiomas = new System.Windows.Forms.ComboBox();
            this.LBL_IdiomaTraduccion = new System.Windows.Forms.Label();
            this.TBX_Clave = new System.Windows.Forms.TextBox();
            this.LBL_Clave = new System.Windows.Forms.Label();
            this.DGV_Traducciones = new System.Windows.Forms.DataGridView();
            this.GBX_Idiomas = new System.Windows.Forms.GroupBox();
            this.BTN_LimpiarIdioma = new System.Windows.Forms.Button();
            this.BTN_EliminarIdioma = new System.Windows.Forms.Button();
            this.BTN_EditarIdioma = new System.Windows.Forms.Button();
            this.BTN_CrearIdioma = new System.Windows.Forms.Button();
            this.TBX_NombreIdioma = new System.Windows.Forms.TextBox();
            this.LBL_NombreIdioma = new System.Windows.Forms.Label();
            this.DGV_Idiomas = new System.Windows.Forms.DataGridView();
            this.PNL_Header.SuspendLayout();
            this.GBX_Traducciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Traducciones)).BeginInit();
            this.GBX_Idiomas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Idiomas)).BeginInit();
            this.SuspendLayout();
            // 
            // PNL_Header
            // 
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(1000, 65);
            this.PNL_Header.TabIndex = 0;
            // 
            // LBL_Titulo
            // 
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 18);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(284, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Traducciones.Titulo";
            this.LBL_Titulo.Text = "Administracion de traducciones";
            // 
            // GBX_Traducciones
            // 
            this.GBX_Traducciones.Controls.Add(this.BTN_LimpiarTraduccion);
            this.GBX_Traducciones.Controls.Add(this.BTN_EliminarTraduccion);
            this.GBX_Traducciones.Controls.Add(this.BTN_EditarTraduccion);
            this.GBX_Traducciones.Controls.Add(this.BTN_CrearTraduccion);
            this.GBX_Traducciones.Controls.Add(this.TBX_Texto);
            this.GBX_Traducciones.Controls.Add(this.LBL_Texto);
            this.GBX_Traducciones.Controls.Add(this.CBX_Idiomas);
            this.GBX_Traducciones.Controls.Add(this.LBL_IdiomaTraduccion);
            this.GBX_Traducciones.Controls.Add(this.TBX_Clave);
            this.GBX_Traducciones.Controls.Add(this.LBL_Clave);
            this.GBX_Traducciones.Controls.Add(this.DGV_Traducciones);
            this.GBX_Traducciones.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GBX_Traducciones.Location = new System.Drawing.Point(12, 78);
            this.GBX_Traducciones.Name = "GBX_Traducciones";
            this.GBX_Traducciones.Size = new System.Drawing.Size(650, 560);
            this.GBX_Traducciones.TabIndex = 1;
            this.GBX_Traducciones.TabStop = false;
            this.GBX_Traducciones.Tag = "Traducciones.GestionTraducciones";
            this.GBX_Traducciones.Text = "Traducciones";
            // 
            // BTN_LimpiarTraduccion
            // 
            this.BTN_LimpiarTraduccion.Location = new System.Drawing.Point(498, 512);
            this.BTN_LimpiarTraduccion.Name = "BTN_LimpiarTraduccion";
            this.BTN_LimpiarTraduccion.Size = new System.Drawing.Size(130, 30);
            this.BTN_LimpiarTraduccion.TabIndex = 10;
            this.BTN_LimpiarTraduccion.Tag = "Accion.Limpiar";
            this.BTN_LimpiarTraduccion.Text = "Limpiar";
            this.BTN_LimpiarTraduccion.UseVisualStyleBackColor = true;
            this.BTN_LimpiarTraduccion.Click += new System.EventHandler(this.BTN_LimpiarTraduccion_Click);
            // 
            // BTN_EliminarTraduccion
            // 
            this.BTN_EliminarTraduccion.BackColor = System.Drawing.Color.Maroon;
            this.BTN_EliminarTraduccion.FlatAppearance.BorderSize = 0;
            this.BTN_EliminarTraduccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EliminarTraduccion.ForeColor = System.Drawing.Color.White;
            this.BTN_EliminarTraduccion.Location = new System.Drawing.Point(342, 512);
            this.BTN_EliminarTraduccion.Name = "BTN_EliminarTraduccion";
            this.BTN_EliminarTraduccion.Size = new System.Drawing.Size(150, 30);
            this.BTN_EliminarTraduccion.TabIndex = 9;
            this.BTN_EliminarTraduccion.Tag = "Traducciones.Eliminar";
            this.BTN_EliminarTraduccion.Text = "Eliminar traduccion";
            this.BTN_EliminarTraduccion.UseVisualStyleBackColor = false;
            this.BTN_EliminarTraduccion.Click += new System.EventHandler(this.BTN_EliminarTraduccion_Click);
            // 
            // BTN_EditarTraduccion
            // 
            this.BTN_EditarTraduccion.BackColor = System.Drawing.Color.FromArgb(0, 0, 192);
            this.BTN_EditarTraduccion.FlatAppearance.BorderSize = 0;
            this.BTN_EditarTraduccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EditarTraduccion.ForeColor = System.Drawing.Color.White;
            this.BTN_EditarTraduccion.Location = new System.Drawing.Point(186, 512);
            this.BTN_EditarTraduccion.Name = "BTN_EditarTraduccion";
            this.BTN_EditarTraduccion.Size = new System.Drawing.Size(150, 30);
            this.BTN_EditarTraduccion.TabIndex = 8;
            this.BTN_EditarTraduccion.Tag = "Traducciones.Editar";
            this.BTN_EditarTraduccion.Text = "Editar traduccion";
            this.BTN_EditarTraduccion.UseVisualStyleBackColor = false;
            this.BTN_EditarTraduccion.Click += new System.EventHandler(this.BTN_EditarTraduccion_Click);
            // 
            // BTN_CrearTraduccion
            // 
            this.BTN_CrearTraduccion.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.BTN_CrearTraduccion.FlatAppearance.BorderSize = 0;
            this.BTN_CrearTraduccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CrearTraduccion.ForeColor = System.Drawing.Color.White;
            this.BTN_CrearTraduccion.Location = new System.Drawing.Point(30, 512);
            this.BTN_CrearTraduccion.Name = "BTN_CrearTraduccion";
            this.BTN_CrearTraduccion.Size = new System.Drawing.Size(150, 30);
            this.BTN_CrearTraduccion.TabIndex = 7;
            this.BTN_CrearTraduccion.Tag = "Traducciones.Crear";
            this.BTN_CrearTraduccion.Text = "Crear traduccion";
            this.BTN_CrearTraduccion.UseVisualStyleBackColor = false;
            this.BTN_CrearTraduccion.Click += new System.EventHandler(this.BTN_CrearTraduccion_Click);
            // 
            // TBX_Texto
            // 
            this.TBX_Texto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TBX_Texto.Location = new System.Drawing.Point(30, 428);
            this.TBX_Texto.Multiline = true;
            this.TBX_Texto.Name = "TBX_Texto";
            this.TBX_Texto.Size = new System.Drawing.Size(598, 68);
            this.TBX_Texto.TabIndex = 6;
            // 
            // LBL_Texto
            // 
            this.LBL_Texto.AutoSize = true;
            this.LBL_Texto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Texto.Location = new System.Drawing.Point(27, 410);
            this.LBL_Texto.Name = "LBL_Texto";
            this.LBL_Texto.Size = new System.Drawing.Size(36, 15);
            this.LBL_Texto.TabIndex = 5;
            this.LBL_Texto.Tag = "Traducciones.Texto";
            this.LBL_Texto.Text = "Texto";
            // 
            // CBX_Idiomas
            // 
            this.CBX_Idiomas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBX_Idiomas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CBX_Idiomas.FormattingEnabled = true;
            this.CBX_Idiomas.Location = new System.Drawing.Point(426, 374);
            this.CBX_Idiomas.Name = "CBX_Idiomas";
            this.CBX_Idiomas.Size = new System.Drawing.Size(202, 23);
            this.CBX_Idiomas.TabIndex = 4;
            // 
            // LBL_IdiomaTraduccion
            // 
            this.LBL_IdiomaTraduccion.AutoSize = true;
            this.LBL_IdiomaTraduccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_IdiomaTraduccion.Location = new System.Drawing.Point(423, 356);
            this.LBL_IdiomaTraduccion.Name = "LBL_IdiomaTraduccion";
            this.LBL_IdiomaTraduccion.Size = new System.Drawing.Size(43, 15);
            this.LBL_IdiomaTraduccion.TabIndex = 3;
            this.LBL_IdiomaTraduccion.Tag = "Traducciones.Idioma";
            this.LBL_IdiomaTraduccion.Text = "Idioma";
            // 
            // TBX_Clave
            // 
            this.TBX_Clave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TBX_Clave.Location = new System.Drawing.Point(30, 374);
            this.TBX_Clave.Name = "TBX_Clave";
            this.TBX_Clave.Size = new System.Drawing.Size(370, 23);
            this.TBX_Clave.TabIndex = 2;
            // 
            // LBL_Clave
            // 
            this.LBL_Clave.AutoSize = true;
            this.LBL_Clave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Clave.Location = new System.Drawing.Point(27, 356);
            this.LBL_Clave.Name = "LBL_Clave";
            this.LBL_Clave.Size = new System.Drawing.Size(35, 15);
            this.LBL_Clave.TabIndex = 1;
            this.LBL_Clave.Tag = "Traducciones.Clave";
            this.LBL_Clave.Text = "Clave";
            // 
            // DGV_Traducciones
            // 
            this.DGV_Traducciones.AllowUserToAddRows = false;
            this.DGV_Traducciones.AllowUserToDeleteRows = false;
            this.DGV_Traducciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Traducciones.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Traducciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Traducciones.Location = new System.Drawing.Point(18, 30);
            this.DGV_Traducciones.Name = "DGV_Traducciones";
            this.DGV_Traducciones.ReadOnly = true;
            this.DGV_Traducciones.RowHeadersVisible = false;
            this.DGV_Traducciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Traducciones.Size = new System.Drawing.Size(610, 305);
            this.DGV_Traducciones.TabIndex = 0;
            this.DGV_Traducciones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Traducciones_CellClick);
            // 
            // GBX_Idiomas
            // 
            this.GBX_Idiomas.Controls.Add(this.BTN_LimpiarIdioma);
            this.GBX_Idiomas.Controls.Add(this.BTN_EliminarIdioma);
            this.GBX_Idiomas.Controls.Add(this.BTN_EditarIdioma);
            this.GBX_Idiomas.Controls.Add(this.BTN_CrearIdioma);
            this.GBX_Idiomas.Controls.Add(this.TBX_NombreIdioma);
            this.GBX_Idiomas.Controls.Add(this.LBL_NombreIdioma);
            this.GBX_Idiomas.Controls.Add(this.DGV_Idiomas);
            this.GBX_Idiomas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GBX_Idiomas.Location = new System.Drawing.Point(678, 78);
            this.GBX_Idiomas.Name = "GBX_Idiomas";
            this.GBX_Idiomas.Size = new System.Drawing.Size(310, 560);
            this.GBX_Idiomas.TabIndex = 2;
            this.GBX_Idiomas.TabStop = false;
            this.GBX_Idiomas.Tag = "Traducciones.GestionIdiomas";
            this.GBX_Idiomas.Text = "Idiomas";
            // 
            // BTN_LimpiarIdioma
            // 
            this.BTN_LimpiarIdioma.Location = new System.Drawing.Point(162, 502);
            this.BTN_LimpiarIdioma.Name = "BTN_LimpiarIdioma";
            this.BTN_LimpiarIdioma.Size = new System.Drawing.Size(130, 30);
            this.BTN_LimpiarIdioma.TabIndex = 6;
            this.BTN_LimpiarIdioma.Tag = "Accion.Limpiar";
            this.BTN_LimpiarIdioma.Text = "Limpiar";
            this.BTN_LimpiarIdioma.UseVisualStyleBackColor = true;
            this.BTN_LimpiarIdioma.Click += new System.EventHandler(this.BTN_LimpiarIdioma_Click);
            // 
            // BTN_EliminarIdioma
            // 
            this.BTN_EliminarIdioma.BackColor = System.Drawing.Color.Maroon;
            this.BTN_EliminarIdioma.FlatAppearance.BorderSize = 0;
            this.BTN_EliminarIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EliminarIdioma.ForeColor = System.Drawing.Color.White;
            this.BTN_EliminarIdioma.Location = new System.Drawing.Point(16, 502);
            this.BTN_EliminarIdioma.Name = "BTN_EliminarIdioma";
            this.BTN_EliminarIdioma.Size = new System.Drawing.Size(130, 30);
            this.BTN_EliminarIdioma.TabIndex = 5;
            this.BTN_EliminarIdioma.Tag = "Idiomas.Eliminar";
            this.BTN_EliminarIdioma.Text = "Eliminar idioma";
            this.BTN_EliminarIdioma.UseVisualStyleBackColor = false;
            this.BTN_EliminarIdioma.Click += new System.EventHandler(this.BTN_EliminarIdioma_Click);
            // 
            // BTN_EditarIdioma
            // 
            this.BTN_EditarIdioma.BackColor = System.Drawing.Color.FromArgb(0, 0, 192);
            this.BTN_EditarIdioma.FlatAppearance.BorderSize = 0;
            this.BTN_EditarIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EditarIdioma.ForeColor = System.Drawing.Color.White;
            this.BTN_EditarIdioma.Location = new System.Drawing.Point(162, 458);
            this.BTN_EditarIdioma.Name = "BTN_EditarIdioma";
            this.BTN_EditarIdioma.Size = new System.Drawing.Size(130, 30);
            this.BTN_EditarIdioma.TabIndex = 4;
            this.BTN_EditarIdioma.Tag = "Idiomas.Editar";
            this.BTN_EditarIdioma.Text = "Editar idioma";
            this.BTN_EditarIdioma.UseVisualStyleBackColor = false;
            this.BTN_EditarIdioma.Click += new System.EventHandler(this.BTN_EditarIdioma_Click);
            // 
            // BTN_CrearIdioma
            // 
            this.BTN_CrearIdioma.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.BTN_CrearIdioma.FlatAppearance.BorderSize = 0;
            this.BTN_CrearIdioma.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CrearIdioma.ForeColor = System.Drawing.Color.White;
            this.BTN_CrearIdioma.Location = new System.Drawing.Point(16, 458);
            this.BTN_CrearIdioma.Name = "BTN_CrearIdioma";
            this.BTN_CrearIdioma.Size = new System.Drawing.Size(130, 30);
            this.BTN_CrearIdioma.TabIndex = 3;
            this.BTN_CrearIdioma.Tag = "Idiomas.Crear";
            this.BTN_CrearIdioma.Text = "Crear idioma";
            this.BTN_CrearIdioma.UseVisualStyleBackColor = false;
            this.BTN_CrearIdioma.Click += new System.EventHandler(this.BTN_CrearIdioma_Click);
            // 
            // TBX_NombreIdioma
            // 
            this.TBX_NombreIdioma.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TBX_NombreIdioma.Location = new System.Drawing.Point(16, 414);
            this.TBX_NombreIdioma.Name = "TBX_NombreIdioma";
            this.TBX_NombreIdioma.Size = new System.Drawing.Size(276, 23);
            this.TBX_NombreIdioma.TabIndex = 2;
            // 
            // LBL_NombreIdioma
            // 
            this.LBL_NombreIdioma.AutoSize = true;
            this.LBL_NombreIdioma.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_NombreIdioma.Location = new System.Drawing.Point(13, 396);
            this.LBL_NombreIdioma.Name = "LBL_NombreIdioma";
            this.LBL_NombreIdioma.Size = new System.Drawing.Size(51, 15);
            this.LBL_NombreIdioma.TabIndex = 1;
            this.LBL_NombreIdioma.Tag = "Idiomas.Nombre";
            this.LBL_NombreIdioma.Text = "Nombre";
            // 
            // DGV_Idiomas
            // 
            this.DGV_Idiomas.AllowUserToAddRows = false;
            this.DGV_Idiomas.AllowUserToDeleteRows = false;
            this.DGV_Idiomas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Idiomas.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Idiomas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Idiomas.Location = new System.Drawing.Point(16, 30);
            this.DGV_Idiomas.Name = "DGV_Idiomas";
            this.DGV_Idiomas.ReadOnly = true;
            this.DGV_Idiomas.RowHeadersVisible = false;
            this.DGV_Idiomas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Idiomas.Size = new System.Drawing.Size(276, 345);
            this.DGV_Idiomas.TabIndex = 0;
            this.DGV_Idiomas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Idiomas_CellClick);
            // 
            // FrmAdministrarTraducciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.GBX_Idiomas);
            this.Controls.Add(this.GBX_Traducciones);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdministrarTraducciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmAdministrarTraducciones.Text";
            this.Text = "Administracion de traducciones";
            this.Load += new System.EventHandler(this.FrmAdministrarTraducciones_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.GBX_Traducciones.ResumeLayout(false);
            this.GBX_Traducciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Traducciones)).EndInit();
            this.GBX_Idiomas.ResumeLayout(false);
            this.GBX_Idiomas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Idiomas)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.GroupBox GBX_Traducciones;
        private System.Windows.Forms.DataGridView DGV_Traducciones;
        private System.Windows.Forms.Label LBL_Clave;
        private System.Windows.Forms.TextBox TBX_Clave;
        private System.Windows.Forms.ComboBox CBX_Idiomas;
        private System.Windows.Forms.Label LBL_IdiomaTraduccion;
        private System.Windows.Forms.TextBox TBX_Texto;
        private System.Windows.Forms.Label LBL_Texto;
        private System.Windows.Forms.Button BTN_CrearTraduccion;
        private System.Windows.Forms.Button BTN_EditarTraduccion;
        private System.Windows.Forms.Button BTN_EliminarTraduccion;
        private System.Windows.Forms.Button BTN_LimpiarTraduccion;
        private System.Windows.Forms.GroupBox GBX_Idiomas;
        private System.Windows.Forms.DataGridView DGV_Idiomas;
        private System.Windows.Forms.Label LBL_NombreIdioma;
        private System.Windows.Forms.TextBox TBX_NombreIdioma;
        private System.Windows.Forms.Button BTN_CrearIdioma;
        private System.Windows.Forms.Button BTN_EditarIdioma;
        private System.Windows.Forms.Button BTN_EliminarIdioma;
        private System.Windows.Forms.Button BTN_LimpiarIdioma;
    }
}
