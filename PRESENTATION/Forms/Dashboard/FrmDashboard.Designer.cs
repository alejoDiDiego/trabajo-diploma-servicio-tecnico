namespace UI.Forms.Dashboard
{
    partial class FrmDashboard
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
            this.PNL_CardAbiertas = new System.Windows.Forms.Panel();
            this.LBL_AbiertasTitulo = new System.Windows.Forms.Label();
            this.LBL_AbiertasValor = new System.Windows.Forms.Label();
            this.PNL_CardEsperando = new System.Windows.Forms.Panel();
            this.LBL_EsperandoTitulo = new System.Windows.Forms.Label();
            this.LBL_EsperandoValor = new System.Windows.Forms.Label();
            this.PNL_CardReparacion = new System.Windows.Forms.Panel();
            this.LBL_ReparacionTitulo = new System.Windows.Forms.Label();
            this.LBL_ReparacionValor = new System.Windows.Forms.Label();
            this.PNL_CardListas = new System.Windows.Forms.Panel();
            this.LBL_ListasTitulo = new System.Windows.Forms.Label();
            this.LBL_ListasValor = new System.Windows.Forms.Label();
            this.PNL_CardGarantias = new System.Windows.Forms.Panel();
            this.LBL_GarantiasTitulo = new System.Windows.Forms.Label();
            this.LBL_GarantiasValor = new System.Windows.Forms.Label();
            this.PNL_CardBajoMinimo = new System.Windows.Forms.Panel();
            this.LBL_BajoMinimoTitulo = new System.Windows.Forms.Label();
            this.LBL_BajoMinimoValor = new System.Windows.Forms.Label();
            this.BTN_Actualizar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_CardAbiertas.SuspendLayout();
            this.PNL_CardEsperando.SuspendLayout();
            this.PNL_CardReparacion.SuspendLayout();
            this.PNL_CardListas.SuspendLayout();
            this.PNL_CardGarantias.SuspendLayout();
            this.PNL_CardBajoMinimo.SuspendLayout();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Titulo);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(980, 60);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Titulo
            //
            this.LBL_Titulo.AutoSize = true;
            this.LBL_Titulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Titulo.ForeColor = System.Drawing.Color.White;
            this.LBL_Titulo.Location = new System.Drawing.Point(15, 16);
            this.LBL_Titulo.Name = "LBL_Titulo";
            this.LBL_Titulo.Size = new System.Drawing.Size(200, 25);
            this.LBL_Titulo.TabIndex = 0;
            this.LBL_Titulo.Tag = "Dashboard.Titulo";
            this.LBL_Titulo.Text = "Dashboard";
            //
            // PNL_CardAbiertas
            //
            this.PNL_CardAbiertas.BackColor = System.Drawing.Color.White;
            this.PNL_CardAbiertas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardAbiertas.Controls.Add(this.LBL_AbiertasTitulo);
            this.PNL_CardAbiertas.Controls.Add(this.LBL_AbiertasValor);
            this.PNL_CardAbiertas.Location = new System.Drawing.Point(15, 75);
            this.PNL_CardAbiertas.Name = "PNL_CardAbiertas";
            this.PNL_CardAbiertas.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardAbiertas.TabIndex = 1;
            //
            // LBL_AbiertasTitulo
            //
            this.LBL_AbiertasTitulo.AutoSize = true;
            this.LBL_AbiertasTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_AbiertasTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_AbiertasTitulo.Name = "LBL_AbiertasTitulo";
            this.LBL_AbiertasTitulo.Size = new System.Drawing.Size(100, 15);
            this.LBL_AbiertasTitulo.TabIndex = 0;
            this.LBL_AbiertasTitulo.Tag = "Dashboard.Abiertas";
            this.LBL_AbiertasTitulo.Text = "Ordenes abiertas";
            //
            // LBL_AbiertasValor
            //
            this.LBL_AbiertasValor.AutoSize = true;
            this.LBL_AbiertasValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_AbiertasValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_AbiertasValor.Name = "LBL_AbiertasValor";
            this.LBL_AbiertasValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_AbiertasValor.TabIndex = 1;
            this.LBL_AbiertasValor.Text = "0";
            //
            // PNL_CardEsperando
            //
            this.PNL_CardEsperando.BackColor = System.Drawing.Color.White;
            this.PNL_CardEsperando.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardEsperando.Controls.Add(this.LBL_EsperandoTitulo);
            this.PNL_CardEsperando.Controls.Add(this.LBL_EsperandoValor);
            this.PNL_CardEsperando.Location = new System.Drawing.Point(340, 75);
            this.PNL_CardEsperando.Name = "PNL_CardEsperando";
            this.PNL_CardEsperando.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardEsperando.TabIndex = 2;
            //
            // LBL_EsperandoTitulo
            //
            this.LBL_EsperandoTitulo.AutoSize = true;
            this.LBL_EsperandoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_EsperandoTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_EsperandoTitulo.Name = "LBL_EsperandoTitulo";
            this.LBL_EsperandoTitulo.Size = new System.Drawing.Size(120, 15);
            this.LBL_EsperandoTitulo.TabIndex = 0;
            this.LBL_EsperandoTitulo.Tag = "Dashboard.EsperandoRespuesta";
            this.LBL_EsperandoTitulo.Text = "Esperando respuesta";
            //
            // LBL_EsperandoValor
            //
            this.LBL_EsperandoValor.AutoSize = true;
            this.LBL_EsperandoValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_EsperandoValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_EsperandoValor.Name = "LBL_EsperandoValor";
            this.LBL_EsperandoValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_EsperandoValor.TabIndex = 1;
            this.LBL_EsperandoValor.Text = "0";
            //
            // PNL_CardReparacion
            //
            this.PNL_CardReparacion.BackColor = System.Drawing.Color.White;
            this.PNL_CardReparacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardReparacion.Controls.Add(this.LBL_ReparacionTitulo);
            this.PNL_CardReparacion.Controls.Add(this.LBL_ReparacionValor);
            this.PNL_CardReparacion.Location = new System.Drawing.Point(665, 75);
            this.PNL_CardReparacion.Name = "PNL_CardReparacion";
            this.PNL_CardReparacion.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardReparacion.TabIndex = 3;
            //
            // LBL_ReparacionTitulo
            //
            this.LBL_ReparacionTitulo.AutoSize = true;
            this.LBL_ReparacionTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_ReparacionTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_ReparacionTitulo.Name = "LBL_ReparacionTitulo";
            this.LBL_ReparacionTitulo.Size = new System.Drawing.Size(80, 15);
            this.LBL_ReparacionTitulo.TabIndex = 0;
            this.LBL_ReparacionTitulo.Tag = "Dashboard.EnReparacion";
            this.LBL_ReparacionTitulo.Text = "En reparacion";
            //
            // LBL_ReparacionValor
            //
            this.LBL_ReparacionValor.AutoSize = true;
            this.LBL_ReparacionValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_ReparacionValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_ReparacionValor.Name = "LBL_ReparacionValor";
            this.LBL_ReparacionValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_ReparacionValor.TabIndex = 1;
            this.LBL_ReparacionValor.Text = "0";
            //
            // PNL_CardListas
            //
            this.PNL_CardListas.BackColor = System.Drawing.Color.White;
            this.PNL_CardListas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardListas.Controls.Add(this.LBL_ListasTitulo);
            this.PNL_CardListas.Controls.Add(this.LBL_ListasValor);
            this.PNL_CardListas.Location = new System.Drawing.Point(15, 195);
            this.PNL_CardListas.Name = "PNL_CardListas";
            this.PNL_CardListas.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardListas.TabIndex = 4;
            //
            // LBL_ListasTitulo
            //
            this.LBL_ListasTitulo.AutoSize = true;
            this.LBL_ListasTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_ListasTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_ListasTitulo.Name = "LBL_ListasTitulo";
            this.LBL_ListasTitulo.Size = new System.Drawing.Size(100, 15);
            this.LBL_ListasTitulo.TabIndex = 0;
            this.LBL_ListasTitulo.Tag = "Dashboard.ListasRetiro";
            this.LBL_ListasTitulo.Text = "Listas para retiro";
            //
            // LBL_ListasValor
            //
            this.LBL_ListasValor.AutoSize = true;
            this.LBL_ListasValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_ListasValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_ListasValor.Name = "LBL_ListasValor";
            this.LBL_ListasValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_ListasValor.TabIndex = 1;
            this.LBL_ListasValor.Text = "0";
            //
            // PNL_CardGarantias
            //
            this.PNL_CardGarantias.BackColor = System.Drawing.Color.White;
            this.PNL_CardGarantias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardGarantias.Controls.Add(this.LBL_GarantiasTitulo);
            this.PNL_CardGarantias.Controls.Add(this.LBL_GarantiasValor);
            this.PNL_CardGarantias.Location = new System.Drawing.Point(340, 195);
            this.PNL_CardGarantias.Name = "PNL_CardGarantias";
            this.PNL_CardGarantias.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardGarantias.TabIndex = 5;
            //
            // LBL_GarantiasTitulo
            //
            this.LBL_GarantiasTitulo.AutoSize = true;
            this.LBL_GarantiasTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_GarantiasTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_GarantiasTitulo.Name = "LBL_GarantiasTitulo";
            this.LBL_GarantiasTitulo.Size = new System.Drawing.Size(110, 15);
            this.LBL_GarantiasTitulo.TabIndex = 0;
            this.LBL_GarantiasTitulo.Tag = "Dashboard.GarantiasAbiertas";
            this.LBL_GarantiasTitulo.Text = "Garantias abiertas";
            //
            // LBL_GarantiasValor
            //
            this.LBL_GarantiasValor.AutoSize = true;
            this.LBL_GarantiasValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_GarantiasValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_GarantiasValor.Name = "LBL_GarantiasValor";
            this.LBL_GarantiasValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_GarantiasValor.TabIndex = 1;
            this.LBL_GarantiasValor.Text = "0";
            //
            // PNL_CardBajoMinimo
            //
            this.PNL_CardBajoMinimo.BackColor = System.Drawing.Color.White;
            this.PNL_CardBajoMinimo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_CardBajoMinimo.Controls.Add(this.LBL_BajoMinimoTitulo);
            this.PNL_CardBajoMinimo.Controls.Add(this.LBL_BajoMinimoValor);
            this.PNL_CardBajoMinimo.Location = new System.Drawing.Point(665, 195);
            this.PNL_CardBajoMinimo.Name = "PNL_CardBajoMinimo";
            this.PNL_CardBajoMinimo.Size = new System.Drawing.Size(300, 105);
            this.PNL_CardBajoMinimo.TabIndex = 6;
            //
            // LBL_BajoMinimoTitulo
            //
            this.LBL_BajoMinimoTitulo.AutoSize = true;
            this.LBL_BajoMinimoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_BajoMinimoTitulo.Location = new System.Drawing.Point(10, 10);
            this.LBL_BajoMinimoTitulo.Name = "LBL_BajoMinimoTitulo";
            this.LBL_BajoMinimoTitulo.Size = new System.Drawing.Size(120, 15);
            this.LBL_BajoMinimoTitulo.TabIndex = 0;
            this.LBL_BajoMinimoTitulo.Tag = "Dashboard.BajoMinimo";
            this.LBL_BajoMinimoTitulo.Text = "Repuestos bajo minimo";
            //
            // LBL_BajoMinimoValor
            //
            this.LBL_BajoMinimoValor.AutoSize = true;
            this.LBL_BajoMinimoValor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.LBL_BajoMinimoValor.Location = new System.Drawing.Point(10, 40);
            this.LBL_BajoMinimoValor.Name = "LBL_BajoMinimoValor";
            this.LBL_BajoMinimoValor.Size = new System.Drawing.Size(34, 37);
            this.LBL_BajoMinimoValor.TabIndex = 1;
            this.LBL_BajoMinimoValor.Text = "0";
            //
            // BTN_Actualizar
            //
            this.BTN_Actualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Actualizar.FlatAppearance.BorderSize = 0;
            this.BTN_Actualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Actualizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Actualizar.ForeColor = System.Drawing.Color.White;
            this.BTN_Actualizar.Location = new System.Drawing.Point(845, 320);
            this.BTN_Actualizar.Name = "BTN_Actualizar";
            this.BTN_Actualizar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Actualizar.TabIndex = 7;
            this.BTN_Actualizar.Tag = "Dashboard.Actualizar";
            this.BTN_Actualizar.Text = "Actualizar";
            this.BTN_Actualizar.UseVisualStyleBackColor = false;
            this.BTN_Actualizar.Click += new System.EventHandler(this.BTN_Actualizar_Click);
            //
            // FrmDashboard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(980, 545);
            this.Controls.Add(this.BTN_Actualizar);
            this.Controls.Add(this.PNL_CardBajoMinimo);
            this.Controls.Add(this.PNL_CardGarantias);
            this.Controls.Add(this.PNL_CardListas);
            this.Controls.Add(this.PNL_CardReparacion);
            this.Controls.Add(this.PNL_CardEsperando);
            this.Controls.Add(this.PNL_CardAbiertas);
            this.Controls.Add(this.PNL_Header);
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmDashboard.Text";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_CardAbiertas.ResumeLayout(false);
            this.PNL_CardAbiertas.PerformLayout();
            this.PNL_CardEsperando.ResumeLayout(false);
            this.PNL_CardEsperando.PerformLayout();
            this.PNL_CardReparacion.ResumeLayout(false);
            this.PNL_CardReparacion.PerformLayout();
            this.PNL_CardListas.ResumeLayout(false);
            this.PNL_CardListas.PerformLayout();
            this.PNL_CardGarantias.ResumeLayout(false);
            this.PNL_CardGarantias.PerformLayout();
            this.PNL_CardBajoMinimo.ResumeLayout(false);
            this.PNL_CardBajoMinimo.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Titulo;
        private System.Windows.Forms.Panel PNL_CardAbiertas;
        private System.Windows.Forms.Label LBL_AbiertasTitulo;
        private System.Windows.Forms.Label LBL_AbiertasValor;
        private System.Windows.Forms.Panel PNL_CardEsperando;
        private System.Windows.Forms.Label LBL_EsperandoTitulo;
        private System.Windows.Forms.Label LBL_EsperandoValor;
        private System.Windows.Forms.Panel PNL_CardReparacion;
        private System.Windows.Forms.Label LBL_ReparacionTitulo;
        private System.Windows.Forms.Label LBL_ReparacionValor;
        private System.Windows.Forms.Panel PNL_CardListas;
        private System.Windows.Forms.Label LBL_ListasTitulo;
        private System.Windows.Forms.Label LBL_ListasValor;
        private System.Windows.Forms.Panel PNL_CardGarantias;
        private System.Windows.Forms.Label LBL_GarantiasTitulo;
        private System.Windows.Forms.Label LBL_GarantiasValor;
        private System.Windows.Forms.Panel PNL_CardBajoMinimo;
        private System.Windows.Forms.Label LBL_BajoMinimoTitulo;
        private System.Windows.Forms.Label LBL_BajoMinimoValor;
        private System.Windows.Forms.Button BTN_Actualizar;
    }
}
