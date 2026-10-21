namespace UI.Forms.Proveedores
{
    partial class FrmProveedorEditar
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
            this.LBL_RazonSocial = new System.Windows.Forms.Label();
            this.TXT_RazonSocial = new System.Windows.Forms.TextBox();
            this.LBL_Cuit = new System.Windows.Forms.Label();
            this.TXT_Cuit = new System.Windows.Forms.TextBox();
            this.LBL_Telefono = new System.Windows.Forms.Label();
            this.TXT_Telefono = new System.Windows.Forms.TextBox();
            this.LBL_Email = new System.Windows.Forms.Label();
            this.TXT_Email = new System.Windows.Forms.TextBox();
            this.LBL_Direccion = new System.Windows.Forms.Label();
            this.TXT_Direccion = new System.Windows.Forms.TextBox();
            this.LBL_Contacto = new System.Windows.Forms.Label();
            this.TXT_Contacto = new System.Windows.Forms.TextBox();
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
            this.PNL_Header.Size = new System.Drawing.Size(470, 60);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Titulo
            //
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(150, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "ProveedorEditar.TituloNuevo";
            this.LBL_Titulo.Text = "Nuevo proveedor";
            //
            // LBL_RazonSocial
            //
            this.LBL_RazonSocial.AutoSize = true;
            this.LBL_RazonSocial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RazonSocial.Location = new System.Drawing.Point(15, 78);
            this.LBL_RazonSocial.Name = "LBL_RazonSocial";
            this.LBL_RazonSocial.Size = new System.Drawing.Size(80, 15);
            this.LBL_RazonSocial.TabIndex = 1;
            this.LBL_RazonSocial.Tag = "Campo.RazonSocial";
            this.LBL_RazonSocial.Text = "Razon social:";
            //
            // TXT_RazonSocial
            //
            this.TXT_RazonSocial.Location = new System.Drawing.Point(140, 75);
            this.TXT_RazonSocial.Name = "TXT_RazonSocial";
            this.TXT_RazonSocial.Size = new System.Drawing.Size(300, 22);
            this.TXT_RazonSocial.TabIndex = 2;
            //
            // LBL_Cuit
            //
            this.LBL_Cuit.AutoSize = true;
            this.LBL_Cuit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cuit.Location = new System.Drawing.Point(15, 116);
            this.LBL_Cuit.Name = "LBL_Cuit";
            this.LBL_Cuit.Size = new System.Drawing.Size(40, 15);
            this.LBL_Cuit.TabIndex = 3;
            this.LBL_Cuit.Tag = "Campo.Cuit";
            this.LBL_Cuit.Text = "Cuit:";
            //
            // TXT_Cuit
            //
            this.TXT_Cuit.Location = new System.Drawing.Point(140, 113);
            this.TXT_Cuit.Name = "TXT_Cuit";
            this.TXT_Cuit.Size = new System.Drawing.Size(300, 22);
            this.TXT_Cuit.TabIndex = 4;
            //
            // LBL_Telefono
            //
            this.LBL_Telefono.AutoSize = true;
            this.LBL_Telefono.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Telefono.Location = new System.Drawing.Point(15, 154);
            this.LBL_Telefono.Name = "LBL_Telefono";
            this.LBL_Telefono.Size = new System.Drawing.Size(57, 15);
            this.LBL_Telefono.TabIndex = 5;
            this.LBL_Telefono.Tag = "Campo.Telefono";
            this.LBL_Telefono.Text = "Telefono:";
            //
            // TXT_Telefono
            //
            this.TXT_Telefono.Location = new System.Drawing.Point(140, 151);
            this.TXT_Telefono.Name = "TXT_Telefono";
            this.TXT_Telefono.Size = new System.Drawing.Size(300, 22);
            this.TXT_Telefono.TabIndex = 6;
            //
            // LBL_Email
            //
            this.LBL_Email.AutoSize = true;
            this.LBL_Email.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Email.Location = new System.Drawing.Point(15, 192);
            this.LBL_Email.Name = "LBL_Email";
            this.LBL_Email.Size = new System.Drawing.Size(39, 15);
            this.LBL_Email.TabIndex = 7;
            this.LBL_Email.Tag = "Campo.Email";
            this.LBL_Email.Text = "Email:";
            //
            // TXT_Email
            //
            this.TXT_Email.Location = new System.Drawing.Point(140, 189);
            this.TXT_Email.Name = "TXT_Email";
            this.TXT_Email.Size = new System.Drawing.Size(300, 22);
            this.TXT_Email.TabIndex = 8;
            //
            // LBL_Direccion
            //
            this.LBL_Direccion.AutoSize = true;
            this.LBL_Direccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Direccion.Location = new System.Drawing.Point(15, 230);
            this.LBL_Direccion.Name = "LBL_Direccion";
            this.LBL_Direccion.Size = new System.Drawing.Size(60, 15);
            this.LBL_Direccion.TabIndex = 9;
            this.LBL_Direccion.Tag = "Campo.Direccion";
            this.LBL_Direccion.Text = "Direccion:";
            //
            // TXT_Direccion
            //
            this.TXT_Direccion.Location = new System.Drawing.Point(140, 227);
            this.TXT_Direccion.Name = "TXT_Direccion";
            this.TXT_Direccion.Size = new System.Drawing.Size(300, 22);
            this.TXT_Direccion.TabIndex = 10;
            //
            // LBL_Contacto
            //
            this.LBL_Contacto.AutoSize = true;
            this.LBL_Contacto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Contacto.Location = new System.Drawing.Point(15, 268);
            this.LBL_Contacto.Name = "LBL_Contacto";
            this.LBL_Contacto.Size = new System.Drawing.Size(62, 15);
            this.LBL_Contacto.TabIndex = 11;
            this.LBL_Contacto.Tag = "Campo.Contacto";
            this.LBL_Contacto.Text = "Contacto:";
            //
            // TXT_Contacto
            //
            this.TXT_Contacto.Location = new System.Drawing.Point(140, 265);
            this.TXT_Contacto.Name = "TXT_Contacto";
            this.TXT_Contacto.Size = new System.Drawing.Size(300, 22);
            this.TXT_Contacto.TabIndex = 12;
            //
            // BTN_Aceptar
            //
            this.BTN_Aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Aceptar.FlatAppearance.BorderSize = 0;
            this.BTN_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aceptar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aceptar.Location = new System.Drawing.Point(260, 310);
            this.BTN_Aceptar.Name = "BTN_Aceptar";
            this.BTN_Aceptar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Aceptar.TabIndex = 13;
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
            this.BTN_Cancelar.Location = new System.Drawing.Point(365, 310);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Cancelar.TabIndex = 14;
            this.BTN_Cancelar.Tag = "Accion.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            //
            // FrmProveedorEditar
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(470, 355);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.TXT_Contacto);
            this.Controls.Add(this.LBL_Contacto);
            this.Controls.Add(this.TXT_Direccion);
            this.Controls.Add(this.LBL_Direccion);
            this.Controls.Add(this.TXT_Email);
            this.Controls.Add(this.LBL_Email);
            this.Controls.Add(this.TXT_Telefono);
            this.Controls.Add(this.LBL_Telefono);
            this.Controls.Add(this.TXT_Cuit);
            this.Controls.Add(this.LBL_Cuit);
            this.Controls.Add(this.TXT_RazonSocial);
            this.Controls.Add(this.LBL_RazonSocial);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProveedorEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "ProveedorEditar.TituloNuevo";
            this.Text = "Nuevo proveedor";
            this.Load += new System.EventHandler(this.FrmProveedorEditar_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_RazonSocial;
        private System.Windows.Forms.TextBox TXT_RazonSocial;
        private System.Windows.Forms.Label LBL_Cuit;
        private System.Windows.Forms.TextBox TXT_Cuit;
        private System.Windows.Forms.Label LBL_Telefono;
        private System.Windows.Forms.TextBox TXT_Telefono;
        private System.Windows.Forms.Label LBL_Email;
        private System.Windows.Forms.TextBox TXT_Email;
        private System.Windows.Forms.Label LBL_Direccion;
        private System.Windows.Forms.TextBox TXT_Direccion;
        private System.Windows.Forms.Label LBL_Contacto;
        private System.Windows.Forms.TextBox TXT_Contacto;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
