namespace PRESENTATION.Forms.Auth
{
    partial class FrmCuenta
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
            this.SuspendLayout();
            // 
            // LBL_Username
            // 
            this.LBL_Username.AutoSize = true;
            this.LBL_Username.Location = new System.Drawing.Point(50, 30);
            this.LBL_Username.Name = "LBL_Username";
            this.LBL_Username.Size = new System.Drawing.Size(50, 13);
            this.LBL_Username.TabIndex = 0;
            this.LBL_Username.Text = "Nombre: ";
            // 
            // LBL_FechaInicio
            // 
            this.LBL_FechaInicio.AutoSize = true;
            this.LBL_FechaInicio.Location = new System.Drawing.Point(50, 65);
            this.LBL_FechaInicio.Name = "LBL_FechaInicio";
            this.LBL_FechaInicio.Size = new System.Drawing.Size(136, 13);
            this.LBL_FechaInicio.TabIndex = 1;
            this.LBL_FechaInicio.Text = "Fecha de Inicio de Sesión: ";
            // 
            // FrmCuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LBL_FechaInicio);
            this.Controls.Add(this.LBL_Username);
            this.Name = "FrmCuenta";
            this.Text = "FrmCuenta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LBL_Username;
        private System.Windows.Forms.Label LBL_FechaInicio;
    }
}