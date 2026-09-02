namespace UI.Forms.Auth
{
    partial class FrmAdministrarPermisos
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
            this.LBL_Titulo = new System.Windows.Forms.Label();
            this.GBX_Arbol = new System.Windows.Forms.GroupBox();
            this.TVW_Permisos = new System.Windows.Forms.TreeView();
            this.GBX_Familia = new System.Windows.Forms.GroupBox();
            this.BTN_EliminarFamilia = new System.Windows.Forms.Button();
            this.BTN_EditarFamilia = new System.Windows.Forms.Button();
            this.BTN_CrearFamilia = new System.Windows.Forms.Button();
            this.TBX_NombreFamilia = new System.Windows.Forms.TextBox();
            this.LBL_NombreFamilia = new System.Windows.Forms.Label();
            this.GBX_Catalogo = new System.Windows.Forms.GroupBox();
            this.BTN_AgregarPermiso = new System.Windows.Forms.Button();
            this.LBX_PermisosSimples = new System.Windows.Forms.ListBox();
            this.LBL_PermisosSimples = new System.Windows.Forms.Label();
            this.BTN_AgregarFamilia = new System.Windows.Forms.Button();
            this.LBX_Familias = new System.Windows.Forms.ListBox();
            this.LBL_Familias = new System.Windows.Forms.Label();
            this.GBX_Composicion = new System.Windows.Forms.GroupBox();
            this.BTN_Limpiar = new System.Windows.Forms.Button();
            this.BTN_QuitarSeleccionado = new System.Windows.Forms.Button();
            this.TBX_Destino = new System.Windows.Forms.TextBox();
            this.LBL_Destino = new System.Windows.Forms.Label();
            this.GBX_Arbol.SuspendLayout();
            this.GBX_Familia.SuspendLayout();
            this.GBX_Catalogo.SuspendLayout();
            this.GBX_Composicion.SuspendLayout();
            this.SuspendLayout();
            // 
            // LBL_Titulo
            // 
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.LBL_Titulo.Location = new System.Drawing.Point(20, 18);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(300, 30);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Permisos.Titulo";
            this.LBL_Titulo.Text = "Administracion de permisos";
            // 
            // GBX_Arbol
            // 
            this.GBX_Arbol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left))));
            this.GBX_Arbol.Controls.Add(this.TVW_Permisos);
            this.GBX_Arbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBX_Arbol.Location = new System.Drawing.Point(24, 72);
            this.GBX_Arbol.Name = "GBX_Arbol";
            this.GBX_Arbol.Size = new System.Drawing.Size(365, 505);
            this.GBX_Arbol.TabIndex = 1;
            this.GBX_Arbol.TabStop = false;
            this.GBX_Arbol.Tag = "Permisos.Arbol";
            this.GBX_Arbol.Text = "Arbol de permisos";
            // 
            // TVW_Permisos
            // 
            this.TVW_Permisos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TVW_Permisos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TVW_Permisos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TVW_Permisos.HideSelection = false;
            this.TVW_Permisos.Indent = 24;
            this.TVW_Permisos.ItemHeight = 23;
            this.TVW_Permisos.Location = new System.Drawing.Point(14, 26);
            this.TVW_Permisos.Name = "TVW_Permisos";
            this.TVW_Permisos.Size = new System.Drawing.Size(337, 462);
            this.TVW_Permisos.TabIndex = 0;
            this.TVW_Permisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TVW_Permisos_AfterSelect);
            // 
            // GBX_Familia
            // 
            this.GBX_Familia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBX_Familia.Controls.Add(this.BTN_EliminarFamilia);
            this.GBX_Familia.Controls.Add(this.BTN_EditarFamilia);
            this.GBX_Familia.Controls.Add(this.BTN_CrearFamilia);
            this.GBX_Familia.Controls.Add(this.TBX_NombreFamilia);
            this.GBX_Familia.Controls.Add(this.LBL_NombreFamilia);
            this.GBX_Familia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBX_Familia.Location = new System.Drawing.Point(405, 72);
            this.GBX_Familia.Name = "GBX_Familia";
            this.GBX_Familia.Size = new System.Drawing.Size(375, 150);
            this.GBX_Familia.TabIndex = 2;
            this.GBX_Familia.TabStop = false;
            this.GBX_Familia.Tag = "Permisos.Familia";
            this.GBX_Familia.Text = "Familia";
            // 
            // BTN_EliminarFamilia
            // 
            this.BTN_EliminarFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_EliminarFamilia.Enabled = false;
            this.BTN_EliminarFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_EliminarFamilia.Location = new System.Drawing.Point(253, 96);
            this.BTN_EliminarFamilia.Name = "BTN_EliminarFamilia";
            this.BTN_EliminarFamilia.Size = new System.Drawing.Size(104, 32);
            this.BTN_EliminarFamilia.TabIndex = 4;
            this.BTN_EliminarFamilia.Tag = "Permisos.EliminarFamilia";
            this.BTN_EliminarFamilia.Text = "Eliminar familia";
            this.BTN_EliminarFamilia.UseVisualStyleBackColor = true;
            this.BTN_EliminarFamilia.Click += new System.EventHandler(this.BTN_EliminarFamilia_Click);
            // 
            // BTN_EditarFamilia
            // 
            this.BTN_EditarFamilia.Enabled = false;
            this.BTN_EditarFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_EditarFamilia.Location = new System.Drawing.Point(137, 96);
            this.BTN_EditarFamilia.Name = "BTN_EditarFamilia";
            this.BTN_EditarFamilia.Size = new System.Drawing.Size(104, 32);
            this.BTN_EditarFamilia.TabIndex = 3;
            this.BTN_EditarFamilia.Tag = "Permisos.EditarFamilia";
            this.BTN_EditarFamilia.Text = "Editar familia";
            this.BTN_EditarFamilia.UseVisualStyleBackColor = true;
            this.BTN_EditarFamilia.Click += new System.EventHandler(this.BTN_EditarFamilia_Click);
            // 
            // BTN_CrearFamilia
            // 
            this.BTN_CrearFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CrearFamilia.Location = new System.Drawing.Point(21, 96);
            this.BTN_CrearFamilia.Name = "BTN_CrearFamilia";
            this.BTN_CrearFamilia.Size = new System.Drawing.Size(104, 32);
            this.BTN_CrearFamilia.TabIndex = 2;
            this.BTN_CrearFamilia.Tag = "Permisos.CrearFamilia";
            this.BTN_CrearFamilia.Text = "Crear familia";
            this.BTN_CrearFamilia.UseVisualStyleBackColor = true;
            this.BTN_CrearFamilia.Click += new System.EventHandler(this.BTN_CrearFamilia_Click);
            // 
            // TBX_NombreFamilia
            // 
            this.TBX_NombreFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBX_NombreFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBX_NombreFamilia.Location = new System.Drawing.Point(21, 57);
            this.TBX_NombreFamilia.MaxLength = 100;
            this.TBX_NombreFamilia.Name = "TBX_NombreFamilia";
            this.TBX_NombreFamilia.Size = new System.Drawing.Size(336, 23);
            this.TBX_NombreFamilia.TabIndex = 1;
            // 
            // LBL_NombreFamilia
            // 
            this.LBL_NombreFamilia.AutoSize = true;
            this.LBL_NombreFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_NombreFamilia.Location = new System.Drawing.Point(18, 31);
            this.LBL_NombreFamilia.Name = "LBL_NombreFamilia";
            this.LBL_NombreFamilia.Size = new System.Drawing.Size(88, 15);
            this.LBL_NombreFamilia.TabIndex = 0;
            this.LBL_NombreFamilia.Tag = "Permisos.NombreFamilia";
            this.LBL_NombreFamilia.Text = "Nombre familia";
            // 
            // GBX_Catalogo
            // 
            this.GBX_Catalogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBX_Catalogo.Controls.Add(this.BTN_AgregarPermiso);
            this.GBX_Catalogo.Controls.Add(this.LBX_PermisosSimples);
            this.GBX_Catalogo.Controls.Add(this.LBL_PermisosSimples);
            this.GBX_Catalogo.Controls.Add(this.BTN_AgregarFamilia);
            this.GBX_Catalogo.Controls.Add(this.LBX_Familias);
            this.GBX_Catalogo.Controls.Add(this.LBL_Familias);
            this.GBX_Catalogo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBX_Catalogo.Location = new System.Drawing.Point(405, 238);
            this.GBX_Catalogo.Name = "GBX_Catalogo";
            this.GBX_Catalogo.Size = new System.Drawing.Size(375, 339);
            this.GBX_Catalogo.TabIndex = 3;
            this.GBX_Catalogo.TabStop = false;
            this.GBX_Catalogo.Tag = "Permisos.Catalogo";
            this.GBX_Catalogo.Text = "Catalogo";
            // 
            // BTN_AgregarPermiso
            // 
            this.BTN_AgregarPermiso.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_AgregarPermiso.Enabled = false;
            this.BTN_AgregarPermiso.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_AgregarPermiso.Location = new System.Drawing.Point(191, 292);
            this.BTN_AgregarPermiso.Name = "BTN_AgregarPermiso";
            this.BTN_AgregarPermiso.Size = new System.Drawing.Size(166, 31);
            this.BTN_AgregarPermiso.TabIndex = 5;
            this.BTN_AgregarPermiso.Tag = "Permisos.AgregarPermiso";
            this.BTN_AgregarPermiso.Text = "Agregar permiso";
            this.BTN_AgregarPermiso.UseVisualStyleBackColor = true;
            this.BTN_AgregarPermiso.Click += new System.EventHandler(this.BTN_AgregarPermiso_Click);
            // 
            // LBX_PermisosSimples
            // 
            this.LBX_PermisosSimples.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBX_PermisosSimples.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBX_PermisosSimples.FormattingEnabled = true;
            this.LBX_PermisosSimples.ItemHeight = 15;
            this.LBX_PermisosSimples.Location = new System.Drawing.Point(191, 52);
            this.LBX_PermisosSimples.Name = "LBX_PermisosSimples";
            this.LBX_PermisosSimples.Size = new System.Drawing.Size(166, 229);
            this.LBX_PermisosSimples.TabIndex = 4;
            this.LBX_PermisosSimples.SelectedIndexChanged += new System.EventHandler(this.LBX_PermisosSimples_SelectedIndexChanged);
            // 
            // LBL_PermisosSimples
            // 
            this.LBL_PermisosSimples.AutoSize = true;
            this.LBL_PermisosSimples.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_PermisosSimples.Location = new System.Drawing.Point(188, 27);
            this.LBL_PermisosSimples.Name = "LBL_PermisosSimples";
            this.LBL_PermisosSimples.Size = new System.Drawing.Size(99, 15);
            this.LBL_PermisosSimples.TabIndex = 3;
            this.LBL_PermisosSimples.Tag = "Permisos.PermisosSimples";
            this.LBL_PermisosSimples.Text = "Permisos simples";
            // 
            // BTN_AgregarFamilia
            // 
            this.BTN_AgregarFamilia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_AgregarFamilia.Enabled = false;
            this.BTN_AgregarFamilia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_AgregarFamilia.Location = new System.Drawing.Point(21, 292);
            this.BTN_AgregarFamilia.Name = "BTN_AgregarFamilia";
            this.BTN_AgregarFamilia.Size = new System.Drawing.Size(154, 31);
            this.BTN_AgregarFamilia.TabIndex = 2;
            this.BTN_AgregarFamilia.Tag = "Permisos.AgregarFamilia";
            this.BTN_AgregarFamilia.Text = "Agregar familia";
            this.BTN_AgregarFamilia.UseVisualStyleBackColor = true;
            this.BTN_AgregarFamilia.Click += new System.EventHandler(this.BTN_AgregarFamilia_Click);
            // 
            // LBX_Familias
            // 
            this.LBX_Familias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LBX_Familias.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBX_Familias.FormattingEnabled = true;
            this.LBX_Familias.ItemHeight = 15;
            this.LBX_Familias.Location = new System.Drawing.Point(21, 52);
            this.LBX_Familias.Name = "LBX_Familias";
            this.LBX_Familias.Size = new System.Drawing.Size(154, 229);
            this.LBX_Familias.TabIndex = 1;
            this.LBX_Familias.SelectedIndexChanged += new System.EventHandler(this.LBX_Familias_SelectedIndexChanged);
            // 
            // LBL_Familias
            // 
            this.LBL_Familias.AutoSize = true;
            this.LBL_Familias.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Familias.Location = new System.Drawing.Point(18, 27);
            this.LBL_Familias.Name = "LBL_Familias";
            this.LBL_Familias.Size = new System.Drawing.Size(51, 15);
            this.LBL_Familias.TabIndex = 0;
            this.LBL_Familias.Tag = "Permisos.Familias";
            this.LBL_Familias.Text = "Familias";
            // 
            // GBX_Composicion
            // 
            this.GBX_Composicion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBX_Composicion.Controls.Add(this.BTN_Limpiar);
            this.GBX_Composicion.Controls.Add(this.BTN_QuitarSeleccionado);
            this.GBX_Composicion.Controls.Add(this.TBX_Destino);
            this.GBX_Composicion.Controls.Add(this.LBL_Destino);
            this.GBX_Composicion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBX_Composicion.Location = new System.Drawing.Point(800, 72);
            this.GBX_Composicion.Name = "GBX_Composicion";
            this.GBX_Composicion.Size = new System.Drawing.Size(260, 505);
            this.GBX_Composicion.TabIndex = 4;
            this.GBX_Composicion.TabStop = false;
            this.GBX_Composicion.Tag = "Permisos.Composicion";
            this.GBX_Composicion.Text = "Composicion";
            // 
            // BTN_Limpiar
            // 
            this.BTN_Limpiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_Limpiar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Limpiar.Location = new System.Drawing.Point(20, 449);
            this.BTN_Limpiar.Name = "BTN_Limpiar";
            this.BTN_Limpiar.Size = new System.Drawing.Size(220, 32);
            this.BTN_Limpiar.TabIndex = 3;
            this.BTN_Limpiar.Tag = "Permisos.Limpiar";
            this.BTN_Limpiar.Text = "Limpiar";
            this.BTN_Limpiar.UseVisualStyleBackColor = true;
            this.BTN_Limpiar.Click += new System.EventHandler(this.BTN_Limpiar_Click);
            // 
            // BTN_QuitarSeleccionado
            // 
            this.BTN_QuitarSeleccionado.Enabled = false;
            this.BTN_QuitarSeleccionado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_QuitarSeleccionado.Location = new System.Drawing.Point(20, 101);
            this.BTN_QuitarSeleccionado.Name = "BTN_QuitarSeleccionado";
            this.BTN_QuitarSeleccionado.Size = new System.Drawing.Size(220, 32);
            this.BTN_QuitarSeleccionado.TabIndex = 2;
            this.BTN_QuitarSeleccionado.Tag = "Permisos.QuitarSeleccionado";
            this.BTN_QuitarSeleccionado.Text = "Quitar seleccionado";
            this.BTN_QuitarSeleccionado.UseVisualStyleBackColor = true;
            this.BTN_QuitarSeleccionado.Click += new System.EventHandler(this.BTN_QuitarSeleccionado_Click);
            // 
            // TBX_Destino
            // 
            this.TBX_Destino.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBX_Destino.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBX_Destino.Location = new System.Drawing.Point(20, 57);
            this.TBX_Destino.Name = "TBX_Destino";
            this.TBX_Destino.ReadOnly = true;
            this.TBX_Destino.Size = new System.Drawing.Size(220, 23);
            this.TBX_Destino.TabIndex = 1;
            // 
            // LBL_Destino
            // 
            this.LBL_Destino.AutoSize = true;
            this.LBL_Destino.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Destino.Location = new System.Drawing.Point(17, 31);
            this.LBL_Destino.Name = "LBL_Destino";
            this.LBL_Destino.Size = new System.Drawing.Size(47, 15);
            this.LBL_Destino.TabIndex = 0;
            this.LBL_Destino.Tag = "Permisos.Destino";
            this.LBL_Destino.Text = "Destino";
            // 
            // FrmAdministrarPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 601);
            this.Controls.Add(this.GBX_Composicion);
            this.Controls.Add(this.GBX_Catalogo);
            this.Controls.Add(this.GBX_Familia);
            this.Controls.Add(this.GBX_Arbol);
            this.Controls.Add(this.LBL_Titulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1100, 640);
            this.Name = "FrmAdministrarPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmAdministrarPermisos.Text";
            this.Text = "Administracion de permisos";
            this.Load += new System.EventHandler(this.FrmAdministrarPermisos_Load);
            this.GBX_Arbol.ResumeLayout(false);
            this.GBX_Familia.ResumeLayout(false);
            this.GBX_Familia.PerformLayout();
            this.GBX_Catalogo.ResumeLayout(false);
            this.GBX_Catalogo.PerformLayout();
            this.GBX_Composicion.ResumeLayout(false);
            this.GBX_Composicion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.GroupBox GBX_Arbol;
        private System.Windows.Forms.TreeView TVW_Permisos;
        private System.Windows.Forms.GroupBox GBX_Familia;
        private System.Windows.Forms.Button BTN_EliminarFamilia;
        private System.Windows.Forms.Button BTN_EditarFamilia;
        private System.Windows.Forms.Button BTN_CrearFamilia;
        private System.Windows.Forms.TextBox TBX_NombreFamilia;
        private System.Windows.Forms.Label LBL_NombreFamilia;
        private System.Windows.Forms.GroupBox GBX_Catalogo;
        private System.Windows.Forms.Button BTN_AgregarPermiso;
        private System.Windows.Forms.ListBox LBX_PermisosSimples;
        private System.Windows.Forms.Label LBL_PermisosSimples;
        private System.Windows.Forms.Button BTN_AgregarFamilia;
        private System.Windows.Forms.ListBox LBX_Familias;
        private System.Windows.Forms.Label LBL_Familias;
        private System.Windows.Forms.GroupBox GBX_Composicion;
        private System.Windows.Forms.Button BTN_Limpiar;
        private System.Windows.Forms.Button BTN_QuitarSeleccionado;
        private System.Windows.Forms.TextBox TBX_Destino;
        private System.Windows.Forms.Label LBL_Destino;
    }
}
