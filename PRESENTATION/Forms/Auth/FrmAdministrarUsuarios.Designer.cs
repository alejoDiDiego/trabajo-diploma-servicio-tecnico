namespace PRESENTATION.Forms.Auth
{
    partial class FrmAdministrarUsuarios
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
            this.BTN_CerrarSesion = new System.Windows.Forms.Button();
            this.LBL_FechaInicio = new System.Windows.Forms.Label();
            this.LBL_Username = new System.Windows.Forms.Label();
            this.LBL_Titulo = new System.Windows.Forms.Label();
            this.PNL_Permisos = new System.Windows.Forms.Panel();
            this.BTN_CrearUsuario = new System.Windows.Forms.Button();
            this.TBX_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBX_Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LBL_NuevoUsuario = new System.Windows.Forms.Label();
            this.DGV_Usuarios = new System.Windows.Forms.DataGridView();
            this.BTN_EliminarUsuario = new System.Windows.Forms.Button();
            this.BTN_EditarUsuario = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Permisos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // PNL_Header
            // 
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.BTN_CerrarSesion);
            this.PNL_Header.Controls.Add(this.LBL_FechaInicio);
            this.PNL_Header.Controls.Add(this.LBL_Username);
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(950, 70);
            this.PNL_Header.TabIndex = 0;
            // 
            // BTN_CerrarSesion
            // 
            this.BTN_CerrarSesion.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BTN_CerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CerrarSesion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_CerrarSesion.ForeColor = System.Drawing.Color.White;
            this.BTN_CerrarSesion.Location = new System.Drawing.Point(808, 20);
            this.BTN_CerrarSesion.Name = "BTN_CerrarSesion";
            this.BTN_CerrarSesion.Size = new System.Drawing.Size(125, 30);
            this.BTN_CerrarSesion.TabIndex = 10;
            this.BTN_CerrarSesion.Text = "Cerrar Sesión";
            this.BTN_CerrarSesion.UseVisualStyleBackColor = false;
            this.BTN_CerrarSesion.Click += new System.EventHandler(this.BTN_CerrarSesion_Click);
            // 
            // LBL_FechaInicio
            // 
            this.LBL_FechaInicio.AutoSize = true;
            this.LBL_FechaInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_FechaInicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.LBL_FechaInicio.Location = new System.Drawing.Point(220, 46);
            this.LBL_FechaInicio.Name = "LBL_FechaInicio";
            this.LBL_FechaInicio.Size = new System.Drawing.Size(91, 15);
            this.LBL_FechaInicio.TabIndex = 11;
            this.LBL_FechaInicio.Text = "Sesión iniciada: ";
            // 
            // LBL_Username
            // 
            this.LBL_Username.AutoSize = true;
            this.LBL_Username.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.LBL_Username.Location = new System.Drawing.Point(16, 46);
            this.LBL_Username.Name = "LBL_Username";
            this.LBL_Username.Size = new System.Drawing.Size(53, 15);
            this.LBL_Username.TabIndex = 12;
            this.LBL_Username.Text = "Usuario: ";
            // 
            // LBL_Titulo
            // 
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 10);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(256, 25);
            this.LBL_Titulo.TabIndex = 13;
            this.LBL_Titulo.Text = "Administración de Usuarios";
            // 
            // PNL_Permisos
            // 
            this.PNL_Permisos.Controls.Add(this.BTN_EditarUsuario);
            this.PNL_Permisos.Controls.Add(this.BTN_EliminarUsuario);
            this.PNL_Permisos.Controls.Add(this.BTN_CrearUsuario);
            this.PNL_Permisos.Controls.Add(this.TBX_Password);
            this.PNL_Permisos.Controls.Add(this.label2);
            this.PNL_Permisos.Controls.Add(this.TBX_Username);
            this.PNL_Permisos.Controls.Add(this.label1);
            this.PNL_Permisos.Controls.Add(this.LBL_NuevoUsuario);
            this.PNL_Permisos.Controls.Add(this.DGV_Usuarios);
            this.PNL_Permisos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PNL_Permisos.Location = new System.Drawing.Point(0, 70);
            this.PNL_Permisos.Name = "PNL_Permisos";
            this.PNL_Permisos.Padding = new System.Windows.Forms.Padding(15);
            this.PNL_Permisos.Size = new System.Drawing.Size(950, 490);
            this.PNL_Permisos.TabIndex = 1;
            // 
            // BTN_CrearUsuario
            // 
            this.BTN_CrearUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_CrearUsuario.FlatAppearance.BorderSize = 0;
            this.BTN_CrearUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CrearUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_CrearUsuario.ForeColor = System.Drawing.Color.White;
            this.BTN_CrearUsuario.Location = new System.Drawing.Point(495, 439);
            this.BTN_CrearUsuario.Name = "BTN_CrearUsuario";
            this.BTN_CrearUsuario.Size = new System.Drawing.Size(160, 28);
            this.BTN_CrearUsuario.TabIndex = 3;
            this.BTN_CrearUsuario.Text = "Crear Usuario";
            this.BTN_CrearUsuario.UseVisualStyleBackColor = false;
            this.BTN_CrearUsuario.Click += new System.EventHandler(this.BTN_CrearUsuario_Click);
            // 
            // TBX_Password
            // 
            this.TBX_Password.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TBX_Password.Location = new System.Drawing.Point(255, 441);
            this.TBX_Password.Name = "TBX_Password";
            this.TBX_Password.PasswordChar = '*';
            this.TBX_Password.Size = new System.Drawing.Size(220, 25);
            this.TBX_Password.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(255, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Contraseña";
            // 
            // TBX_Username
            // 
            this.TBX_Username.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TBX_Username.Location = new System.Drawing.Point(15, 441);
            this.TBX_Username.Name = "TBX_Username";
            this.TBX_Username.Size = new System.Drawing.Size(220, 25);
            this.TBX_Username.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(15, 422);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre de Usuario";
            // 
            // LBL_NuevoUsuario
            // 
            this.LBL_NuevoUsuario.AutoSize = true;
            this.LBL_NuevoUsuario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.LBL_NuevoUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.LBL_NuevoUsuario.Location = new System.Drawing.Point(15, 392);
            this.LBL_NuevoUsuario.Name = "LBL_NuevoUsuario";
            this.LBL_NuevoUsuario.Size = new System.Drawing.Size(108, 19);
            this.LBL_NuevoUsuario.TabIndex = 6;
            this.LBL_NuevoUsuario.Text = "Nuevo Usuario";
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
            this.DGV_Usuarios.Location = new System.Drawing.Point(15, 15);
            this.DGV_Usuarios.Name = "DGV_Usuarios";
            this.DGV_Usuarios.ReadOnly = true;
            this.DGV_Usuarios.RowHeadersVisible = false;
            this.DGV_Usuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Usuarios.Size = new System.Drawing.Size(905, 360);
            this.DGV_Usuarios.TabIndex = 0;
            this.DGV_Usuarios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Usuarios_CellClick);
            // 
            // BTN_EliminarUsuario
            // 
            this.BTN_EliminarUsuario.BackColor = System.Drawing.Color.Maroon;
            this.BTN_EliminarUsuario.Enabled = false;
            this.BTN_EliminarUsuario.FlatAppearance.BorderSize = 0;
            this.BTN_EliminarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EliminarUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_EliminarUsuario.ForeColor = System.Drawing.Color.White;
            this.BTN_EliminarUsuario.Location = new System.Drawing.Point(760, 383);
            this.BTN_EliminarUsuario.Name = "BTN_EliminarUsuario";
            this.BTN_EliminarUsuario.Size = new System.Drawing.Size(160, 28);
            this.BTN_EliminarUsuario.TabIndex = 7;
            this.BTN_EliminarUsuario.Text = "Eliminar Usuario";
            this.BTN_EliminarUsuario.UseVisualStyleBackColor = false;
            this.BTN_EliminarUsuario.Click += new System.EventHandler(this.BTN_EliminarUsuario_Click);
            // 
            // BTN_EditarUsuario
            // 
            this.BTN_EditarUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BTN_EditarUsuario.Enabled = false;
            this.BTN_EditarUsuario.FlatAppearance.BorderSize = 0;
            this.BTN_EditarUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_EditarUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_EditarUsuario.ForeColor = System.Drawing.Color.White;
            this.BTN_EditarUsuario.Location = new System.Drawing.Point(661, 438);
            this.BTN_EditarUsuario.Name = "BTN_EditarUsuario";
            this.BTN_EditarUsuario.Size = new System.Drawing.Size(160, 28);
            this.BTN_EditarUsuario.TabIndex = 8;
            this.BTN_EditarUsuario.Text = "Editar Usuario";
            this.BTN_EditarUsuario.UseVisualStyleBackColor = false;
            this.BTN_EditarUsuario.Click += new System.EventHandler(this.BTN_EditarUsuario_Click);
            // 
            // FrmAdministrarUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(950, 560);
            this.Controls.Add(this.PNL_Permisos);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmAdministrarUsuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administración de Usuarios";
            this.Load += new System.EventHandler(this.FrmAdministrarCuentas_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Permisos.ResumeLayout(false);
            this.PNL_Permisos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_Username;
        private System.Windows.Forms.Label LBL_FechaInicio;
        private System.Windows.Forms.Button BTN_CerrarSesion;
        private System.Windows.Forms.Panel PNL_Permisos;
        private System.Windows.Forms.DataGridView DGV_Usuarios;
        private System.Windows.Forms.Label LBL_NuevoUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBX_Username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBX_Password;
        private System.Windows.Forms.Button BTN_CrearUsuario;
        private System.Windows.Forms.Button BTN_EditarUsuario;
        private System.Windows.Forms.Button BTN_EliminarUsuario;
    }
}
