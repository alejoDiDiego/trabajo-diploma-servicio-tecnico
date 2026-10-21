namespace UI.Forms.Catalogos
{
    partial class FrmCatalogoEditar
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
            this.LBL_Nombre = new System.Windows.Forms.Label();
            this.TXT_Nombre = new System.Windows.Forms.TextBox();
            this.BTN_Aceptar = new System.Windows.Forms.Button();
            this.BTN_Cancelar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(400, 60);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Titulo
            //
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(120, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Catalogos.EditarNombre";
            this.LBL_Titulo.Text = "Catalogo";
            //
            // LBL_Nombre
            //
            this.LBL_Nombre.AutoSize = true;
            this.LBL_Nombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Nombre.Location = new System.Drawing.Point(15, 85);
            this.LBL_Nombre.Name = "LBL_Nombre";
            this.LBL_Nombre.Size = new System.Drawing.Size(54, 15);
            this.LBL_Nombre.TabIndex = 1;
            this.LBL_Nombre.Tag = "Catalogos.Nombre";
            this.LBL_Nombre.Text = "Nombre:";
            //
            // TXT_Nombre
            //
            this.TXT_Nombre.Location = new System.Drawing.Point(90, 82);
            this.TXT_Nombre.Name = "TXT_Nombre";
            this.TXT_Nombre.Size = new System.Drawing.Size(280, 22);
            this.TXT_Nombre.TabIndex = 2;
            //
            // BTN_Aceptar
            //
            this.BTN_Aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Aceptar.FlatAppearance.BorderSize = 0;
            this.BTN_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aceptar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aceptar.Location = new System.Drawing.Point(180, 130);
            this.BTN_Aceptar.Name = "BTN_Aceptar";
            this.BTN_Aceptar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Aceptar.TabIndex = 3;
            this.BTN_Aceptar.Tag = "Accion.Aceptar";
            this.BTN_Aceptar.Text = "Aceptar";
            this.BTN_Aceptar.UseVisualStyleBackColor = false;
            //
            // BTN_Cancelar
            //
            this.BTN_Cancelar.BackColor = System.Drawing.Color.Gray;
            this.BTN_Cancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_Cancelar.FlatAppearance.BorderSize = 0;
            this.BTN_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Cancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Cancelar.ForeColor = System.Drawing.Color.White;
            this.BTN_Cancelar.Location = new System.Drawing.Point(285, 130);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Cancelar.TabIndex = 4;
            this.BTN_Cancelar.Tag = "Accion.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            //
            // FrmCatalogoEditar
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.TXT_Nombre);
            this.Controls.Add(this.LBL_Nombre);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCatalogoEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "Catalogos.EditarNombre";
            this.Text = "Catalogo";
            this.Load += new System.EventHandler(this.FrmCatalogoEditar_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_Nombre;
        private System.Windows.Forms.TextBox TXT_Nombre;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
