namespace UI.Forms.Clientes
{
    partial class FrmClienteEditar
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
            this.LBL_Apellido = new System.Windows.Forms.Label();
            this.TXT_Apellido = new System.Windows.Forms.TextBox();
            this.LBL_Documento = new System.Windows.Forms.Label();
            this.TXT_Documento = new System.Windows.Forms.TextBox();
            this.LBL_Telefono = new System.Windows.Forms.Label();
            this.TXT_Telefono = new System.Windows.Forms.TextBox();
            this.LBL_Email = new System.Windows.Forms.Label();
            this.TXT_Email = new System.Windows.Forms.TextBox();
            this.LBL_Direccion = new System.Windows.Forms.Label();
            this.TXT_Direccion = new System.Windows.Forms.TextBox();
            this.LBL_Observaciones = new System.Windows.Forms.Label();
            this.TXT_Observaciones = new System.Windows.Forms.TextBox();
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
            this.LBL_Titulo.Tag = "ClienteEditar.TituloNuevo";
            this.LBL_Titulo.Text = "Nuevo cliente";
            //
            // LBL_Nombre
            //
            this.LBL_Nombre.AutoSize = true;
            this.LBL_Nombre.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Nombre.Location = new System.Drawing.Point(15, 78);
            this.LBL_Nombre.Name = "LBL_Nombre";
            this.LBL_Nombre.Size = new System.Drawing.Size(54, 15);
            this.LBL_Nombre.TabIndex = 1;
            this.LBL_Nombre.Tag = "Campo.Nombre";
            this.LBL_Nombre.Text = "Nombre:";
            //
            // TXT_Nombre
            //
            this.TXT_Nombre.Location = new System.Drawing.Point(140, 75);
            this.TXT_Nombre.Name = "TXT_Nombre";
            this.TXT_Nombre.Size = new System.Drawing.Size(300, 22);
            this.TXT_Nombre.TabIndex = 2;
            //
            // LBL_Apellido
            //
            this.LBL_Apellido.AutoSize = true;
            this.LBL_Apellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Apellido.Location = new System.Drawing.Point(15, 123);
            this.LBL_Apellido.Name = "LBL_Apellido";
            this.LBL_Apellido.Size = new System.Drawing.Size(54, 15);
            this.LBL_Apellido.TabIndex = 3;
            this.LBL_Apellido.Tag = "Campo.Apellido";
            this.LBL_Apellido.Text = "Apellido:";
            //
            // TXT_Apellido
            //
            this.TXT_Apellido.Location = new System.Drawing.Point(140, 120);
            this.TXT_Apellido.Name = "TXT_Apellido";
            this.TXT_Apellido.Size = new System.Drawing.Size(300, 22);
            this.TXT_Apellido.TabIndex = 4;
            //
            // LBL_Documento
            //
            this.LBL_Documento.AutoSize = true;
            this.LBL_Documento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Documento.Location = new System.Drawing.Point(15, 168);
            this.LBL_Documento.Name = "LBL_Documento";
            this.LBL_Documento.Size = new System.Drawing.Size(74, 15);
            this.LBL_Documento.TabIndex = 5;
            this.LBL_Documento.Tag = "Campo.Documento";
            this.LBL_Documento.Text = "Documento:";
            //
            // TXT_Documento
            //
            this.TXT_Documento.Location = new System.Drawing.Point(140, 165);
            this.TXT_Documento.Name = "TXT_Documento";
            this.TXT_Documento.Size = new System.Drawing.Size(300, 22);
            this.TXT_Documento.TabIndex = 6;
            //
            // LBL_Telefono
            //
            this.LBL_Telefono.AutoSize = true;
            this.LBL_Telefono.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Telefono.Location = new System.Drawing.Point(15, 213);
            this.LBL_Telefono.Name = "LBL_Telefono";
            this.LBL_Telefono.Size = new System.Drawing.Size(57, 15);
            this.LBL_Telefono.TabIndex = 7;
            this.LBL_Telefono.Tag = "Campo.Telefono";
            this.LBL_Telefono.Text = "Telefono:";
            //
            // TXT_Telefono
            //
            this.TXT_Telefono.Location = new System.Drawing.Point(140, 210);
            this.TXT_Telefono.Name = "TXT_Telefono";
            this.TXT_Telefono.Size = new System.Drawing.Size(300, 22);
            this.TXT_Telefono.TabIndex = 8;
            //
            // LBL_Email
            //
            this.LBL_Email.AutoSize = true;
            this.LBL_Email.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Email.Location = new System.Drawing.Point(15, 258);
            this.LBL_Email.Name = "LBL_Email";
            this.LBL_Email.Size = new System.Drawing.Size(39, 15);
            this.LBL_Email.TabIndex = 9;
            this.LBL_Email.Tag = "Campo.Email";
            this.LBL_Email.Text = "Email:";
            //
            // TXT_Email
            //
            this.TXT_Email.Location = new System.Drawing.Point(140, 255);
            this.TXT_Email.Name = "TXT_Email";
            this.TXT_Email.Size = new System.Drawing.Size(300, 22);
            this.TXT_Email.TabIndex = 10;
            //
            // LBL_Direccion
            //
            this.LBL_Direccion.AutoSize = true;
            this.LBL_Direccion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Direccion.Location = new System.Drawing.Point(15, 303);
            this.LBL_Direccion.Name = "LBL_Direccion";
            this.LBL_Direccion.Size = new System.Drawing.Size(60, 15);
            this.LBL_Direccion.TabIndex = 11;
            this.LBL_Direccion.Tag = "Campo.Direccion";
            this.LBL_Direccion.Text = "Direccion:";
            //
            // TXT_Direccion
            //
            this.TXT_Direccion.Location = new System.Drawing.Point(140, 300);
            this.TXT_Direccion.Name = "TXT_Direccion";
            this.TXT_Direccion.Size = new System.Drawing.Size(300, 22);
            this.TXT_Direccion.TabIndex = 12;
            //
            // LBL_Observaciones
            //
            this.LBL_Observaciones.AutoSize = true;
            this.LBL_Observaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Observaciones.Location = new System.Drawing.Point(15, 348);
            this.LBL_Observaciones.Name = "LBL_Observaciones";
            this.LBL_Observaciones.Size = new System.Drawing.Size(90, 15);
            this.LBL_Observaciones.TabIndex = 13;
            this.LBL_Observaciones.Tag = "Campo.Observaciones";
            this.LBL_Observaciones.Text = "Observaciones:";
            //
            // TXT_Observaciones
            //
            this.TXT_Observaciones.Location = new System.Drawing.Point(140, 345);
            this.TXT_Observaciones.Multiline = true;
            this.TXT_Observaciones.Name = "TXT_Observaciones";
            this.TXT_Observaciones.Size = new System.Drawing.Size(300, 60);
            this.TXT_Observaciones.TabIndex = 14;
            //
            // BTN_Aceptar
            //
            this.BTN_Aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Aceptar.FlatAppearance.BorderSize = 0;
            this.BTN_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aceptar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aceptar.Location = new System.Drawing.Point(260, 425);
            this.BTN_Aceptar.Name = "BTN_Aceptar";
            this.BTN_Aceptar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Aceptar.TabIndex = 15;
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
            this.BTN_Cancelar.Location = new System.Drawing.Point(365, 425);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Cancelar.TabIndex = 16;
            this.BTN_Cancelar.Tag = "Accion.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            //
            // FrmClienteEditar
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(470, 470);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.TXT_Observaciones);
            this.Controls.Add(this.LBL_Observaciones);
            this.Controls.Add(this.TXT_Direccion);
            this.Controls.Add(this.LBL_Direccion);
            this.Controls.Add(this.TXT_Email);
            this.Controls.Add(this.LBL_Email);
            this.Controls.Add(this.TXT_Telefono);
            this.Controls.Add(this.LBL_Telefono);
            this.Controls.Add(this.TXT_Documento);
            this.Controls.Add(this.LBL_Documento);
            this.Controls.Add(this.TXT_Apellido);
            this.Controls.Add(this.LBL_Apellido);
            this.Controls.Add(this.TXT_Nombre);
            this.Controls.Add(this.LBL_Nombre);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmClienteEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "ClienteEditar.TituloNuevo";
            this.Text = "Nuevo cliente";
            this.Load += new System.EventHandler(this.FrmClienteEditar_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_Nombre;
        private System.Windows.Forms.TextBox TXT_Nombre;
        private System.Windows.Forms.Label LBL_Apellido;
        private System.Windows.Forms.TextBox TXT_Apellido;
        private System.Windows.Forms.Label LBL_Documento;
        private System.Windows.Forms.TextBox TXT_Documento;
        private System.Windows.Forms.Label LBL_Telefono;
        private System.Windows.Forms.TextBox TXT_Telefono;
        private System.Windows.Forms.Label LBL_Email;
        private System.Windows.Forms.TextBox TXT_Email;
        private System.Windows.Forms.Label LBL_Direccion;
        private System.Windows.Forms.TextBox TXT_Direccion;
        private System.Windows.Forms.Label LBL_Observaciones;
        private System.Windows.Forms.TextBox TXT_Observaciones;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
