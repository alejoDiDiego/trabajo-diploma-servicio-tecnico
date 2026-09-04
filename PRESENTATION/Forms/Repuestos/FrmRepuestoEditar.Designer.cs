namespace UI.Forms.Repuestos
{
    partial class FrmRepuestoEditar
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
            this.LBL_Codigo = new System.Windows.Forms.Label();
            this.TXT_Codigo = new System.Windows.Forms.TextBox();
            this.LBL_Descripcion = new System.Windows.Forms.Label();
            this.TXT_Descripcion = new System.Windows.Forms.TextBox();
            this.LBL_StockInicial = new System.Windows.Forms.Label();
            this.NUM_StockInicial = new System.Windows.Forms.NumericUpDown();
            this.LBL_StockMinimo = new System.Windows.Forms.Label();
            this.NUM_StockMinimo = new System.Windows.Forms.NumericUpDown();
            this.LBL_Costo = new System.Windows.Forms.Label();
            this.NUM_Costo = new System.Windows.Forms.NumericUpDown();
            this.LBL_Precio = new System.Windows.Forms.Label();
            this.NUM_Precio = new System.Windows.Forms.NumericUpDown();
            this.BTN_Aceptar = new System.Windows.Forms.Button();
            this.BTN_Cancelar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_StockInicial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_StockMinimo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Costo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Precio)).BeginInit();
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
            this.LBL_Titulo.Tag = "RepuestoEditar.TituloNuevo";
            this.LBL_Titulo.Text = "Nuevo repuesto";
            //
            // LBL_Codigo
            //
            this.LBL_Codigo.AutoSize = true;
            this.LBL_Codigo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Codigo.Location = new System.Drawing.Point(15, 78);
            this.LBL_Codigo.Name = "LBL_Codigo";
            this.LBL_Codigo.Size = new System.Drawing.Size(49, 15);
            this.LBL_Codigo.TabIndex = 1;
            this.LBL_Codigo.Tag = "Repuestos.Codigo";
            this.LBL_Codigo.Text = "Codigo:";
            //
            // TXT_Codigo
            //
            this.TXT_Codigo.Location = new System.Drawing.Point(160, 75);
            this.TXT_Codigo.Name = "TXT_Codigo";
            this.TXT_Codigo.Size = new System.Drawing.Size(280, 22);
            this.TXT_Codigo.TabIndex = 2;
            //
            // LBL_Descripcion
            //
            this.LBL_Descripcion.AutoSize = true;
            this.LBL_Descripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Descripcion.Location = new System.Drawing.Point(15, 116);
            this.LBL_Descripcion.Name = "LBL_Descripcion";
            this.LBL_Descripcion.Size = new System.Drawing.Size(72, 15);
            this.LBL_Descripcion.TabIndex = 3;
            this.LBL_Descripcion.Tag = "Repuestos.Descripcion";
            this.LBL_Descripcion.Text = "Descripcion:";
            //
            // TXT_Descripcion
            //
            this.TXT_Descripcion.Location = new System.Drawing.Point(160, 113);
            this.TXT_Descripcion.Name = "TXT_Descripcion";
            this.TXT_Descripcion.Size = new System.Drawing.Size(280, 22);
            this.TXT_Descripcion.TabIndex = 4;
            //
            // LBL_StockInicial
            //
            this.LBL_StockInicial.AutoSize = true;
            this.LBL_StockInicial.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_StockInicial.Location = new System.Drawing.Point(15, 154);
            this.LBL_StockInicial.Name = "LBL_StockInicial";
            this.LBL_StockInicial.Size = new System.Drawing.Size(72, 15);
            this.LBL_StockInicial.TabIndex = 5;
            this.LBL_StockInicial.Tag = "Repuestos.StockInicial";
            this.LBL_StockInicial.Text = "Stock inicial:";
            //
            // NUM_StockInicial
            //
            this.NUM_StockInicial.Location = new System.Drawing.Point(160, 151);
            this.NUM_StockInicial.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUM_StockInicial.Name = "NUM_StockInicial";
            this.NUM_StockInicial.Size = new System.Drawing.Size(120, 22);
            this.NUM_StockInicial.TabIndex = 6;
            //
            // LBL_StockMinimo
            //
            this.LBL_StockMinimo.AutoSize = true;
            this.LBL_StockMinimo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_StockMinimo.Location = new System.Drawing.Point(15, 192);
            this.LBL_StockMinimo.Name = "LBL_StockMinimo";
            this.LBL_StockMinimo.Size = new System.Drawing.Size(79, 15);
            this.LBL_StockMinimo.TabIndex = 7;
            this.LBL_StockMinimo.Tag = "Repuestos.StockMinimo";
            this.LBL_StockMinimo.Text = "Stock minimo:";
            //
            // NUM_StockMinimo
            //
            this.NUM_StockMinimo.Location = new System.Drawing.Point(160, 189);
            this.NUM_StockMinimo.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUM_StockMinimo.Name = "NUM_StockMinimo";
            this.NUM_StockMinimo.Size = new System.Drawing.Size(120, 22);
            this.NUM_StockMinimo.TabIndex = 8;
            //
            // LBL_Costo
            //
            this.LBL_Costo.AutoSize = true;
            this.LBL_Costo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Costo.Location = new System.Drawing.Point(15, 230);
            this.LBL_Costo.Name = "LBL_Costo";
            this.LBL_Costo.Size = new System.Drawing.Size(42, 15);
            this.LBL_Costo.TabIndex = 9;
            this.LBL_Costo.Tag = "Repuestos.Costo";
            this.LBL_Costo.Text = "Costo:";
            //
            // NUM_Costo
            //
            this.NUM_Costo.DecimalPlaces = 2;
            this.NUM_Costo.Location = new System.Drawing.Point(160, 227);
            this.NUM_Costo.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUM_Costo.Name = "NUM_Costo";
            this.NUM_Costo.Size = new System.Drawing.Size(120, 22);
            this.NUM_Costo.TabIndex = 10;
            //
            // LBL_Precio
            //
            this.LBL_Precio.AutoSize = true;
            this.LBL_Precio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Precio.Location = new System.Drawing.Point(15, 268);
            this.LBL_Precio.Name = "LBL_Precio";
            this.LBL_Precio.Size = new System.Drawing.Size(105, 15);
            this.LBL_Precio.TabIndex = 11;
            this.LBL_Precio.Tag = "Repuestos.Precio";
            this.LBL_Precio.Text = "Precio referencia:";
            //
            // NUM_Precio
            //
            this.NUM_Precio.DecimalPlaces = 2;
            this.NUM_Precio.Location = new System.Drawing.Point(160, 265);
            this.NUM_Precio.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUM_Precio.Name = "NUM_Precio";
            this.NUM_Precio.Size = new System.Drawing.Size(120, 22);
            this.NUM_Precio.TabIndex = 12;
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
            // FrmRepuestoEditar
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(470, 355);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.NUM_Precio);
            this.Controls.Add(this.LBL_Precio);
            this.Controls.Add(this.NUM_Costo);
            this.Controls.Add(this.LBL_Costo);
            this.Controls.Add(this.NUM_StockMinimo);
            this.Controls.Add(this.LBL_StockMinimo);
            this.Controls.Add(this.NUM_StockInicial);
            this.Controls.Add(this.LBL_StockInicial);
            this.Controls.Add(this.TXT_Descripcion);
            this.Controls.Add(this.LBL_Descripcion);
            this.Controls.Add(this.TXT_Codigo);
            this.Controls.Add(this.LBL_Codigo);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRepuestoEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "RepuestoEditar.TituloNuevo";
            this.Text = "Nuevo repuesto";
            this.Load += new System.EventHandler(this.FrmRepuestoEditar_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_StockInicial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_StockMinimo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Costo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Precio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_Codigo;
        private System.Windows.Forms.TextBox TXT_Codigo;
        private System.Windows.Forms.Label LBL_Descripcion;
        private System.Windows.Forms.TextBox TXT_Descripcion;
        private System.Windows.Forms.Label LBL_StockInicial;
        private System.Windows.Forms.NumericUpDown NUM_StockInicial;
        private System.Windows.Forms.Label LBL_StockMinimo;
        private System.Windows.Forms.NumericUpDown NUM_StockMinimo;
        private System.Windows.Forms.Label LBL_Costo;
        private System.Windows.Forms.NumericUpDown NUM_Costo;
        private System.Windows.Forms.Label LBL_Precio;
        private System.Windows.Forms.NumericUpDown NUM_Precio;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
