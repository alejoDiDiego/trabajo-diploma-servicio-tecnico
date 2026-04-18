namespace PRESENTATION.Forms.Auth
{
    partial class FrmAdministrarUsuarios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LBL_Username = new System.Windows.Forms.Label();
            this.LBL_FechaInicio = new System.Windows.Forms.Label();
            this.DGV_Usuarios = new System.Windows.Forms.DataGridView();
            this.PNL_Permisos = new System.Windows.Forms.Panel();
            this.BTN_CrearUsuario = new System.Windows.Forms.Button();
            this.TBX_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBX_Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).BeginInit();
            this.PNL_Permisos.SuspendLayout();
            this.SuspendLayout();
            // 
            // LBL_Username
            // 
            this.LBL_Username.AutoSize = true;
            this.LBL_Username.Location = new System.Drawing.Point(22, 39);
            this.LBL_Username.Name = "LBL_Username";
            this.LBL_Username.Size = new System.Drawing.Size(50, 13);
            this.LBL_Username.TabIndex = 0;
            this.LBL_Username.Text = "Nombre: ";
            // 
            // LBL_FechaInicio
            // 
            this.LBL_FechaInicio.AutoSize = true;
            this.LBL_FechaInicio.Location = new System.Drawing.Point(22, 74);
            this.LBL_FechaInicio.Name = "LBL_FechaInicio";
            this.LBL_FechaInicio.Size = new System.Drawing.Size(136, 13);
            this.LBL_FechaInicio.TabIndex = 1;
            this.LBL_FechaInicio.Text = "Fecha de Inicio de Sesión: ";
            // 
            // DGV_Usuarios
            // 
            this.DGV_Usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Usuarios.Location = new System.Drawing.Point(18, 21);
            this.DGV_Usuarios.Name = "DGV_Usuarios";
            this.DGV_Usuarios.Size = new System.Drawing.Size(785, 247);
            this.DGV_Usuarios.TabIndex = 2;
            // 
            // PNL_Permisos
            // 
            this.PNL_Permisos.Controls.Add(this.BTN_CrearUsuario);
            this.PNL_Permisos.Controls.Add(this.TBX_Password);
            this.PNL_Permisos.Controls.Add(this.label2);
            this.PNL_Permisos.Controls.Add(this.TBX_Username);
            this.PNL_Permisos.Controls.Add(this.label1);
            this.PNL_Permisos.Controls.Add(this.DGV_Usuarios);
            this.PNL_Permisos.Location = new System.Drawing.Point(25, 144);
            this.PNL_Permisos.Name = "PNL_Permisos";
            this.PNL_Permisos.Size = new System.Drawing.Size(835, 468);
            this.PNL_Permisos.TabIndex = 3;
            // 
            // BTN_CrearUsuario
            // 
            this.BTN_CrearUsuario.Location = new System.Drawing.Point(29, 428);
            this.BTN_CrearUsuario.Name = "BTN_CrearUsuario";
            this.BTN_CrearUsuario.Size = new System.Drawing.Size(191, 23);
            this.BTN_CrearUsuario.TabIndex = 16;
            this.BTN_CrearUsuario.Text = "Crear Usuario";
            this.BTN_CrearUsuario.UseVisualStyleBackColor = true;
            this.BTN_CrearUsuario.Click += new System.EventHandler(this.BTN_CrearUsuario_Click);
            // 
            // TBX_Password
            // 
            this.TBX_Password.Location = new System.Drawing.Point(29, 376);
            this.TBX_Password.Name = "TBX_Password";
            this.TBX_Password.Size = new System.Drawing.Size(191, 20);
            this.TBX_Password.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Contraseña";
            // 
            // TBX_Username
            // 
            this.TBX_Username.Location = new System.Drawing.Point(29, 318);
            this.TBX_Username.Name = "TBX_Username";
            this.TBX_Username.Size = new System.Drawing.Size(191, 20);
            this.TBX_Username.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Nombre de Usuario";
            // 
            // FrmAdministrarUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 715);
            this.Controls.Add(this.PNL_Permisos);
            this.Controls.Add(this.LBL_FechaInicio);
            this.Controls.Add(this.LBL_Username);
            this.Name = "FrmAdministrarUsuarios";
            this.Text = "FrmCuenta";
            this.Load += new System.EventHandler(this.FrmAdministrarCuentas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Usuarios)).EndInit();
            this.PNL_Permisos.ResumeLayout(false);
            this.PNL_Permisos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LBL_Username;
        private System.Windows.Forms.Label LBL_FechaInicio;
        private System.Windows.Forms.DataGridView DGV_Usuarios;
        private System.Windows.Forms.Panel PNL_Permisos;
        private System.Windows.Forms.Button BTN_CrearUsuario;
        private System.Windows.Forms.TextBox TBX_Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBX_Username;
        private System.Windows.Forms.Label label1;
    }
}