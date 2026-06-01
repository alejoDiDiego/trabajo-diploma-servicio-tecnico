namespace UI.Forms.Auth
{
    partial class FrmLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.TBX_Username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBX_Password = new System.Windows.Forms.TextBox();
            this.BTN_IniciarSesion = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.SuspendLayout();

            // PNL_Header
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(340, 65);
            this.PNL_Header.TabIndex = 0;

            // LBL_Titulo
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Tag = "Login.Titulo";
            this.LBL_Titulo.Text = "Iniciar Sesión";

            // label1
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(40, 85);
            this.label1.Name = "label1";
            this.label1.Tag = "Campo.Username";
            this.label1.Text = "Nombre de Usuario";

            // TBX_Username
            this.TBX_Username.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TBX_Username.Location = new System.Drawing.Point(40, 105);
            this.TBX_Username.Name = "TBX_Username";
            this.TBX_Username.Size = new System.Drawing.Size(260, 26);
            this.TBX_Username.TabIndex = 1;

            // label2
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(40, 148);
            this.label2.Name = "label2";
            this.label2.Tag = "Campo.Password";
            this.label2.Text = "Contraseña";

            // TBX_Password
            this.TBX_Password.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.TBX_Password.Location = new System.Drawing.Point(40, 168);
            this.TBX_Password.Name = "TBX_Password";
            this.TBX_Password.PasswordChar = '*';
            this.TBX_Password.Size = new System.Drawing.Size(260, 26);
            this.TBX_Password.TabIndex = 2;

            // BTN_IniciarSesion
            this.BTN_IniciarSesion.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.BTN_IniciarSesion.FlatAppearance.BorderSize = 0;
            this.BTN_IniciarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IniciarSesion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.BTN_IniciarSesion.ForeColor = System.Drawing.Color.White;
            this.BTN_IniciarSesion.Location = new System.Drawing.Point(40, 222);
            this.BTN_IniciarSesion.Name = "BTN_IniciarSesion";
            this.BTN_IniciarSesion.Size = new System.Drawing.Size(260, 36);
            this.BTN_IniciarSesion.TabIndex = 3;
            this.BTN_IniciarSesion.Tag = "Login.Titulo";
            this.BTN_IniciarSesion.Text = "Iniciar Sesión";
            this.BTN_IniciarSesion.UseVisualStyleBackColor = false;
            this.BTN_IniciarSesion.Click += new System.EventHandler(this.BTN_IniciarSesion_Click);

            // FrmLogin
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(340, 290);
            this.Controls.Add(this.BTN_IniciarSesion);
            this.Controls.Add(this.TBX_Password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBX_Username);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmLogin.Text";
            this.Text = "Iniciar Sesión";
            this.Load += new System.EventHandler(this.FrmLogin_Load);

            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBX_Username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBX_Password;
        private System.Windows.Forms.Button BTN_IniciarSesion;
    }
}
