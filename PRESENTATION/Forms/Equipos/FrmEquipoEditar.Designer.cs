namespace UI.Forms.Equipos
{
    partial class FrmEquipoEditar
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
            this.LBL_Cliente = new System.Windows.Forms.Label();
            this.CBO_Cliente = new System.Windows.Forms.ComboBox();
            this.LBL_Tipo = new System.Windows.Forms.Label();
            this.CBO_Tipo = new System.Windows.Forms.ComboBox();
            this.LBL_Marca = new System.Windows.Forms.Label();
            this.CBO_Marca = new System.Windows.Forms.ComboBox();
            this.LBL_Modelo = new System.Windows.Forms.Label();
            this.TXT_Modelo = new System.Windows.Forms.TextBox();
            this.LBL_NumeroSerie = new System.Windows.Forms.Label();
            this.TXT_NumeroSerie = new System.Windows.Forms.TextBox();
            this.LBL_Imei = new System.Windows.Forms.Label();
            this.TXT_Imei = new System.Windows.Forms.TextBox();
            this.LBL_Color = new System.Windows.Forms.Label();
            this.TXT_Color = new System.Windows.Forms.TextBox();
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
            this.LBL_Titulo.Tag = "EquipoEditar.TituloNuevo";
            this.LBL_Titulo.Text = "Nuevo equipo";
            //
            // LBL_Cliente
            //
            this.LBL_Cliente.AutoSize = true;
            this.LBL_Cliente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cliente.Location = new System.Drawing.Point(15, 78);
            this.LBL_Cliente.Name = "LBL_Cliente";
            this.LBL_Cliente.Size = new System.Drawing.Size(47, 15);
            this.LBL_Cliente.TabIndex = 1;
            this.LBL_Cliente.Tag = "Campo.Cliente";
            this.LBL_Cliente.Text = "Cliente:";
            //
            // CBO_Cliente
            //
            this.CBO_Cliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Cliente.Location = new System.Drawing.Point(140, 75);
            this.CBO_Cliente.Name = "CBO_Cliente";
            this.CBO_Cliente.Size = new System.Drawing.Size(300, 21);
            this.CBO_Cliente.TabIndex = 2;
            //
            // LBL_Tipo
            //
            this.LBL_Tipo.AutoSize = true;
            this.LBL_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Tipo.Location = new System.Drawing.Point(15, 116);
            this.LBL_Tipo.Name = "LBL_Tipo";
            this.LBL_Tipo.Size = new System.Drawing.Size(33, 15);
            this.LBL_Tipo.TabIndex = 3;
            this.LBL_Tipo.Tag = "Campo.TipoEquipo";
            this.LBL_Tipo.Text = "Tipo:";
            //
            // CBO_Tipo
            //
            this.CBO_Tipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Tipo.Location = new System.Drawing.Point(140, 113);
            this.CBO_Tipo.Name = "CBO_Tipo";
            this.CBO_Tipo.Size = new System.Drawing.Size(300, 21);
            this.CBO_Tipo.TabIndex = 4;
            //
            // LBL_Marca
            //
            this.LBL_Marca.AutoSize = true;
            this.LBL_Marca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Marca.Location = new System.Drawing.Point(15, 154);
            this.LBL_Marca.Name = "LBL_Marca";
            this.LBL_Marca.Size = new System.Drawing.Size(43, 15);
            this.LBL_Marca.TabIndex = 5;
            this.LBL_Marca.Tag = "Campo.Marca";
            this.LBL_Marca.Text = "Marca:";
            //
            // CBO_Marca
            //
            this.CBO_Marca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Marca.Location = new System.Drawing.Point(140, 151);
            this.CBO_Marca.Name = "CBO_Marca";
            this.CBO_Marca.Size = new System.Drawing.Size(300, 21);
            this.CBO_Marca.TabIndex = 6;
            //
            // LBL_Modelo
            //
            this.LBL_Modelo.AutoSize = true;
            this.LBL_Modelo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Modelo.Location = new System.Drawing.Point(15, 192);
            this.LBL_Modelo.Name = "LBL_Modelo";
            this.LBL_Modelo.Size = new System.Drawing.Size(51, 15);
            this.LBL_Modelo.TabIndex = 7;
            this.LBL_Modelo.Tag = "Campo.Modelo";
            this.LBL_Modelo.Text = "Modelo:";
            //
            // TXT_Modelo
            //
            this.TXT_Modelo.Location = new System.Drawing.Point(140, 189);
            this.TXT_Modelo.Name = "TXT_Modelo";
            this.TXT_Modelo.Size = new System.Drawing.Size(300, 22);
            this.TXT_Modelo.TabIndex = 8;
            //
            // LBL_NumeroSerie
            //
            this.LBL_NumeroSerie.AutoSize = true;
            this.LBL_NumeroSerie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_NumeroSerie.Location = new System.Drawing.Point(15, 230);
            this.LBL_NumeroSerie.Name = "LBL_NumeroSerie";
            this.LBL_NumeroSerie.Size = new System.Drawing.Size(94, 15);
            this.LBL_NumeroSerie.TabIndex = 9;
            this.LBL_NumeroSerie.Tag = "Campo.NumeroSerie";
            this.LBL_NumeroSerie.Text = "Numero de serie:";
            //
            // TXT_NumeroSerie
            //
            this.TXT_NumeroSerie.Location = new System.Drawing.Point(140, 227);
            this.TXT_NumeroSerie.Name = "TXT_NumeroSerie";
            this.TXT_NumeroSerie.Size = new System.Drawing.Size(300, 22);
            this.TXT_NumeroSerie.TabIndex = 10;
            //
            // LBL_Imei
            //
            this.LBL_Imei.AutoSize = true;
            this.LBL_Imei.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Imei.Location = new System.Drawing.Point(15, 268);
            this.LBL_Imei.Name = "LBL_Imei";
            this.LBL_Imei.Size = new System.Drawing.Size(36, 15);
            this.LBL_Imei.TabIndex = 11;
            this.LBL_Imei.Tag = "Campo.Imei";
            this.LBL_Imei.Text = "Imei:";
            //
            // TXT_Imei
            //
            this.TXT_Imei.Location = new System.Drawing.Point(140, 265);
            this.TXT_Imei.Name = "TXT_Imei";
            this.TXT_Imei.Size = new System.Drawing.Size(300, 22);
            this.TXT_Imei.TabIndex = 12;
            //
            // LBL_Color
            //
            this.LBL_Color.AutoSize = true;
            this.LBL_Color.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Color.Location = new System.Drawing.Point(15, 306);
            this.LBL_Color.Name = "LBL_Color";
            this.LBL_Color.Size = new System.Drawing.Size(39, 15);
            this.LBL_Color.TabIndex = 13;
            this.LBL_Color.Tag = "Campo.Color";
            this.LBL_Color.Text = "Color:";
            //
            // TXT_Color
            //
            this.TXT_Color.Location = new System.Drawing.Point(140, 303);
            this.TXT_Color.Name = "TXT_Color";
            this.TXT_Color.Size = new System.Drawing.Size(300, 22);
            this.TXT_Color.TabIndex = 14;
            //
            // LBL_Observaciones
            //
            this.LBL_Observaciones.AutoSize = true;
            this.LBL_Observaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Observaciones.Location = new System.Drawing.Point(15, 344);
            this.LBL_Observaciones.Name = "LBL_Observaciones";
            this.LBL_Observaciones.Size = new System.Drawing.Size(90, 15);
            this.LBL_Observaciones.TabIndex = 15;
            this.LBL_Observaciones.Tag = "Campo.Observaciones";
            this.LBL_Observaciones.Text = "Observaciones:";
            //
            // TXT_Observaciones
            //
            this.TXT_Observaciones.Location = new System.Drawing.Point(140, 341);
            this.TXT_Observaciones.Multiline = true;
            this.TXT_Observaciones.Name = "TXT_Observaciones";
            this.TXT_Observaciones.Size = new System.Drawing.Size(300, 60);
            this.TXT_Observaciones.TabIndex = 16;
            //
            // BTN_Aceptar
            //
            this.BTN_Aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Aceptar.FlatAppearance.BorderSize = 0;
            this.BTN_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aceptar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aceptar.Location = new System.Drawing.Point(260, 421);
            this.BTN_Aceptar.Name = "BTN_Aceptar";
            this.BTN_Aceptar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Aceptar.TabIndex = 17;
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
            this.BTN_Cancelar.Location = new System.Drawing.Point(365, 421);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Cancelar.TabIndex = 18;
            this.BTN_Cancelar.Tag = "Accion.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            //
            // FrmEquipoEditar
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(470, 466);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.TXT_Observaciones);
            this.Controls.Add(this.LBL_Observaciones);
            this.Controls.Add(this.TXT_Color);
            this.Controls.Add(this.LBL_Color);
            this.Controls.Add(this.TXT_Imei);
            this.Controls.Add(this.LBL_Imei);
            this.Controls.Add(this.TXT_NumeroSerie);
            this.Controls.Add(this.LBL_NumeroSerie);
            this.Controls.Add(this.TXT_Modelo);
            this.Controls.Add(this.LBL_Modelo);
            this.Controls.Add(this.CBO_Marca);
            this.Controls.Add(this.LBL_Marca);
            this.Controls.Add(this.CBO_Tipo);
            this.Controls.Add(this.LBL_Tipo);
            this.Controls.Add(this.CBO_Cliente);
            this.Controls.Add(this.LBL_Cliente);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEquipoEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "EquipoEditar.TituloNuevo";
            this.Text = "Nuevo equipo";
            this.Load += new System.EventHandler(this.FrmEquipoEditar_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_Cliente;
        private System.Windows.Forms.ComboBox CBO_Cliente;
        private System.Windows.Forms.Label LBL_Tipo;
        private System.Windows.Forms.ComboBox CBO_Tipo;
        private System.Windows.Forms.Label LBL_Marca;
        private System.Windows.Forms.ComboBox CBO_Marca;
        private System.Windows.Forms.Label LBL_Modelo;
        private System.Windows.Forms.TextBox TXT_Modelo;
        private System.Windows.Forms.Label LBL_NumeroSerie;
        private System.Windows.Forms.TextBox TXT_NumeroSerie;
        private System.Windows.Forms.Label LBL_Imei;
        private System.Windows.Forms.TextBox TXT_Imei;
        private System.Windows.Forms.Label LBL_Color;
        private System.Windows.Forms.TextBox TXT_Color;
        private System.Windows.Forms.Label LBL_Observaciones;
        private System.Windows.Forms.TextBox TXT_Observaciones;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
