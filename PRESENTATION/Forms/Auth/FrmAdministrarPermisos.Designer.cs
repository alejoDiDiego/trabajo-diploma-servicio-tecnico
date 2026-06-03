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
            this.GBX_Detalle = new System.Windows.Forms.GroupBox();
            this.BTN_Limpiar = new System.Windows.Forms.Button();
            this.BTN_Mover = new System.Windows.Forms.Button();
            this.BTN_Eliminar = new System.Windows.Forms.Button();
            this.BTN_Editar = new System.Windows.Forms.Button();
            this.BTN_Crear = new System.Windows.Forms.Button();
            this.CBX_Padre = new System.Windows.Forms.ComboBox();
            this.LBL_Padre = new System.Windows.Forms.Label();
            this.CBX_Tipo = new System.Windows.Forms.ComboBox();
            this.LBL_Tipo = new System.Windows.Forms.Label();
            this.TBX_Descripcion = new System.Windows.Forms.TextBox();
            this.LBL_Descripcion = new System.Windows.Forms.Label();
            this.TBX_Codigo = new System.Windows.Forms.TextBox();
            this.LBL_Codigo = new System.Windows.Forms.Label();
            this.TBX_Nombre = new System.Windows.Forms.TextBox();
            this.LBL_Nombre = new System.Windows.Forms.Label();
            this.GBX_Arbol.SuspendLayout();
            this.GBX_Detalle.SuspendLayout();
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
            this.GBX_Arbol.Size = new System.Drawing.Size(410, 505);
            this.GBX_Arbol.TabIndex = 1;
            this.GBX_Arbol.TabStop = false;
            this.GBX_Arbol.Tag = "Permisos.Arbol";
            this.GBX_Arbol.Text = "Arbol de permisos";
            // 
            // TVW_Permisos
            // 
            this.TVW_Permisos.AllowDrop = true;
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
            this.TVW_Permisos.Size = new System.Drawing.Size(382, 462);
            this.TVW_Permisos.TabIndex = 0;
            this.TVW_Permisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TVW_Permisos_AfterSelect);
            this.TVW_Permisos.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TVW_Permisos_ItemDrag);
            this.TVW_Permisos.DragDrop += new System.Windows.Forms.DragEventHandler(this.TVW_Permisos_DragDrop);
            this.TVW_Permisos.DragEnter += new System.Windows.Forms.DragEventHandler(this.TVW_Permisos_DragEnter);
            this.TVW_Permisos.DragOver += new System.Windows.Forms.DragEventHandler(this.TVW_Permisos_DragOver);
            // 
            // GBX_Detalle
            // 
            this.GBX_Detalle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GBX_Detalle.Controls.Add(this.BTN_Limpiar);
            this.GBX_Detalle.Controls.Add(this.BTN_Mover);
            this.GBX_Detalle.Controls.Add(this.BTN_Eliminar);
            this.GBX_Detalle.Controls.Add(this.BTN_Editar);
            this.GBX_Detalle.Controls.Add(this.BTN_Crear);
            this.GBX_Detalle.Controls.Add(this.CBX_Padre);
            this.GBX_Detalle.Controls.Add(this.LBL_Padre);
            this.GBX_Detalle.Controls.Add(this.CBX_Tipo);
            this.GBX_Detalle.Controls.Add(this.LBL_Tipo);
            this.GBX_Detalle.Controls.Add(this.TBX_Descripcion);
            this.GBX_Detalle.Controls.Add(this.LBL_Descripcion);
            this.GBX_Detalle.Controls.Add(this.TBX_Codigo);
            this.GBX_Detalle.Controls.Add(this.LBL_Codigo);
            this.GBX_Detalle.Controls.Add(this.TBX_Nombre);
            this.GBX_Detalle.Controls.Add(this.LBL_Nombre);
            this.GBX_Detalle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GBX_Detalle.Location = new System.Drawing.Point(455, 72);
            this.GBX_Detalle.Name = "GBX_Detalle";
            this.GBX_Detalle.Size = new System.Drawing.Size(505, 505);
            this.GBX_Detalle.TabIndex = 2;
            this.GBX_Detalle.TabStop = false;
            this.GBX_Detalle.Tag = "Permisos.Detalle";
            this.GBX_Detalle.Text = "Detalle";
            // 
            // BTN_Limpiar
            // 
            this.BTN_Limpiar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Limpiar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Limpiar.Location = new System.Drawing.Point(348, 442);
            this.BTN_Limpiar.Name = "BTN_Limpiar";
            this.BTN_Limpiar.Size = new System.Drawing.Size(130, 34);
            this.BTN_Limpiar.TabIndex = 14;
            this.BTN_Limpiar.Tag = "Permisos.Limpiar";
            this.BTN_Limpiar.Text = "Limpiar";
            this.BTN_Limpiar.UseVisualStyleBackColor = true;
            this.BTN_Limpiar.Click += new System.EventHandler(this.BTN_Limpiar_Click);
            // 
            // BTN_Mover
            // 
            this.BTN_Mover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_Mover.Enabled = false;
            this.BTN_Mover.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Mover.Location = new System.Drawing.Point(174, 442);
            this.BTN_Mover.Name = "BTN_Mover";
            this.BTN_Mover.Size = new System.Drawing.Size(130, 34);
            this.BTN_Mover.TabIndex = 13;
            this.BTN_Mover.Tag = "Permisos.Mover";
            this.BTN_Mover.Text = "Mover";
            this.BTN_Mover.UseVisualStyleBackColor = true;
            this.BTN_Mover.Click += new System.EventHandler(this.BTN_Mover_Click);
            // 
            // BTN_Eliminar
            // 
            this.BTN_Eliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_Eliminar.Enabled = false;
            this.BTN_Eliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Eliminar.Location = new System.Drawing.Point(28, 442);
            this.BTN_Eliminar.Name = "BTN_Eliminar";
            this.BTN_Eliminar.Size = new System.Drawing.Size(130, 34);
            this.BTN_Eliminar.TabIndex = 12;
            this.BTN_Eliminar.Tag = "Permisos.Eliminar";
            this.BTN_Eliminar.Text = "Eliminar";
            this.BTN_Eliminar.UseVisualStyleBackColor = true;
            this.BTN_Eliminar.Click += new System.EventHandler(this.BTN_Eliminar_Click);
            // 
            // BTN_Editar
            // 
            this.BTN_Editar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_Editar.Enabled = false;
            this.BTN_Editar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Editar.Location = new System.Drawing.Point(174, 392);
            this.BTN_Editar.Name = "BTN_Editar";
            this.BTN_Editar.Size = new System.Drawing.Size(130, 34);
            this.BTN_Editar.TabIndex = 11;
            this.BTN_Editar.Tag = "Permisos.Editar";
            this.BTN_Editar.Text = "Editar";
            this.BTN_Editar.UseVisualStyleBackColor = true;
            this.BTN_Editar.Click += new System.EventHandler(this.BTN_Editar_Click);
            // 
            // BTN_Crear
            // 
            this.BTN_Crear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BTN_Crear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Crear.Location = new System.Drawing.Point(28, 392);
            this.BTN_Crear.Name = "BTN_Crear";
            this.BTN_Crear.Size = new System.Drawing.Size(130, 34);
            this.BTN_Crear.TabIndex = 10;
            this.BTN_Crear.Tag = "Permisos.Crear";
            this.BTN_Crear.Text = "Crear";
            this.BTN_Crear.UseVisualStyleBackColor = true;
            this.BTN_Crear.Click += new System.EventHandler(this.BTN_Crear_Click);
            // 
            // CBX_Padre
            // 
            this.CBX_Padre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CBX_Padre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBX_Padre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBX_Padre.FormattingEnabled = true;
            this.CBX_Padre.Location = new System.Drawing.Point(160, 326);
            this.CBX_Padre.Name = "CBX_Padre";
            this.CBX_Padre.Size = new System.Drawing.Size(318, 23);
            this.CBX_Padre.TabIndex = 9;
            // 
            // LBL_Padre
            // 
            this.LBL_Padre.AutoSize = true;
            this.LBL_Padre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Padre.Location = new System.Drawing.Point(25, 330);
            this.LBL_Padre.Name = "LBL_Padre";
            this.LBL_Padre.Size = new System.Drawing.Size(35, 13);
            this.LBL_Padre.TabIndex = 8;
            this.LBL_Padre.Tag = "Permisos.Padre";
            this.LBL_Padre.Text = "Padre";
            // 
            // CBX_Tipo
            // 
            this.CBX_Tipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CBX_Tipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBX_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBX_Tipo.FormattingEnabled = true;
            this.CBX_Tipo.Location = new System.Drawing.Point(160, 282);
            this.CBX_Tipo.Name = "CBX_Tipo";
            this.CBX_Tipo.Size = new System.Drawing.Size(318, 23);
            this.CBX_Tipo.TabIndex = 7;
            // 
            // LBL_Tipo
            // 
            this.LBL_Tipo.AutoSize = true;
            this.LBL_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Tipo.Location = new System.Drawing.Point(25, 286);
            this.LBL_Tipo.Name = "LBL_Tipo";
            this.LBL_Tipo.Size = new System.Drawing.Size(28, 13);
            this.LBL_Tipo.TabIndex = 6;
            this.LBL_Tipo.Tag = "Permisos.Tipo";
            this.LBL_Tipo.Text = "Tipo";
            // 
            // TBX_Descripcion
            // 
            this.TBX_Descripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBX_Descripcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBX_Descripcion.Location = new System.Drawing.Point(160, 143);
            this.TBX_Descripcion.MaxLength = 500;
            this.TBX_Descripcion.Multiline = true;
            this.TBX_Descripcion.Name = "TBX_Descripcion";
            this.TBX_Descripcion.Size = new System.Drawing.Size(318, 108);
            this.TBX_Descripcion.TabIndex = 5;
            // 
            // LBL_Descripcion
            // 
            this.LBL_Descripcion.AutoSize = true;
            this.LBL_Descripcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Descripcion.Location = new System.Drawing.Point(25, 146);
            this.LBL_Descripcion.Name = "LBL_Descripcion";
            this.LBL_Descripcion.Size = new System.Drawing.Size(63, 13);
            this.LBL_Descripcion.TabIndex = 4;
            this.LBL_Descripcion.Tag = "Permisos.Descripcion";
            this.LBL_Descripcion.Text = "Descripcion";
            // 
            // TBX_Codigo
            // 
            this.TBX_Codigo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBX_Codigo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBX_Codigo.Location = new System.Drawing.Point(160, 94);
            this.TBX_Codigo.MaxLength = 100;
            this.TBX_Codigo.Name = "TBX_Codigo";
            this.TBX_Codigo.Size = new System.Drawing.Size(318, 23);
            this.TBX_Codigo.TabIndex = 3;
            // 
            // LBL_Codigo
            // 
            this.LBL_Codigo.AutoSize = true;
            this.LBL_Codigo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Codigo.Location = new System.Drawing.Point(25, 98);
            this.LBL_Codigo.Name = "LBL_Codigo";
            this.LBL_Codigo.Size = new System.Drawing.Size(40, 13);
            this.LBL_Codigo.TabIndex = 2;
            this.LBL_Codigo.Tag = "Permisos.Codigo";
            this.LBL_Codigo.Text = "Codigo";
            // 
            // TBX_Nombre
            // 
            this.TBX_Nombre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBX_Nombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBX_Nombre.Location = new System.Drawing.Point(160, 46);
            this.TBX_Nombre.MaxLength = 100;
            this.TBX_Nombre.Name = "TBX_Nombre";
            this.TBX_Nombre.Size = new System.Drawing.Size(318, 23);
            this.TBX_Nombre.TabIndex = 1;
            // 
            // LBL_Nombre
            // 
            this.LBL_Nombre.AutoSize = true;
            this.LBL_Nombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Nombre.Location = new System.Drawing.Point(25, 50);
            this.LBL_Nombre.Name = "LBL_Nombre";
            this.LBL_Nombre.Size = new System.Drawing.Size(44, 13);
            this.LBL_Nombre.TabIndex = 0;
            this.LBL_Nombre.Tag = "Permisos.Nombre";
            this.LBL_Nombre.Text = "Nombre";
            // 
            // FrmAdministrarPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 601);
            this.Controls.Add(this.GBX_Detalle);
            this.Controls.Add(this.GBX_Arbol);
            this.Controls.Add(this.LBL_Titulo);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "FrmAdministrarPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmAdministrarPermisos.Text";
            this.Text = "Administracion de permisos";
            this.Load += new System.EventHandler(this.FrmAdministrarPermisos_Load);
            this.GBX_Arbol.ResumeLayout(false);
            this.GBX_Detalle.ResumeLayout(false);
            this.GBX_Detalle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.GroupBox GBX_Arbol;
        private System.Windows.Forms.TreeView TVW_Permisos;
        private System.Windows.Forms.GroupBox GBX_Detalle;
        private System.Windows.Forms.Label LBL_Nombre;
        private System.Windows.Forms.TextBox TBX_Nombre;
        private System.Windows.Forms.Label LBL_Codigo;
        private System.Windows.Forms.TextBox TBX_Codigo;
        private System.Windows.Forms.Label LBL_Descripcion;
        private System.Windows.Forms.TextBox TBX_Descripcion;
        private System.Windows.Forms.Label LBL_Tipo;
        private System.Windows.Forms.ComboBox CBX_Tipo;
        private System.Windows.Forms.Label LBL_Padre;
        private System.Windows.Forms.ComboBox CBX_Padre;
        private System.Windows.Forms.Button BTN_Crear;
        private System.Windows.Forms.Button BTN_Editar;
        private System.Windows.Forms.Button BTN_Eliminar;
        private System.Windows.Forms.Button BTN_Mover;
        private System.Windows.Forms.Button BTN_Limpiar;
    }
}
