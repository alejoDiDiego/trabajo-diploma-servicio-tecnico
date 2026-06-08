namespace UI.Forms.Auth
{
    partial class FrmAsignarPermisosUsuario
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
            this.GBX_Usuarios = new System.Windows.Forms.GroupBox();
            this.DGV_Usuarios = new System.Windows.Forms.DataGridView();
            this.GBX_Disponibles = new System.Windows.Forms.GroupBox();
            this.LBX_Disponibles = new System.Windows.Forms.ListBox();
            this.GBX_Asignadas = new System.Windows.Forms.GroupBox();
            this.LBX_Asignadas = new System.Windows.Forms.ListBox();
            this.BTN_Asignar = new System.Windows.Forms.Button();
            this.BTN_Quitar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.GBX_Usuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).BeginInit();
            this.GBX_Disponibles.SuspendLayout();
            this.GBX_Asignadas.SuspendLayout();
            this.SuspendLayout();
            // 
            // PNL_Header
            // 
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(980, 65);
            this.PNL_Header.TabIndex = 0;
            // 
            // LBL_Titulo
            // 
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 18);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(317, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "AsignarPermisos.Titulo";
            this.LBL_Titulo.Text = "Asignacion de permisos a usuarios";
            // 
            // GBX_Usuarios
            // 
            this.GBX_Usuarios.Controls.Add(this.DGV_Usuarios);
            this.GBX_Usuarios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GBX_Usuarios.Location = new System.Drawing.Point(12, 82);
            this.GBX_Usuarios.Name = "GBX_Usuarios";
            this.GBX_Usuarios.Size = new System.Drawing.Size(415, 430);
            this.GBX_Usuarios.TabIndex = 1;
            this.GBX_Usuarios.TabStop = false;
            this.GBX_Usuarios.Tag = "AsignarPermisos.Usuarios";
            this.GBX_Usuarios.Text = "Usuarios";
            // 
            // DGV_Usuarios
            // 
            this.DGV_Usuarios.AllowUserToAddRows = false;
            this.DGV_Usuarios.AllowUserToDeleteRows = false;
            this.DGV_Usuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Usuarios.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Usuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Usuarios.ColumnHeadersHeight = 32;
            this.DGV_Usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_Usuarios.Location = new System.Drawing.Point(15, 25);
            this.DGV_Usuarios.Name = "DGV_Usuarios";
            this.DGV_Usuarios.ReadOnly = true;
            this.DGV_Usuarios.RowHeadersVisible = false;
            this.DGV_Usuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Usuarios.Size = new System.Drawing.Size(380, 385);
            this.DGV_Usuarios.TabIndex = 0;
            this.DGV_Usuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Usuarios_CellClick);
            // 
            // GBX_Disponibles
            // 
            this.GBX_Disponibles.Controls.Add(this.LBX_Disponibles);
            this.GBX_Disponibles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GBX_Disponibles.Location = new System.Drawing.Point(450, 82);
            this.GBX_Disponibles.Name = "GBX_Disponibles";
            this.GBX_Disponibles.Size = new System.Drawing.Size(230, 430);
            this.GBX_Disponibles.TabIndex = 2;
            this.GBX_Disponibles.TabStop = false;
            this.GBX_Disponibles.Tag = "AsignarPermisos.FamiliasDisponibles";
            this.GBX_Disponibles.Text = "Familias disponibles";
            // 
            // LBX_Disponibles
            // 
            this.LBX_Disponibles.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBX_Disponibles.FormattingEnabled = true;
            this.LBX_Disponibles.ItemHeight = 15;
            this.LBX_Disponibles.Location = new System.Drawing.Point(15, 25);
            this.LBX_Disponibles.Name = "LBX_Disponibles";
            this.LBX_Disponibles.Size = new System.Drawing.Size(200, 394);
            this.LBX_Disponibles.TabIndex = 0;
            this.LBX_Disponibles.SelectedIndexChanged += new System.EventHandler(this.LBX_Disponibles_SelectedIndexChanged);
            // 
            // GBX_Asignadas
            // 
            this.GBX_Asignadas.Controls.Add(this.LBX_Asignadas);
            this.GBX_Asignadas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GBX_Asignadas.Location = new System.Drawing.Point(746, 82);
            this.GBX_Asignadas.Name = "GBX_Asignadas";
            this.GBX_Asignadas.Size = new System.Drawing.Size(230, 430);
            this.GBX_Asignadas.TabIndex = 3;
            this.GBX_Asignadas.TabStop = false;
            this.GBX_Asignadas.Tag = "AsignarPermisos.FamiliasAsignadas";
            this.GBX_Asignadas.Text = "Familias asignadas";
            // 
            // LBX_Asignadas
            // 
            this.LBX_Asignadas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBX_Asignadas.FormattingEnabled = true;
            this.LBX_Asignadas.ItemHeight = 15;
            this.LBX_Asignadas.Location = new System.Drawing.Point(15, 25);
            this.LBX_Asignadas.Name = "LBX_Asignadas";
            this.LBX_Asignadas.Size = new System.Drawing.Size(200, 394);
            this.LBX_Asignadas.TabIndex = 0;
            this.LBX_Asignadas.SelectedIndexChanged += new System.EventHandler(this.LBX_Asignadas_SelectedIndexChanged);
            // 
            // BTN_Asignar
            // 
            this.BTN_Asignar.Enabled = false;
            this.BTN_Asignar.Location = new System.Drawing.Point(692, 232);
            this.BTN_Asignar.Name = "BTN_Asignar";
            this.BTN_Asignar.Size = new System.Drawing.Size(35, 32);
            this.BTN_Asignar.TabIndex = 4;
            this.BTN_Asignar.Tag = "AsignarPermisos.Asignar";
            this.BTN_Asignar.Text = ">";
            this.BTN_Asignar.UseVisualStyleBackColor = true;
            this.BTN_Asignar.Click += new System.EventHandler(this.BTN_Asignar_Click);
            // 
            // BTN_Quitar
            // 
            this.BTN_Quitar.Enabled = false;
            this.BTN_Quitar.Location = new System.Drawing.Point(692, 282);
            this.BTN_Quitar.Name = "BTN_Quitar";
            this.BTN_Quitar.Size = new System.Drawing.Size(35, 32);
            this.BTN_Quitar.TabIndex = 5;
            this.BTN_Quitar.Tag = "AsignarPermisos.Quitar";
            this.BTN_Quitar.Text = "<";
            this.BTN_Quitar.UseVisualStyleBackColor = true;
            this.BTN_Quitar.Click += new System.EventHandler(this.BTN_Quitar_Click);
            // 
            // FrmAsignarPermisosUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(980, 530);
            this.Controls.Add(this.BTN_Quitar);
            this.Controls.Add(this.BTN_Asignar);
            this.Controls.Add(this.GBX_Asignadas);
            this.Controls.Add(this.GBX_Disponibles);
            this.Controls.Add(this.GBX_Usuarios);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAsignarPermisosUsuario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmAsignarPermisosUsuario.Text";
            this.Text = "Asignacion de permisos a usuarios";
            this.Load += new System.EventHandler(this.FrmAsignarPermisosUsuario_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.GBX_Usuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).EndInit();
            this.GBX_Disponibles.ResumeLayout(false);
            this.GBX_Asignadas.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.GroupBox GBX_Usuarios;
        private System.Windows.Forms.DataGridView DGV_Usuarios;
        private System.Windows.Forms.GroupBox GBX_Disponibles;
        private System.Windows.Forms.ListBox LBX_Disponibles;
        private System.Windows.Forms.GroupBox GBX_Asignadas;
        private System.Windows.Forms.ListBox LBX_Asignadas;
        private System.Windows.Forms.Button BTN_Asignar;
        private System.Windows.Forms.Button BTN_Quitar;
    }
}
