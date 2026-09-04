namespace UI.Forms.Repuestos
{
    partial class FrmAjusteStock
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
            this.LBL_StockActual = new System.Windows.Forms.Label();
            this.LBL_Cantidad = new System.Windows.Forms.Label();
            this.NUM_Cantidad = new System.Windows.Forms.NumericUpDown();
            this.LBL_Motivo = new System.Windows.Forms.Label();
            this.TXT_Motivo = new System.Windows.Forms.TextBox();
            this.BTN_Aceptar = new System.Windows.Forms.Button();
            this.BTN_Cancelar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Cantidad)).BeginInit();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(420, 60);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Titulo
            //
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(130, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "AjusteStock.Titulo";
            this.LBL_Titulo.Text = "Ajustar stock";
            //
            // LBL_StockActual
            //
            this.LBL_StockActual.AutoSize = true;
            this.LBL_StockActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_StockActual.Location = new System.Drawing.Point(15, 78);
            this.LBL_StockActual.Name = "LBL_StockActual";
            this.LBL_StockActual.Size = new System.Drawing.Size(90, 15);
            this.LBL_StockActual.TabIndex = 1;
            this.LBL_StockActual.Tag = "AjusteStock.StockActual";
            this.LBL_StockActual.Text = "Stock actual: 0";
            //
            // LBL_Cantidad
            //
            this.LBL_Cantidad.AutoSize = true;
            this.LBL_Cantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cantidad.Location = new System.Drawing.Point(15, 112);
            this.LBL_Cantidad.Name = "LBL_Cantidad";
            this.LBL_Cantidad.Size = new System.Drawing.Size(84, 15);
            this.LBL_Cantidad.TabIndex = 2;
            this.LBL_Cantidad.Tag = "AjusteStock.Cantidad";
            this.LBL_Cantidad.Text = "Cantidad (+/-):";
            //
            // NUM_Cantidad
            //
            this.NUM_Cantidad.Location = new System.Drawing.Point(140, 109);
            this.NUM_Cantidad.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUM_Cantidad.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.NUM_Cantidad.Name = "NUM_Cantidad";
            this.NUM_Cantidad.Size = new System.Drawing.Size(120, 22);
            this.NUM_Cantidad.TabIndex = 3;
            //
            // LBL_Motivo
            //
            this.LBL_Motivo.AutoSize = true;
            this.LBL_Motivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Motivo.Location = new System.Drawing.Point(15, 148);
            this.LBL_Motivo.Name = "LBL_Motivo";
            this.LBL_Motivo.Size = new System.Drawing.Size(51, 15);
            this.LBL_Motivo.TabIndex = 4;
            this.LBL_Motivo.Tag = "AjusteStock.Motivo";
            this.LBL_Motivo.Text = "Motivo:";
            //
            // TXT_Motivo
            //
            this.TXT_Motivo.Location = new System.Drawing.Point(140, 145);
            this.TXT_Motivo.Multiline = true;
            this.TXT_Motivo.Name = "TXT_Motivo";
            this.TXT_Motivo.Size = new System.Drawing.Size(255, 60);
            this.TXT_Motivo.TabIndex = 5;
            //
            // BTN_Aceptar
            //
            this.BTN_Aceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Aceptar.FlatAppearance.BorderSize = 0;
            this.BTN_Aceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aceptar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aceptar.Location = new System.Drawing.Point(210, 222);
            this.BTN_Aceptar.Name = "BTN_Aceptar";
            this.BTN_Aceptar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Aceptar.TabIndex = 6;
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
            this.BTN_Cancelar.Location = new System.Drawing.Point(315, 222);
            this.BTN_Cancelar.Name = "BTN_Cancelar";
            this.BTN_Cancelar.Size = new System.Drawing.Size(95, 30);
            this.BTN_Cancelar.TabIndex = 7;
            this.BTN_Cancelar.Tag = "Accion.Cancelar";
            this.BTN_Cancelar.Text = "Cancelar";
            this.BTN_Cancelar.UseVisualStyleBackColor = false;
            //
            // FrmAjusteStock
            //
            this.AcceptButton = this.BTN_Aceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cancelar;
            this.ClientSize = new System.Drawing.Size(420, 267);
            this.Controls.Add(this.BTN_Cancelar);
            this.Controls.Add(this.BTN_Aceptar);
            this.Controls.Add(this.TXT_Motivo);
            this.Controls.Add(this.LBL_Motivo);
            this.Controls.Add(this.NUM_Cantidad);
            this.Controls.Add(this.LBL_Cantidad);
            this.Controls.Add(this.LBL_StockActual);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAjusteStock";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "FrmAjusteStock.Text";
            this.Text = "Ajustar stock";
            this.Load += new System.EventHandler(this.FrmAjusteStock_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Cantidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Label LBL_StockActual;
        private System.Windows.Forms.Label LBL_Cantidad;
        private System.Windows.Forms.NumericUpDown NUM_Cantidad;
        private System.Windows.Forms.Label LBL_Motivo;
        private System.Windows.Forms.TextBox TXT_Motivo;
        private System.Windows.Forms.Button BTN_Aceptar;
        private System.Windows.Forms.Button BTN_Cancelar;
    }
}
