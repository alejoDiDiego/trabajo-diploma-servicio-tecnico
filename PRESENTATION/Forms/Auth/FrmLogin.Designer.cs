namespace PRESENTATION.Forms.Auth
{
    partial class FrmLogin
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
            this.label1 = new System.Windows.Forms.Label();
            this.TBX_Username = new System.Windows.Forms.TextBox();
            this.TBX_Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BTN_IniciarSesion = new System.Windows.Forms.Button();
            this.BTN_Volver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre de Usuario";
            // 
            // TBX_Username
            // 
            this.TBX_Username.Location = new System.Drawing.Point(21, 37);
            this.TBX_Username.Name = "TBX_Username";
            this.TBX_Username.Size = new System.Drawing.Size(191, 20);
            this.TBX_Username.TabIndex = 1;
            // 
            // TBX_Password
            // 
            this.TBX_Password.Location = new System.Drawing.Point(21, 95);
            this.TBX_Password.Name = "TBX_Password";
            this.TBX_Password.Size = new System.Drawing.Size(191, 20);
            this.TBX_Password.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Contraseña";
            // 
            // BTN_IniciarSesion
            // 
            this.BTN_IniciarSesion.Location = new System.Drawing.Point(21, 147);
            this.BTN_IniciarSesion.Name = "BTN_IniciarSesion";
            this.BTN_IniciarSesion.Size = new System.Drawing.Size(191, 23);
            this.BTN_IniciarSesion.TabIndex = 4;
            this.BTN_IniciarSesion.Text = "Iniciar Sesion";
            this.BTN_IniciarSesion.UseVisualStyleBackColor = true;
            this.BTN_IniciarSesion.Click += new System.EventHandler(this.BTN_IniciarSesion_Click);
            // 
            // BTN_Volver
            // 
            this.BTN_Volver.Location = new System.Drawing.Point(21, 176);
            this.BTN_Volver.Name = "BTN_Volver";
            this.BTN_Volver.Size = new System.Drawing.Size(191, 23);
            this.BTN_Volver.TabIndex = 5;
            this.BTN_Volver.Text = "Volver";
            this.BTN_Volver.UseVisualStyleBackColor = true;
            this.BTN_Volver.Click += new System.EventHandler(this.BTN_Volver_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 223);
            this.Controls.Add(this.BTN_Volver);
            this.Controls.Add(this.BTN_IniciarSesion);
            this.Controls.Add(this.TBX_Password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TBX_Username);
            this.Controls.Add(this.label1);
            this.Name = "FrmLogin";
            this.Text = "FrmLogin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBX_Username;
        private System.Windows.Forms.TextBox TBX_Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BTN_IniciarSesion;
        private System.Windows.Forms.Button BTN_Volver;
    }
}