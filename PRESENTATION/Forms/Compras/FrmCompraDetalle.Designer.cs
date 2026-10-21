namespace UI.Forms.Compras
{
    partial class FrmCompraDetalle
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
            this.LBL_Numero = new System.Windows.Forms.Label();
            this.LBL_Estado = new System.Windows.Forms.Label();
            this.LBL_EstadoValor = new System.Windows.Forms.Label();
            this.LBL_Fecha = new System.Windows.Forms.Label();
            this.LBL_FechaValor = new System.Windows.Forms.Label();
            this.LBL_Total = new System.Windows.Forms.Label();
            this.LBL_TotalValor = new System.Windows.Forms.Label();
            this.PNL_Encabezado = new System.Windows.Forms.Panel();
            this.LBL_Proveedor = new System.Windows.Forms.Label();
            this.CBO_Proveedor = new System.Windows.Forms.ComboBox();
            this.LBL_Obs = new System.Windows.Forms.Label();
            this.TXT_Obs = new System.Windows.Forms.TextBox();
            this.LBL_Motivo = new System.Windows.Forms.Label();
            this.TXT_Motivo = new System.Windows.Forms.TextBox();
            this.DGV_Detalle = new System.Windows.Forms.DataGridView();
            this.PNL_Agregar = new System.Windows.Forms.Panel();
            this.LBL_Repuesto = new System.Windows.Forms.Label();
            this.CBO_Repuesto = new System.Windows.Forms.ComboBox();
            this.LBL_Cantidad = new System.Windows.Forms.Label();
            this.NUM_Cantidad = new System.Windows.Forms.NumericUpDown();
            this.LBL_Costo = new System.Windows.Forms.Label();
            this.NUM_Costo = new System.Windows.Forms.NumericUpDown();
            this.BTN_AgregarItem = new System.Windows.Forms.Button();
            this.BTN_QuitarItem = new System.Windows.Forms.Button();
            this.PNL_Botones = new System.Windows.Forms.Panel();
            this.BTN_GuardarBorrador = new System.Windows.Forms.Button();
            this.BTN_Confirmar = new System.Windows.Forms.Button();
            this.BTN_CancelarCompra = new System.Windows.Forms.Button();
            this.BTN_AnularCompra = new System.Windows.Forms.Button();
            this.BTN_Cerrar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.PNL_Encabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Detalle)).BeginInit();
            this.PNL_Agregar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Cantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Costo)).BeginInit();
            this.PNL_Botones.SuspendLayout();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Numero);
            this.PNL_Header.Controls.Add(this.LBL_Estado);
            this.PNL_Header.Controls.Add(this.LBL_EstadoValor);
            this.PNL_Header.Controls.Add(this.LBL_Fecha);
            this.PNL_Header.Controls.Add(this.LBL_FechaValor);
            this.PNL_Header.Controls.Add(this.LBL_Total);
            this.PNL_Header.Controls.Add(this.LBL_TotalValor);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(880, 90);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Numero
            //
            this.LBL_Numero.AutoSize = true;
            this.LBL_Numero.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Numero.ForeColor = System.Drawing.Color.White;
            this.LBL_Numero.Location = new System.Drawing.Point(15, 8);
            this.LBL_Numero.Name = "LBL_Numero";
            this.LBL_Numero.Size = new System.Drawing.Size(100, 25);
            this.LBL_Numero.TabIndex = 0;
            this.LBL_Numero.Tag = "CompraDetalle.Numero";
            this.LBL_Numero.Text = "Compra";
            //
            // LBL_Estado
            //
            this.LBL_Estado.AutoSize = true;
            this.LBL_Estado.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Estado.ForeColor = System.Drawing.Color.White;
            this.LBL_Estado.Location = new System.Drawing.Point(15, 42);
            this.LBL_Estado.Name = "LBL_Estado";
            this.LBL_Estado.Size = new System.Drawing.Size(45, 15);
            this.LBL_Estado.TabIndex = 1;
            this.LBL_Estado.Tag = "CompraDetalle.Estado";
            this.LBL_Estado.Text = "Estado:";
            //
            // LBL_EstadoValor
            //
            this.LBL_EstadoValor.AutoSize = true;
            this.LBL_EstadoValor.BackColor = System.Drawing.Color.White;
            this.LBL_EstadoValor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_EstadoValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_EstadoValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.LBL_EstadoValor.Location = new System.Drawing.Point(70, 40);
            this.LBL_EstadoValor.Name = "LBL_EstadoValor";
            this.LBL_EstadoValor.Size = new System.Drawing.Size(60, 17);
            this.LBL_EstadoValor.TabIndex = 2;
            this.LBL_EstadoValor.Text = "-";
            //
            // LBL_Fecha
            //
            this.LBL_Fecha.AutoSize = true;
            this.LBL_Fecha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Fecha.ForeColor = System.Drawing.Color.White;
            this.LBL_Fecha.Location = new System.Drawing.Point(300, 42);
            this.LBL_Fecha.Name = "LBL_Fecha";
            this.LBL_Fecha.Size = new System.Drawing.Size(41, 15);
            this.LBL_Fecha.TabIndex = 3;
            this.LBL_Fecha.Tag = "CompraDetalle.Fecha";
            this.LBL_Fecha.Text = "Fecha:";
            //
            // LBL_FechaValor
            //
            this.LBL_FechaValor.AutoSize = true;
            this.LBL_FechaValor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_FechaValor.ForeColor = System.Drawing.Color.White;
            this.LBL_FechaValor.Location = new System.Drawing.Point(347, 42);
            this.LBL_FechaValor.Name = "LBL_FechaValor";
            this.LBL_FechaValor.Size = new System.Drawing.Size(12, 15);
            this.LBL_FechaValor.TabIndex = 4;
            this.LBL_FechaValor.Text = "-";
            //
            // LBL_Total
            //
            this.LBL_Total.AutoSize = true;
            this.LBL_Total.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Total.ForeColor = System.Drawing.Color.White;
            this.LBL_Total.Location = new System.Drawing.Point(600, 42);
            this.LBL_Total.Name = "LBL_Total";
            this.LBL_Total.Size = new System.Drawing.Size(40, 15);
            this.LBL_Total.TabIndex = 5;
            this.LBL_Total.Tag = "CompraDetalle.Total";
            this.LBL_Total.Text = "Total:";
            //
            // LBL_TotalValor
            //
            this.LBL_TotalValor.AutoSize = true;
            this.LBL_TotalValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_TotalValor.ForeColor = System.Drawing.Color.White;
            this.LBL_TotalValor.Location = new System.Drawing.Point(646, 42);
            this.LBL_TotalValor.Name = "LBL_TotalValor";
            this.LBL_TotalValor.Size = new System.Drawing.Size(30, 15);
            this.LBL_TotalValor.TabIndex = 6;
            this.LBL_TotalValor.Text = "0.00";
            //
            // PNL_Encabezado
            //
            this.PNL_Encabezado.BackColor = System.Drawing.Color.White;
            this.PNL_Encabezado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Encabezado.Controls.Add(this.LBL_Proveedor);
            this.PNL_Encabezado.Controls.Add(this.CBO_Proveedor);
            this.PNL_Encabezado.Controls.Add(this.LBL_Obs);
            this.PNL_Encabezado.Controls.Add(this.TXT_Obs);
            this.PNL_Encabezado.Controls.Add(this.LBL_Motivo);
            this.PNL_Encabezado.Controls.Add(this.TXT_Motivo);
            this.PNL_Encabezado.Location = new System.Drawing.Point(15, 100);
            this.PNL_Encabezado.Name = "PNL_Encabezado";
            this.PNL_Encabezado.Size = new System.Drawing.Size(850, 120);
            this.PNL_Encabezado.TabIndex = 1;
            //
            // LBL_Proveedor
            //
            this.LBL_Proveedor.AutoSize = true;
            this.LBL_Proveedor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Proveedor.Location = new System.Drawing.Point(10, 14);
            this.LBL_Proveedor.Name = "LBL_Proveedor";
            this.LBL_Proveedor.Size = new System.Drawing.Size(64, 15);
            this.LBL_Proveedor.TabIndex = 0;
            this.LBL_Proveedor.Tag = "CompraDetalle.Proveedor";
            this.LBL_Proveedor.Text = "Proveedor:";
            //
            // CBO_Proveedor
            //
            this.CBO_Proveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Proveedor.Location = new System.Drawing.Point(100, 11);
            this.CBO_Proveedor.Name = "CBO_Proveedor";
            this.CBO_Proveedor.Size = new System.Drawing.Size(330, 21);
            this.CBO_Proveedor.TabIndex = 1;
            //
            // LBL_Obs
            //
            this.LBL_Obs.AutoSize = true;
            this.LBL_Obs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Obs.Location = new System.Drawing.Point(10, 48);
            this.LBL_Obs.Name = "LBL_Obs";
            this.LBL_Obs.Size = new System.Drawing.Size(87, 15);
            this.LBL_Obs.TabIndex = 2;
            this.LBL_Obs.Tag = "CompraDetalle.Observaciones";
            this.LBL_Obs.Text = "Observaciones:";
            //
            // TXT_Obs
            //
            this.TXT_Obs.Location = new System.Drawing.Point(100, 45);
            this.TXT_Obs.Name = "TXT_Obs";
            this.TXT_Obs.Size = new System.Drawing.Size(735, 22);
            this.TXT_Obs.TabIndex = 3;
            //
            // LBL_Motivo
            //
            this.LBL_Motivo.AutoSize = true;
            this.LBL_Motivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Motivo.Location = new System.Drawing.Point(10, 78);
            this.LBL_Motivo.Name = "LBL_Motivo";
            this.LBL_Motivo.Size = new System.Drawing.Size(87, 15);
            this.LBL_Motivo.TabIndex = 4;
            this.LBL_Motivo.Tag = "CompraDetalle.MotivoAnulacion";
            this.LBL_Motivo.Text = "Motivo anulacion:";
            this.LBL_Motivo.Visible = false;
            //
            // TXT_Motivo
            //
            this.TXT_Motivo.Location = new System.Drawing.Point(100, 75);
            this.TXT_Motivo.Name = "TXT_Motivo";
            this.TXT_Motivo.ReadOnly = true;
            this.TXT_Motivo.Size = new System.Drawing.Size(735, 22);
            this.TXT_Motivo.TabIndex = 5;
            this.TXT_Motivo.Visible = false;
            //
            // DGV_Detalle
            //
            this.DGV_Detalle.AllowUserToAddRows = false;
            this.DGV_Detalle.AllowUserToDeleteRows = false;
            this.DGV_Detalle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Detalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Detalle.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Detalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Detalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Detalle.Location = new System.Drawing.Point(15, 230);
            this.DGV_Detalle.MultiSelect = false;
            this.DGV_Detalle.Name = "DGV_Detalle";
            this.DGV_Detalle.ReadOnly = true;
            this.DGV_Detalle.RowHeadersVisible = false;
            this.DGV_Detalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Detalle.Size = new System.Drawing.Size(850, 170);
            this.DGV_Detalle.TabIndex = 2;
            this.DGV_Detalle.SelectionChanged += new System.EventHandler(this.DGV_Detalle_SelectionChanged);
            //
            // PNL_Agregar
            //
            this.PNL_Agregar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Agregar.BackColor = System.Drawing.Color.White;
            this.PNL_Agregar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNL_Agregar.Controls.Add(this.LBL_Repuesto);
            this.PNL_Agregar.Controls.Add(this.CBO_Repuesto);
            this.PNL_Agregar.Controls.Add(this.LBL_Cantidad);
            this.PNL_Agregar.Controls.Add(this.NUM_Cantidad);
            this.PNL_Agregar.Controls.Add(this.LBL_Costo);
            this.PNL_Agregar.Controls.Add(this.NUM_Costo);
            this.PNL_Agregar.Controls.Add(this.BTN_AgregarItem);
            this.PNL_Agregar.Controls.Add(this.BTN_QuitarItem);
            this.PNL_Agregar.Location = new System.Drawing.Point(15, 410);
            this.PNL_Agregar.Name = "PNL_Agregar";
            this.PNL_Agregar.Size = new System.Drawing.Size(850, 70);
            this.PNL_Agregar.TabIndex = 3;
            //
            // LBL_Repuesto
            //
            this.LBL_Repuesto.AutoSize = true;
            this.LBL_Repuesto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Repuesto.Location = new System.Drawing.Point(10, 12);
            this.LBL_Repuesto.Name = "LBL_Repuesto";
            this.LBL_Repuesto.Size = new System.Drawing.Size(61, 15);
            this.LBL_Repuesto.TabIndex = 0;
            this.LBL_Repuesto.Tag = "CompraDetalle.Repuesto";
            this.LBL_Repuesto.Text = "Repuesto:";
            //
            // CBO_Repuesto
            //
            this.CBO_Repuesto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_Repuesto.Location = new System.Drawing.Point(80, 9);
            this.CBO_Repuesto.Name = "CBO_Repuesto";
            this.CBO_Repuesto.Size = new System.Drawing.Size(250, 21);
            this.CBO_Repuesto.TabIndex = 1;
            //
            // LBL_Cantidad
            //
            this.LBL_Cantidad.AutoSize = true;
            this.LBL_Cantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Cantidad.Location = new System.Drawing.Point(340, 12);
            this.LBL_Cantidad.Name = "LBL_Cantidad";
            this.LBL_Cantidad.Size = new System.Drawing.Size(58, 15);
            this.LBL_Cantidad.TabIndex = 2;
            this.LBL_Cantidad.Tag = "CompraDetalle.Cantidad";
            this.LBL_Cantidad.Text = "Cantidad:";
            //
            // NUM_Cantidad
            //
            this.NUM_Cantidad.Location = new System.Drawing.Point(404, 9);
            this.NUM_Cantidad.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUM_Cantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_Cantidad.Name = "NUM_Cantidad";
            this.NUM_Cantidad.Size = new System.Drawing.Size(70, 22);
            this.NUM_Cantidad.TabIndex = 3;
            this.NUM_Cantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // LBL_Costo
            //
            this.LBL_Costo.AutoSize = true;
            this.LBL_Costo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_Costo.Location = new System.Drawing.Point(484, 12);
            this.LBL_Costo.Name = "LBL_Costo";
            this.LBL_Costo.Size = new System.Drawing.Size(42, 15);
            this.LBL_Costo.TabIndex = 4;
            this.LBL_Costo.Tag = "CompraDetalle.Costo";
            this.LBL_Costo.Text = "Costo:";
            //
            // NUM_Costo
            //
            this.NUM_Costo.DecimalPlaces = 2;
            this.NUM_Costo.Location = new System.Drawing.Point(532, 9);
            this.NUM_Costo.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUM_Costo.Name = "NUM_Costo";
            this.NUM_Costo.Size = new System.Drawing.Size(110, 22);
            this.NUM_Costo.TabIndex = 5;
            //
            // BTN_AgregarItem
            //
            this.BTN_AgregarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_AgregarItem.FlatAppearance.BorderSize = 0;
            this.BTN_AgregarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_AgregarItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_AgregarItem.ForeColor = System.Drawing.Color.White;
            this.BTN_AgregarItem.Location = new System.Drawing.Point(652, 6);
            this.BTN_AgregarItem.Name = "BTN_AgregarItem";
            this.BTN_AgregarItem.Size = new System.Drawing.Size(90, 28);
            this.BTN_AgregarItem.TabIndex = 6;
            this.BTN_AgregarItem.Tag = "CompraDetalle.AgregarItem";
            this.BTN_AgregarItem.Text = "Agregar";
            this.BTN_AgregarItem.UseVisualStyleBackColor = false;
            this.BTN_AgregarItem.Click += new System.EventHandler(this.BTN_AgregarItem_Click);
            //
            // BTN_QuitarItem
            //
            this.BTN_QuitarItem.BackColor = System.Drawing.Color.Maroon;
            this.BTN_QuitarItem.FlatAppearance.BorderSize = 0;
            this.BTN_QuitarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_QuitarItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_QuitarItem.ForeColor = System.Drawing.Color.White;
            this.BTN_QuitarItem.Location = new System.Drawing.Point(748, 6);
            this.BTN_QuitarItem.Name = "BTN_QuitarItem";
            this.BTN_QuitarItem.Size = new System.Drawing.Size(90, 28);
            this.BTN_QuitarItem.TabIndex = 7;
            this.BTN_QuitarItem.Tag = "CompraDetalle.QuitarItem";
            this.BTN_QuitarItem.Text = "Quitar";
            this.BTN_QuitarItem.UseVisualStyleBackColor = false;
            this.BTN_QuitarItem.Click += new System.EventHandler(this.BTN_QuitarItem_Click);
            //
            // PNL_Botones
            //
            this.PNL_Botones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PNL_Botones.Controls.Add(this.BTN_GuardarBorrador);
            this.PNL_Botones.Controls.Add(this.BTN_Confirmar);
            this.PNL_Botones.Controls.Add(this.BTN_CancelarCompra);
            this.PNL_Botones.Controls.Add(this.BTN_AnularCompra);
            this.PNL_Botones.Controls.Add(this.BTN_Cerrar);
            this.PNL_Botones.Location = new System.Drawing.Point(15, 490);
            this.PNL_Botones.Name = "PNL_Botones";
            this.PNL_Botones.Size = new System.Drawing.Size(850, 50);
            this.PNL_Botones.TabIndex = 4;
            //
            // BTN_GuardarBorrador
            //
            this.BTN_GuardarBorrador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.BTN_GuardarBorrador.FlatAppearance.BorderSize = 0;
            this.BTN_GuardarBorrador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_GuardarBorrador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_GuardarBorrador.ForeColor = System.Drawing.Color.White;
            this.BTN_GuardarBorrador.Location = new System.Drawing.Point(5, 10);
            this.BTN_GuardarBorrador.Name = "BTN_GuardarBorrador";
            this.BTN_GuardarBorrador.Size = new System.Drawing.Size(140, 30);
            this.BTN_GuardarBorrador.TabIndex = 0;
            this.BTN_GuardarBorrador.Tag = "CompraDetalle.GuardarBorrador";
            this.BTN_GuardarBorrador.Text = "Guardar borrador";
            this.BTN_GuardarBorrador.UseVisualStyleBackColor = false;
            this.BTN_GuardarBorrador.Click += new System.EventHandler(this.BTN_GuardarBorrador_Click);
            //
            // BTN_Confirmar
            //
            this.BTN_Confirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Confirmar.FlatAppearance.BorderSize = 0;
            this.BTN_Confirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Confirmar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Confirmar.ForeColor = System.Drawing.Color.White;
            this.BTN_Confirmar.Location = new System.Drawing.Point(155, 10);
            this.BTN_Confirmar.Name = "BTN_Confirmar";
            this.BTN_Confirmar.Size = new System.Drawing.Size(120, 30);
            this.BTN_Confirmar.TabIndex = 1;
            this.BTN_Confirmar.Tag = "CompraDetalle.Confirmar";
            this.BTN_Confirmar.Text = "Confirmar";
            this.BTN_Confirmar.UseVisualStyleBackColor = false;
            this.BTN_Confirmar.Click += new System.EventHandler(this.BTN_Confirmar_Click);
            //
            // BTN_CancelarCompra
            //
            this.BTN_CancelarCompra.BackColor = System.Drawing.Color.Maroon;
            this.BTN_CancelarCompra.FlatAppearance.BorderSize = 0;
            this.BTN_CancelarCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CancelarCompra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_CancelarCompra.ForeColor = System.Drawing.Color.White;
            this.BTN_CancelarCompra.Location = new System.Drawing.Point(285, 10);
            this.BTN_CancelarCompra.Name = "BTN_CancelarCompra";
            this.BTN_CancelarCompra.Size = new System.Drawing.Size(120, 30);
            this.BTN_CancelarCompra.TabIndex = 2;
            this.BTN_CancelarCompra.Tag = "CompraDetalle.Cancelar";
            this.BTN_CancelarCompra.Text = "Cancelar";
            this.BTN_CancelarCompra.UseVisualStyleBackColor = false;
            this.BTN_CancelarCompra.Click += new System.EventHandler(this.BTN_CancelarCompra_Click);
            //
            // BTN_AnularCompra
            //
            this.BTN_AnularCompra.BackColor = System.Drawing.Color.Maroon;
            this.BTN_AnularCompra.FlatAppearance.BorderSize = 0;
            this.BTN_AnularCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_AnularCompra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_AnularCompra.ForeColor = System.Drawing.Color.White;
            this.BTN_AnularCompra.Location = new System.Drawing.Point(415, 10);
            this.BTN_AnularCompra.Name = "BTN_AnularCompra";
            this.BTN_AnularCompra.Size = new System.Drawing.Size(120, 30);
            this.BTN_AnularCompra.TabIndex = 4;
            this.BTN_AnularCompra.Tag = "CompraDetalle.Anular";
            this.BTN_AnularCompra.Text = "Anular";
            this.BTN_AnularCompra.UseVisualStyleBackColor = false;
            this.BTN_AnularCompra.Click += new System.EventHandler(this.BTN_AnularCompra_Click);
            //
            // BTN_Cerrar
            //
            this.BTN_Cerrar.BackColor = System.Drawing.Color.Gray;
            this.BTN_Cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_Cerrar.FlatAppearance.BorderSize = 0;
            this.BTN_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Cerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Cerrar.ForeColor = System.Drawing.Color.White;
            this.BTN_Cerrar.Location = new System.Drawing.Point(725, 10);
            this.BTN_Cerrar.Name = "BTN_Cerrar";
            this.BTN_Cerrar.Size = new System.Drawing.Size(115, 30);
            this.BTN_Cerrar.TabIndex = 3;
            this.BTN_Cerrar.Tag = "CompraDetalle.Cerrar";
            this.BTN_Cerrar.Text = "Cerrar";
            this.BTN_Cerrar.UseVisualStyleBackColor = false;
            //
            // FrmCompraDetalle
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cerrar;
            this.ClientSize = new System.Drawing.Size(880, 554);
            this.Controls.Add(this.PNL_Botones);
            this.Controls.Add(this.PNL_Agregar);
            this.Controls.Add(this.DGV_Detalle);
            this.Controls.Add(this.PNL_Encabezado);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCompraDetalle";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "CompraDetalle.TituloNuevo";
            this.Text = "Nueva compra";
            this.Load += new System.EventHandler(this.FrmCompraDetalle_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.PNL_Encabezado.ResumeLayout(false);
            this.PNL_Encabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Detalle)).EndInit();
            this.PNL_Agregar.ResumeLayout(false);
            this.PNL_Agregar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Cantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_Costo)).EndInit();
            this.PNL_Botones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Numero;
        private System.Windows.Forms.Label LBL_Estado;
        private System.Windows.Forms.Label LBL_EstadoValor;
        private System.Windows.Forms.Label LBL_Fecha;
        private System.Windows.Forms.Label LBL_FechaValor;
        private System.Windows.Forms.Label LBL_Total;
        private System.Windows.Forms.Label LBL_TotalValor;
        private System.Windows.Forms.Panel PNL_Encabezado;
        private System.Windows.Forms.Label LBL_Proveedor;
        private System.Windows.Forms.ComboBox CBO_Proveedor;
        private System.Windows.Forms.Label LBL_Obs;
        private System.Windows.Forms.TextBox TXT_Obs;
        private System.Windows.Forms.Label LBL_Motivo;
        private System.Windows.Forms.TextBox TXT_Motivo;
        private System.Windows.Forms.DataGridView DGV_Detalle;
        private System.Windows.Forms.Panel PNL_Agregar;
        private System.Windows.Forms.Label LBL_Repuesto;
        private System.Windows.Forms.ComboBox CBO_Repuesto;
        private System.Windows.Forms.Label LBL_Cantidad;
        private System.Windows.Forms.NumericUpDown NUM_Cantidad;
        private System.Windows.Forms.Label LBL_Costo;
        private System.Windows.Forms.NumericUpDown NUM_Costo;
        private System.Windows.Forms.Button BTN_AgregarItem;
        private System.Windows.Forms.Button BTN_QuitarItem;
        private System.Windows.Forms.Panel PNL_Botones;
        private System.Windows.Forms.Button BTN_GuardarBorrador;
        private System.Windows.Forms.Button BTN_Confirmar;
        private System.Windows.Forms.Button BTN_CancelarCompra;
        private System.Windows.Forms.Button BTN_AnularCompra;
        private System.Windows.Forms.Button BTN_Cerrar;
    }
}
