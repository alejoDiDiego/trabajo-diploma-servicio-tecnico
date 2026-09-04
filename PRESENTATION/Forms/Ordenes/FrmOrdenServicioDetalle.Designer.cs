namespace UI.Forms.Ordenes
{
    partial class FrmOrdenServicioDetalle
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
            this.LBL_Resultado = new System.Windows.Forms.Label();
            this.LBL_Tipo = new System.Windows.Forms.Label();
            this.LBL_HCliente = new System.Windows.Forms.Label();
            this.LBL_HEquipo = new System.Windows.Forms.Label();
            this.LBL_HTecnico = new System.Windows.Forms.Label();
            this.LBL_HFecha = new System.Windows.Forms.Label();
            this.TAB_Detalle = new System.Windows.Forms.TabControl();
            this.TAB_Recepcion = new System.Windows.Forms.TabPage();
            this.LBL_RCliente = new System.Windows.Forms.Label();
            this.CBO_RCliente = new System.Windows.Forms.ComboBox();
            this.LBL_REquipo = new System.Windows.Forms.Label();
            this.CBO_REquipo = new System.Windows.Forms.ComboBox();
            this.LBL_RTecnico = new System.Windows.Forms.Label();
            this.CBO_RTecnico = new System.Windows.Forms.ComboBox();
            this.LBL_RProblema = new System.Windows.Forms.Label();
            this.TXT_RProblema = new System.Windows.Forms.TextBox();
            this.LBL_REstadoFisico = new System.Windows.Forms.Label();
            this.TXT_REstadoFisico = new System.Windows.Forms.TextBox();
            this.LBL_RAccesorios = new System.Windows.Forms.Label();
            this.TXT_RAccesorios = new System.Windows.Forms.TextBox();
            this.LBL_RObs = new System.Windows.Forms.Label();
            this.TXT_RObs = new System.Windows.Forms.TextBox();
            this.BTN_CrearOrden = new System.Windows.Forms.Button();
            this.BTN_GuardarRecepcion = new System.Windows.Forms.Button();
            this.BTN_AsignarTecnico = new System.Windows.Forms.Button();
            this.TAB_Diagnostico = new System.Windows.Forms.TabPage();
            this.LBL_DDescripcion = new System.Windows.Forms.Label();
            this.TXT_DDescripcion = new System.Windows.Forms.TextBox();
            this.CHK_EsReparable = new System.Windows.Forms.CheckBox();
            this.LBL_DDias = new System.Windows.Forms.Label();
            this.NUM_DDias = new System.Windows.Forms.NumericUpDown();
            this.LBL_DObs = new System.Windows.Forms.Label();
            this.TXT_DObs = new System.Windows.Forms.TextBox();
            this.LBL_AvisoNoReparable = new System.Windows.Forms.Label();
            this.BTN_Iniciar = new System.Windows.Forms.Button();
            this.BTN_Finalizar = new System.Windows.Forms.Button();
            this.TAB_Presupuesto = new System.Windows.Forms.TabPage();
            this.DGV_Detalle = new System.Windows.Forms.DataGridView();
            this.LBL_PTipo = new System.Windows.Forms.Label();
            this.CBO_PTipo = new System.Windows.Forms.ComboBox();
            this.LBL_PDesc = new System.Windows.Forms.Label();
            this.TXT_PDesc = new System.Windows.Forms.TextBox();
            this.LBL_PCant = new System.Windows.Forms.Label();
            this.NUM_PCant = new System.Windows.Forms.NumericUpDown();
            this.LBL_PPrecio = new System.Windows.Forms.Label();
            this.NUM_PPrecio = new System.Windows.Forms.NumericUpDown();
            this.BTN_AgregarItem = new System.Windows.Forms.Button();
            this.BTN_QuitarItem = new System.Windows.Forms.Button();
            this.LBL_PDescuento = new System.Windows.Forms.Label();
            this.NUM_PDescuento = new System.Windows.Forms.NumericUpDown();
            this.LBL_PGarantia = new System.Windows.Forms.Label();
            this.NUM_PGarantia = new System.Windows.Forms.NumericUpDown();
            this.LBL_PMedio = new System.Windows.Forms.Label();
            this.TXT_PMedio = new System.Windows.Forms.TextBox();
            this.LBL_PMotivo = new System.Windows.Forms.Label();
            this.TXT_PMotivo = new System.Windows.Forms.TextBox();
            this.LBL_PObs = new System.Windows.Forms.Label();
            this.TXT_PObs = new System.Windows.Forms.TextBox();
            this.LBL_Totales = new System.Windows.Forms.Label();
            this.LBL_EstadoPresupuesto = new System.Windows.Forms.Label();
            this.BTN_Emitir = new System.Windows.Forms.Button();
            this.BTN_Aprobar = new System.Windows.Forms.Button();
            this.BTN_Rechazar = new System.Windows.Forms.Button();
            this.TAB_Historial = new System.Windows.Forms.TabPage();
            this.DGV_Historial = new System.Windows.Forms.DataGridView();
            this.TAB_Reparaciones = new System.Windows.Forms.TabPage();
            this.DGV_Reparaciones = new System.Windows.Forms.DataGridView();
            this.BTN_IniciarReparacion = new System.Windows.Forms.Button();
            this.LBL_CRepuesto = new System.Windows.Forms.Label();
            this.CBO_ConsumoRepuesto = new System.Windows.Forms.ComboBox();
            this.LBL_CCantidad = new System.Windows.Forms.Label();
            this.NUM_ConsumoCantidad = new System.Windows.Forms.NumericUpDown();
            this.BTN_Consumir = new System.Windows.Forms.Button();
            this.LBL_Consumidos = new System.Windows.Forms.Label();
            this.DGV_Consumidos = new System.Windows.Forms.DataGridView();
            this.LBL_CostoTotal = new System.Windows.Forms.Label();
            this.LBL_FTrabajo = new System.Windows.Forms.Label();
            this.TXT_FTrabajo = new System.Windows.Forms.TextBox();
            this.LBL_FObs = new System.Windows.Forms.Label();
            this.TXT_FObs = new System.Windows.Forms.TextBox();
            this.BTN_FinalizarReparacion = new System.Windows.Forms.Button();
            this.TAB_Pruebas = new System.Windows.Forms.TabPage();
            this.LBL_PIntervencion = new System.Windows.Forms.Label();
            this.CBO_PruebaReparacion = new System.Windows.Forms.ComboBox();
            this.DGV_Pruebas = new System.Windows.Forms.DataGridView();
            this.LBL_PDDesc = new System.Windows.Forms.Label();
            this.TXT_PruebaDesc = new System.Windows.Forms.TextBox();
            this.RDO_Aprobada = new System.Windows.Forms.RadioButton();
            this.RDO_Fallida = new System.Windows.Forms.RadioButton();
            this.LBL_PDObs = new System.Windows.Forms.Label();
            this.TXT_PruebaObs = new System.Windows.Forms.TextBox();
            this.BTN_RegistrarPrueba = new System.Windows.Forms.Button();
            this.TAB_Entrega = new System.Windows.Forms.TabPage();
            this.LBL_EEntregadoA = new System.Windows.Forms.Label();
            this.TXT_EEntregadoA = new System.Windows.Forms.TextBox();
            this.LBL_EDocumento = new System.Windows.Forms.Label();
            this.TXT_EDocumento = new System.Windows.Forms.TextBox();
            this.LBL_EObs = new System.Windows.Forms.Label();
            this.TXT_EObs = new System.Windows.Forms.TextBox();
            this.LBL_FechaEntrega = new System.Windows.Forms.Label();
            this.BTN_Entregar = new System.Windows.Forms.Button();
            this.BTN_Cerrar = new System.Windows.Forms.Button();
            this.PNL_Header.SuspendLayout();
            this.TAB_Detalle.SuspendLayout();
            this.TAB_Recepcion.SuspendLayout();
            this.TAB_Diagnostico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_DDias)).BeginInit();
            this.TAB_Presupuesto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Detalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PCant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PPrecio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PGarantia)).BeginInit();
            this.TAB_Historial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Historial)).BeginInit();
            this.TAB_Reparaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Reparaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_ConsumoCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Consumidos)).BeginInit();
            this.TAB_Pruebas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Pruebas)).BeginInit();
            this.TAB_Entrega.SuspendLayout();
            this.SuspendLayout();
            //
            // PNL_Header
            //
            this.PNL_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.PNL_Header.Controls.Add(this.LBL_Numero);
            this.PNL_Header.Controls.Add(this.LBL_Estado);
            this.PNL_Header.Controls.Add(this.LBL_Resultado);
            this.PNL_Header.Controls.Add(this.LBL_Tipo);
            this.PNL_Header.Controls.Add(this.LBL_HCliente);
            this.PNL_Header.Controls.Add(this.LBL_HEquipo);
            this.PNL_Header.Controls.Add(this.LBL_HTecnico);
            this.PNL_Header.Controls.Add(this.LBL_HFecha);
            this.PNL_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_Header.Location = new System.Drawing.Point(0, 0);
            this.PNL_Header.Name = "PNL_Header";
            this.PNL_Header.Size = new System.Drawing.Size(920, 104);
            this.PNL_Header.TabIndex = 0;
            //
            // LBL_Numero
            //
            this.LBL_Numero.AutoSize = true;
            this.LBL_Numero.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LBL_Numero.ForeColor = System.Drawing.Color.White;
            this.LBL_Numero.Location = new System.Drawing.Point(15, 8);
            this.LBL_Numero.Name = "LBL_Numero";
            this.LBL_Numero.Size = new System.Drawing.Size(120, 25);
            this.LBL_Numero.TabIndex = 0;
            this.LBL_Numero.Text = "Orden #0";
            //
            // LBL_Estado
            //
            this.LBL_Estado.AutoSize = true;
            this.LBL_Estado.BackColor = System.Drawing.Color.White;
            this.LBL_Estado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_Estado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Estado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.LBL_Estado.Location = new System.Drawing.Point(15, 40);
            this.LBL_Estado.Name = "LBL_Estado";
            this.LBL_Estado.Size = new System.Drawing.Size(60, 17);
            this.LBL_Estado.TabIndex = 1;
            this.LBL_Estado.Text = "Estado";
            //
            // LBL_Resultado
            //
            this.LBL_Resultado.AutoSize = true;
            this.LBL_Resultado.BackColor = System.Drawing.Color.White;
            this.LBL_Resultado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_Resultado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Resultado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.LBL_Resultado.Location = new System.Drawing.Point(220, 40);
            this.LBL_Resultado.Name = "LBL_Resultado";
            this.LBL_Resultado.Size = new System.Drawing.Size(70, 17);
            this.LBL_Resultado.TabIndex = 2;
            this.LBL_Resultado.Text = "Resultado";
            //
            // LBL_Tipo
            //
            this.LBL_Tipo.AutoSize = true;
            this.LBL_Tipo.BackColor = System.Drawing.Color.White;
            this.LBL_Tipo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBL_Tipo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Tipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.LBL_Tipo.Location = new System.Drawing.Point(425, 40);
            this.LBL_Tipo.Name = "LBL_Tipo";
            this.LBL_Tipo.Size = new System.Drawing.Size(40, 17);
            this.LBL_Tipo.TabIndex = 3;
            this.LBL_Tipo.Text = "Tipo";
            //
            // LBL_HCliente
            //
            this.LBL_HCliente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_HCliente.ForeColor = System.Drawing.Color.White;
            this.LBL_HCliente.Location = new System.Drawing.Point(15, 68);
            this.LBL_HCliente.Name = "LBL_HCliente";
            this.LBL_HCliente.Size = new System.Drawing.Size(270, 30);
            this.LBL_HCliente.TabIndex = 4;
            this.LBL_HCliente.Text = "Cliente:";
            //
            // LBL_HEquipo
            //
            this.LBL_HEquipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_HEquipo.ForeColor = System.Drawing.Color.White;
            this.LBL_HEquipo.Location = new System.Drawing.Point(295, 68);
            this.LBL_HEquipo.Name = "LBL_HEquipo";
            this.LBL_HEquipo.Size = new System.Drawing.Size(270, 30);
            this.LBL_HEquipo.TabIndex = 5;
            this.LBL_HEquipo.Text = "Equipo:";
            //
            // LBL_HTecnico
            //
            this.LBL_HTecnico.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_HTecnico.ForeColor = System.Drawing.Color.White;
            this.LBL_HTecnico.Location = new System.Drawing.Point(575, 68);
            this.LBL_HTecnico.Name = "LBL_HTecnico";
            this.LBL_HTecnico.Size = new System.Drawing.Size(180, 30);
            this.LBL_HTecnico.TabIndex = 6;
            this.LBL_HTecnico.Text = "Tecnico:";
            //
            // LBL_HFecha
            //
            this.LBL_HFecha.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_HFecha.ForeColor = System.Drawing.Color.White;
            this.LBL_HFecha.Location = new System.Drawing.Point(765, 68);
            this.LBL_HFecha.Name = "LBL_HFecha";
            this.LBL_HFecha.Size = new System.Drawing.Size(140, 30);
            this.LBL_HFecha.TabIndex = 7;
            this.LBL_HFecha.Text = "Fecha:";
            //
            // TAB_Detalle
            //
            this.TAB_Detalle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TAB_Detalle.Controls.Add(this.TAB_Recepcion);
            this.TAB_Detalle.Controls.Add(this.TAB_Diagnostico);
            this.TAB_Detalle.Controls.Add(this.TAB_Presupuesto);
            this.TAB_Detalle.Controls.Add(this.TAB_Reparaciones);
            this.TAB_Detalle.Controls.Add(this.TAB_Pruebas);
            this.TAB_Detalle.Controls.Add(this.TAB_Historial);
            this.TAB_Detalle.Controls.Add(this.TAB_Entrega);
            this.TAB_Detalle.Location = new System.Drawing.Point(12, 112);
            this.TAB_Detalle.Name = "TAB_Detalle";
            this.TAB_Detalle.SelectedIndex = 0;
            this.TAB_Detalle.Size = new System.Drawing.Size(896, 480);
            this.TAB_Detalle.TabIndex = 1;
            //
            // TAB_Recepcion
            //
            this.TAB_Recepcion.Controls.Add(this.LBL_RCliente);
            this.TAB_Recepcion.Controls.Add(this.CBO_RCliente);
            this.TAB_Recepcion.Controls.Add(this.LBL_REquipo);
            this.TAB_Recepcion.Controls.Add(this.CBO_REquipo);
            this.TAB_Recepcion.Controls.Add(this.LBL_RTecnico);
            this.TAB_Recepcion.Controls.Add(this.CBO_RTecnico);
            this.TAB_Recepcion.Controls.Add(this.LBL_RProblema);
            this.TAB_Recepcion.Controls.Add(this.TXT_RProblema);
            this.TAB_Recepcion.Controls.Add(this.LBL_REstadoFisico);
            this.TAB_Recepcion.Controls.Add(this.TXT_REstadoFisico);
            this.TAB_Recepcion.Controls.Add(this.LBL_RAccesorios);
            this.TAB_Recepcion.Controls.Add(this.TXT_RAccesorios);
            this.TAB_Recepcion.Controls.Add(this.LBL_RObs);
            this.TAB_Recepcion.Controls.Add(this.TXT_RObs);
            this.TAB_Recepcion.Controls.Add(this.BTN_CrearOrden);
            this.TAB_Recepcion.Controls.Add(this.BTN_GuardarRecepcion);
            this.TAB_Recepcion.Controls.Add(this.BTN_AsignarTecnico);
            this.TAB_Recepcion.Location = new System.Drawing.Point(4, 22);
            this.TAB_Recepcion.Name = "TAB_Recepcion";
            this.TAB_Recepcion.Size = new System.Drawing.Size(888, 454);
            this.TAB_Recepcion.TabIndex = 0;
            this.TAB_Recepcion.Tag = "OrdenDetalle.TabRecepcion";
            this.TAB_Recepcion.Text = "Recepcion";
            this.TAB_Recepcion.UseVisualStyleBackColor = true;
            //
            // LBL_RCliente
            //
            this.LBL_RCliente.AutoSize = true;
            this.LBL_RCliente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RCliente.Location = new System.Drawing.Point(12, 14);
            this.LBL_RCliente.Name = "LBL_RCliente";
            this.LBL_RCliente.Size = new System.Drawing.Size(47, 15);
            this.LBL_RCliente.TabIndex = 0;
            this.LBL_RCliente.Tag = "OrdenDetalle.Cliente";
            this.LBL_RCliente.Text = "Cliente:";
            //
            // CBO_RCliente
            //
            this.CBO_RCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_RCliente.Location = new System.Drawing.Point(150, 11);
            this.CBO_RCliente.Name = "CBO_RCliente";
            this.CBO_RCliente.Size = new System.Drawing.Size(330, 21);
            this.CBO_RCliente.TabIndex = 1;
            this.CBO_RCliente.SelectedIndexChanged += new System.EventHandler(this.CBO_RCliente_SelectedIndexChanged);
            //
            // LBL_REquipo
            //
            this.LBL_REquipo.AutoSize = true;
            this.LBL_REquipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_REquipo.Location = new System.Drawing.Point(12, 44);
            this.LBL_REquipo.Name = "LBL_REquipo";
            this.LBL_REquipo.Size = new System.Drawing.Size(48, 15);
            this.LBL_REquipo.TabIndex = 2;
            this.LBL_REquipo.Tag = "OrdenDetalle.Equipo";
            this.LBL_REquipo.Text = "Equipo:";
            //
            // CBO_REquipo
            //
            this.CBO_REquipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_REquipo.Location = new System.Drawing.Point(150, 41);
            this.CBO_REquipo.Name = "CBO_REquipo";
            this.CBO_REquipo.Size = new System.Drawing.Size(330, 21);
            this.CBO_REquipo.TabIndex = 3;
            //
            // LBL_RTecnico
            //
            this.LBL_RTecnico.AutoSize = true;
            this.LBL_RTecnico.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RTecnico.Location = new System.Drawing.Point(12, 74);
            this.LBL_RTecnico.Name = "LBL_RTecnico";
            this.LBL_RTecnico.Size = new System.Drawing.Size(53, 15);
            this.LBL_RTecnico.TabIndex = 4;
            this.LBL_RTecnico.Tag = "OrdenDetalle.Tecnico";
            this.LBL_RTecnico.Text = "Tecnico:";
            //
            // CBO_RTecnico
            //
            this.CBO_RTecnico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_RTecnico.Location = new System.Drawing.Point(150, 71);
            this.CBO_RTecnico.Name = "CBO_RTecnico";
            this.CBO_RTecnico.Size = new System.Drawing.Size(330, 21);
            this.CBO_RTecnico.TabIndex = 5;
            //
            // LBL_RProblema
            //
            this.LBL_RProblema.AutoSize = true;
            this.LBL_RProblema.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RProblema.Location = new System.Drawing.Point(12, 104);
            this.LBL_RProblema.Name = "LBL_RProblema";
            this.LBL_RProblema.Size = new System.Drawing.Size(62, 15);
            this.LBL_RProblema.TabIndex = 6;
            this.LBL_RProblema.Tag = "OrdenDetalle.Problema";
            this.LBL_RProblema.Text = "Problema:";
            //
            // TXT_RProblema
            //
            this.TXT_RProblema.Location = new System.Drawing.Point(150, 101);
            this.TXT_RProblema.Multiline = true;
            this.TXT_RProblema.Name = "TXT_RProblema";
            this.TXT_RProblema.Size = new System.Drawing.Size(720, 44);
            this.TXT_RProblema.TabIndex = 7;
            //
            // LBL_REstadoFisico
            //
            this.LBL_REstadoFisico.AutoSize = true;
            this.LBL_REstadoFisico.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_REstadoFisico.Location = new System.Drawing.Point(12, 156);
            this.LBL_REstadoFisico.Name = "LBL_REstadoFisico";
            this.LBL_REstadoFisico.Size = new System.Drawing.Size(76, 15);
            this.LBL_REstadoFisico.TabIndex = 8;
            this.LBL_REstadoFisico.Tag = "OrdenDetalle.EstadoFisico";
            this.LBL_REstadoFisico.Text = "Estado fisico:";
            //
            // TXT_REstadoFisico
            //
            this.TXT_REstadoFisico.Location = new System.Drawing.Point(150, 153);
            this.TXT_REstadoFisico.Name = "TXT_REstadoFisico";
            this.TXT_REstadoFisico.Size = new System.Drawing.Size(720, 22);
            this.TXT_REstadoFisico.TabIndex = 9;
            //
            // LBL_RAccesorios
            //
            this.LBL_RAccesorios.AutoSize = true;
            this.LBL_RAccesorios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RAccesorios.Location = new System.Drawing.Point(12, 184);
            this.LBL_RAccesorios.Name = "LBL_RAccesorios";
            this.LBL_RAccesorios.Size = new System.Drawing.Size(68, 15);
            this.LBL_RAccesorios.TabIndex = 10;
            this.LBL_RAccesorios.Tag = "OrdenDetalle.Accesorios";
            this.LBL_RAccesorios.Text = "Accesorios:";
            //
            // TXT_RAccesorios
            //
            this.TXT_RAccesorios.Location = new System.Drawing.Point(150, 181);
            this.TXT_RAccesorios.Name = "TXT_RAccesorios";
            this.TXT_RAccesorios.Size = new System.Drawing.Size(720, 22);
            this.TXT_RAccesorios.TabIndex = 11;
            //
            // LBL_RObs
            //
            this.LBL_RObs.AutoSize = true;
            this.LBL_RObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_RObs.Location = new System.Drawing.Point(12, 212);
            this.LBL_RObs.Name = "LBL_RObs";
            this.LBL_RObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_RObs.TabIndex = 12;
            this.LBL_RObs.Tag = "OrdenDetalle.ObsIngreso";
            this.LBL_RObs.Text = "Observaciones:";
            //
            // TXT_RObs
            //
            this.TXT_RObs.Location = new System.Drawing.Point(150, 209);
            this.TXT_RObs.Multiline = true;
            this.TXT_RObs.Name = "TXT_RObs";
            this.TXT_RObs.Size = new System.Drawing.Size(720, 60);
            this.TXT_RObs.TabIndex = 13;
            //
            // BTN_CrearOrden
            //
            this.BTN_CrearOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_CrearOrden.FlatAppearance.BorderSize = 0;
            this.BTN_CrearOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CrearOrden.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_CrearOrden.ForeColor = System.Drawing.Color.White;
            this.BTN_CrearOrden.Location = new System.Drawing.Point(150, 285);
            this.BTN_CrearOrden.Name = "BTN_CrearOrden";
            this.BTN_CrearOrden.Size = new System.Drawing.Size(140, 30);
            this.BTN_CrearOrden.TabIndex = 14;
            this.BTN_CrearOrden.Tag = "OrdenDetalle.CrearOrden";
            this.BTN_CrearOrden.Text = "Crear orden";
            this.BTN_CrearOrden.UseVisualStyleBackColor = false;
            this.BTN_CrearOrden.Click += new System.EventHandler(this.BTN_CrearOrden_Click);
            //
            // BTN_GuardarRecepcion
            //
            this.BTN_GuardarRecepcion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_GuardarRecepcion.FlatAppearance.BorderSize = 0;
            this.BTN_GuardarRecepcion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_GuardarRecepcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_GuardarRecepcion.ForeColor = System.Drawing.Color.White;
            this.BTN_GuardarRecepcion.Location = new System.Drawing.Point(300, 285);
            this.BTN_GuardarRecepcion.Name = "BTN_GuardarRecepcion";
            this.BTN_GuardarRecepcion.Size = new System.Drawing.Size(160, 30);
            this.BTN_GuardarRecepcion.TabIndex = 15;
            this.BTN_GuardarRecepcion.Tag = "OrdenDetalle.GuardarRecepcion";
            this.BTN_GuardarRecepcion.Text = "Guardar recepcion";
            this.BTN_GuardarRecepcion.UseVisualStyleBackColor = false;
            this.BTN_GuardarRecepcion.Click += new System.EventHandler(this.BTN_GuardarRecepcion_Click);
            //
            // BTN_AsignarTecnico
            //
            this.BTN_AsignarTecnico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.BTN_AsignarTecnico.FlatAppearance.BorderSize = 0;
            this.BTN_AsignarTecnico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_AsignarTecnico.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_AsignarTecnico.ForeColor = System.Drawing.Color.White;
            this.BTN_AsignarTecnico.Location = new System.Drawing.Point(470, 285);
            this.BTN_AsignarTecnico.Name = "BTN_AsignarTecnico";
            this.BTN_AsignarTecnico.Size = new System.Drawing.Size(140, 30);
            this.BTN_AsignarTecnico.TabIndex = 16;
            this.BTN_AsignarTecnico.Tag = "OrdenDetalle.AsignarTecnico";
            this.BTN_AsignarTecnico.Text = "Asignar tecnico";
            this.BTN_AsignarTecnico.UseVisualStyleBackColor = false;
            this.BTN_AsignarTecnico.Click += new System.EventHandler(this.BTN_AsignarTecnico_Click);
            //
            // TAB_Diagnostico
            //
            this.TAB_Diagnostico.Controls.Add(this.LBL_DDescripcion);
            this.TAB_Diagnostico.Controls.Add(this.TXT_DDescripcion);
            this.TAB_Diagnostico.Controls.Add(this.CHK_EsReparable);
            this.TAB_Diagnostico.Controls.Add(this.LBL_DDias);
            this.TAB_Diagnostico.Controls.Add(this.NUM_DDias);
            this.TAB_Diagnostico.Controls.Add(this.LBL_DObs);
            this.TAB_Diagnostico.Controls.Add(this.TXT_DObs);
            this.TAB_Diagnostico.Controls.Add(this.LBL_AvisoNoReparable);
            this.TAB_Diagnostico.Controls.Add(this.BTN_Iniciar);
            this.TAB_Diagnostico.Controls.Add(this.BTN_Finalizar);
            this.TAB_Diagnostico.Location = new System.Drawing.Point(4, 22);
            this.TAB_Diagnostico.Name = "TAB_Diagnostico";
            this.TAB_Diagnostico.Size = new System.Drawing.Size(888, 454);
            this.TAB_Diagnostico.TabIndex = 1;
            this.TAB_Diagnostico.Tag = "OrdenDetalle.TabDiagnostico";
            this.TAB_Diagnostico.Text = "Diagnostico";
            this.TAB_Diagnostico.UseVisualStyleBackColor = true;
            //
            // LBL_DDescripcion
            //
            this.LBL_DDescripcion.AutoSize = true;
            this.LBL_DDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_DDescripcion.Location = new System.Drawing.Point(12, 14);
            this.LBL_DDescripcion.Name = "LBL_DDescripcion";
            this.LBL_DDescripcion.Size = new System.Drawing.Size(72, 15);
            this.LBL_DDescripcion.TabIndex = 0;
            this.LBL_DDescripcion.Tag = "OrdenDetalle.Descripcion";
            this.LBL_DDescripcion.Text = "Descripcion:";
            //
            // TXT_DDescripcion
            //
            this.TXT_DDescripcion.Location = new System.Drawing.Point(150, 11);
            this.TXT_DDescripcion.Multiline = true;
            this.TXT_DDescripcion.Name = "TXT_DDescripcion";
            this.TXT_DDescripcion.Size = new System.Drawing.Size(720, 80);
            this.TXT_DDescripcion.TabIndex = 1;
            //
            // CHK_EsReparable
            //
            this.CHK_EsReparable.AutoSize = true;
            this.CHK_EsReparable.Checked = true;
            this.CHK_EsReparable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_EsReparable.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CHK_EsReparable.Location = new System.Drawing.Point(150, 100);
            this.CHK_EsReparable.Name = "CHK_EsReparable";
            this.CHK_EsReparable.Size = new System.Drawing.Size(95, 19);
            this.CHK_EsReparable.TabIndex = 2;
            this.CHK_EsReparable.Tag = "OrdenDetalle.EsReparable";
            this.CHK_EsReparable.Text = "Es reparable";
            this.CHK_EsReparable.UseVisualStyleBackColor = true;
            //
            // LBL_DDias
            //
            this.LBL_DDias.AutoSize = true;
            this.LBL_DDias.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_DDias.Location = new System.Drawing.Point(12, 128);
            this.LBL_DDias.Name = "LBL_DDias";
            this.LBL_DDias.Size = new System.Drawing.Size(118, 15);
            this.LBL_DDias.TabIndex = 3;
            this.LBL_DDias.Tag = "OrdenDetalle.Dias";
            this.LBL_DDias.Text = "Dias estimados (0=-):";
            //
            // NUM_DDias
            //
            this.NUM_DDias.Location = new System.Drawing.Point(150, 125);
            this.NUM_DDias.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.NUM_DDias.Name = "NUM_DDias";
            this.NUM_DDias.Size = new System.Drawing.Size(100, 22);
            this.NUM_DDias.TabIndex = 4;
            //
            // LBL_DObs
            //
            this.LBL_DObs.AutoSize = true;
            this.LBL_DObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_DObs.Location = new System.Drawing.Point(12, 156);
            this.LBL_DObs.Name = "LBL_DObs";
            this.LBL_DObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_DObs.TabIndex = 5;
            this.LBL_DObs.Tag = "OrdenDetalle.ObsDiagnostico";
            this.LBL_DObs.Text = "Observaciones:";
            //
            // TXT_DObs
            //
            this.TXT_DObs.Location = new System.Drawing.Point(150, 153);
            this.TXT_DObs.Multiline = true;
            this.TXT_DObs.Name = "TXT_DObs";
            this.TXT_DObs.Size = new System.Drawing.Size(720, 60);
            this.TXT_DObs.TabIndex = 6;
            //
            // LBL_AvisoNoReparable
            //
            this.LBL_AvisoNoReparable.AutoSize = true;
            this.LBL_AvisoNoReparable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_AvisoNoReparable.ForeColor = System.Drawing.Color.Maroon;
            this.LBL_AvisoNoReparable.Location = new System.Drawing.Point(147, 222);
            this.LBL_AvisoNoReparable.Name = "LBL_AvisoNoReparable";
            this.LBL_AvisoNoReparable.Size = new System.Drawing.Size(200, 15);
            this.LBL_AvisoNoReparable.TabIndex = 7;
            this.LBL_AvisoNoReparable.Tag = "OrdenDetalle.AvisoNoReparable";
            this.LBL_AvisoNoReparable.Text = "Equipo declarado no reparable.";
            this.LBL_AvisoNoReparable.Visible = false;
            //
            // BTN_Iniciar
            //
            this.BTN_Iniciar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Iniciar.FlatAppearance.BorderSize = 0;
            this.BTN_Iniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Iniciar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Iniciar.ForeColor = System.Drawing.Color.White;
            this.BTN_Iniciar.Location = new System.Drawing.Point(150, 250);
            this.BTN_Iniciar.Name = "BTN_Iniciar";
            this.BTN_Iniciar.Size = new System.Drawing.Size(160, 30);
            this.BTN_Iniciar.TabIndex = 8;
            this.BTN_Iniciar.Tag = "OrdenDetalle.IniciarDiagnostico";
            this.BTN_Iniciar.Text = "Iniciar diagnostico";
            this.BTN_Iniciar.UseVisualStyleBackColor = false;
            this.BTN_Iniciar.Click += new System.EventHandler(this.BTN_Iniciar_Click);
            //
            // BTN_Finalizar
            //
            this.BTN_Finalizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Finalizar.FlatAppearance.BorderSize = 0;
            this.BTN_Finalizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Finalizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Finalizar.ForeColor = System.Drawing.Color.White;
            this.BTN_Finalizar.Location = new System.Drawing.Point(320, 250);
            this.BTN_Finalizar.Name = "BTN_Finalizar";
            this.BTN_Finalizar.Size = new System.Drawing.Size(160, 30);
            this.BTN_Finalizar.TabIndex = 9;
            this.BTN_Finalizar.Tag = "OrdenDetalle.FinalizarDiagnostico";
            this.BTN_Finalizar.Text = "Finalizar diagnostico";
            this.BTN_Finalizar.UseVisualStyleBackColor = false;
            this.BTN_Finalizar.Click += new System.EventHandler(this.BTN_Finalizar_Click);
            //
            // TAB_Presupuesto
            //
            this.TAB_Presupuesto.Controls.Add(this.DGV_Detalle);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PTipo);
            this.TAB_Presupuesto.Controls.Add(this.CBO_PTipo);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PDesc);
            this.TAB_Presupuesto.Controls.Add(this.TXT_PDesc);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PCant);
            this.TAB_Presupuesto.Controls.Add(this.NUM_PCant);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PPrecio);
            this.TAB_Presupuesto.Controls.Add(this.NUM_PPrecio);
            this.TAB_Presupuesto.Controls.Add(this.BTN_AgregarItem);
            this.TAB_Presupuesto.Controls.Add(this.BTN_QuitarItem);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PDescuento);
            this.TAB_Presupuesto.Controls.Add(this.NUM_PDescuento);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PGarantia);
            this.TAB_Presupuesto.Controls.Add(this.NUM_PGarantia);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PMedio);
            this.TAB_Presupuesto.Controls.Add(this.TXT_PMedio);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PMotivo);
            this.TAB_Presupuesto.Controls.Add(this.TXT_PMotivo);
            this.TAB_Presupuesto.Controls.Add(this.LBL_PObs);
            this.TAB_Presupuesto.Controls.Add(this.TXT_PObs);
            this.TAB_Presupuesto.Controls.Add(this.LBL_Totales);
            this.TAB_Presupuesto.Controls.Add(this.LBL_EstadoPresupuesto);
            this.TAB_Presupuesto.Controls.Add(this.BTN_Emitir);
            this.TAB_Presupuesto.Controls.Add(this.BTN_Aprobar);
            this.TAB_Presupuesto.Controls.Add(this.BTN_Rechazar);
            this.TAB_Presupuesto.Location = new System.Drawing.Point(4, 22);
            this.TAB_Presupuesto.Name = "TAB_Presupuesto";
            this.TAB_Presupuesto.Size = new System.Drawing.Size(888, 454);
            this.TAB_Presupuesto.TabIndex = 2;
            this.TAB_Presupuesto.Tag = "OrdenDetalle.TabPresupuesto";
            this.TAB_Presupuesto.Text = "Presupuesto";
            this.TAB_Presupuesto.UseVisualStyleBackColor = true;
            //
            // DGV_Detalle
            //
            this.DGV_Detalle.AllowUserToAddRows = false;
            this.DGV_Detalle.AllowUserToDeleteRows = false;
            this.DGV_Detalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Detalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Detalle.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Detalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Detalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Detalle.Location = new System.Drawing.Point(12, 8);
            this.DGV_Detalle.MultiSelect = false;
            this.DGV_Detalle.Name = "DGV_Detalle";
            this.DGV_Detalle.ReadOnly = true;
            this.DGV_Detalle.RowHeadersVisible = false;
            this.DGV_Detalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Detalle.Size = new System.Drawing.Size(864, 160);
            this.DGV_Detalle.TabIndex = 0;
            this.DGV_Detalle.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_Detalle_CellFormatting);
            //
            // LBL_PTipo
            //
            this.LBL_PTipo.AutoSize = true;
            this.LBL_PTipo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PTipo.Location = new System.Drawing.Point(12, 178);
            this.LBL_PTipo.Name = "LBL_PTipo";
            this.LBL_PTipo.Size = new System.Drawing.Size(33, 15);
            this.LBL_PTipo.TabIndex = 1;
            this.LBL_PTipo.Tag = "OrdenDetalle.TipoItem";
            this.LBL_PTipo.Text = "Tipo:";
            //
            // CBO_PTipo
            //
            this.CBO_PTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_PTipo.Location = new System.Drawing.Point(70, 175);
            this.CBO_PTipo.Name = "CBO_PTipo";
            this.CBO_PTipo.Size = new System.Drawing.Size(130, 21);
            this.CBO_PTipo.TabIndex = 2;
            //
            // LBL_PDesc
            //
            this.LBL_PDesc.AutoSize = true;
            this.LBL_PDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PDesc.Location = new System.Drawing.Point(210, 178);
            this.LBL_PDesc.Name = "LBL_PDesc";
            this.LBL_PDesc.Size = new System.Drawing.Size(72, 15);
            this.LBL_PDesc.TabIndex = 3;
            this.LBL_PDesc.Tag = "OrdenDetalle.DescripcionItem";
            this.LBL_PDesc.Text = "Descripcion:";
            //
            // TXT_PDesc
            //
            this.TXT_PDesc.Location = new System.Drawing.Point(288, 175);
            this.TXT_PDesc.Name = "TXT_PDesc";
            this.TXT_PDesc.Size = new System.Drawing.Size(270, 22);
            this.TXT_PDesc.TabIndex = 4;
            //
            // LBL_PCant
            //
            this.LBL_PCant.AutoSize = true;
            this.LBL_PCant.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PCant.Location = new System.Drawing.Point(568, 178);
            this.LBL_PCant.Name = "LBL_PCant";
            this.LBL_PCant.Size = new System.Drawing.Size(58, 15);
            this.LBL_PCant.TabIndex = 5;
            this.LBL_PCant.Tag = "OrdenDetalle.Cantidad";
            this.LBL_PCant.Text = "Cantidad:";
            //
            // NUM_PCant
            //
            this.NUM_PCant.Location = new System.Drawing.Point(632, 175);
            this.NUM_PCant.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NUM_PCant.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_PCant.Name = "NUM_PCant";
            this.NUM_PCant.Size = new System.Drawing.Size(60, 22);
            this.NUM_PCant.TabIndex = 6;
            this.NUM_PCant.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // LBL_PPrecio
            //
            this.LBL_PPrecio.AutoSize = true;
            this.LBL_PPrecio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PPrecio.Location = new System.Drawing.Point(702, 178);
            this.LBL_PPrecio.Name = "LBL_PPrecio";
            this.LBL_PPrecio.Size = new System.Drawing.Size(43, 15);
            this.LBL_PPrecio.TabIndex = 7;
            this.LBL_PPrecio.Tag = "OrdenDetalle.Precio";
            this.LBL_PPrecio.Text = "Precio:";
            //
            // NUM_PPrecio
            //
            this.NUM_PPrecio.DecimalPlaces = 2;
            this.NUM_PPrecio.Location = new System.Drawing.Point(751, 175);
            this.NUM_PPrecio.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUM_PPrecio.Name = "NUM_PPrecio";
            this.NUM_PPrecio.Size = new System.Drawing.Size(125, 22);
            this.NUM_PPrecio.TabIndex = 8;
            //
            // BTN_AgregarItem
            //
            this.BTN_AgregarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_AgregarItem.FlatAppearance.BorderSize = 0;
            this.BTN_AgregarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_AgregarItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_AgregarItem.ForeColor = System.Drawing.Color.White;
            this.BTN_AgregarItem.Location = new System.Drawing.Point(12, 206);
            this.BTN_AgregarItem.Name = "BTN_AgregarItem";
            this.BTN_AgregarItem.Size = new System.Drawing.Size(110, 28);
            this.BTN_AgregarItem.TabIndex = 9;
            this.BTN_AgregarItem.Tag = "OrdenDetalle.AgregarItem";
            this.BTN_AgregarItem.Text = "Agregar item";
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
            this.BTN_QuitarItem.Location = new System.Drawing.Point(132, 206);
            this.BTN_QuitarItem.Name = "BTN_QuitarItem";
            this.BTN_QuitarItem.Size = new System.Drawing.Size(110, 28);
            this.BTN_QuitarItem.TabIndex = 10;
            this.BTN_QuitarItem.Tag = "OrdenDetalle.QuitarItem";
            this.BTN_QuitarItem.Text = "Quitar item";
            this.BTN_QuitarItem.UseVisualStyleBackColor = false;
            this.BTN_QuitarItem.Click += new System.EventHandler(this.BTN_QuitarItem_Click);
            //
            // LBL_PDescuento
            //
            this.LBL_PDescuento.AutoSize = true;
            this.LBL_PDescuento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PDescuento.Location = new System.Drawing.Point(12, 246);
            this.LBL_PDescuento.Name = "LBL_PDescuento";
            this.LBL_PDescuento.Size = new System.Drawing.Size(67, 15);
            this.LBL_PDescuento.TabIndex = 11;
            this.LBL_PDescuento.Tag = "OrdenDetalle.Descuento";
            this.LBL_PDescuento.Text = "Descuento:";
            //
            // NUM_PDescuento
            //
            this.NUM_PDescuento.DecimalPlaces = 2;
            this.NUM_PDescuento.Location = new System.Drawing.Point(100, 243);
            this.NUM_PDescuento.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.NUM_PDescuento.Name = "NUM_PDescuento";
            this.NUM_PDescuento.Size = new System.Drawing.Size(120, 22);
            this.NUM_PDescuento.TabIndex = 12;
            this.NUM_PDescuento.ValueChanged += new System.EventHandler(this.NUM_PDescuento_ValueChanged);
            //
            // LBL_PGarantia
            //
            this.LBL_PGarantia.AutoSize = true;
            this.LBL_PGarantia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PGarantia.Location = new System.Drawing.Point(235, 246);
            this.LBL_PGarantia.Name = "LBL_PGarantia";
            this.LBL_PGarantia.Size = new System.Drawing.Size(88, 15);
            this.LBL_PGarantia.TabIndex = 13;
            this.LBL_PGarantia.Tag = "OrdenDetalle.Garantia";
            this.LBL_PGarantia.Text = "Garantia (dias):";
            //
            // NUM_PGarantia
            //
            this.NUM_PGarantia.Location = new System.Drawing.Point(329, 243);
            this.NUM_PGarantia.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.NUM_PGarantia.Name = "NUM_PGarantia";
            this.NUM_PGarantia.Size = new System.Drawing.Size(70, 22);
            this.NUM_PGarantia.TabIndex = 14;
            //
            // LBL_PMedio
            //
            this.LBL_PMedio.AutoSize = true;
            this.LBL_PMedio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PMedio.Location = new System.Drawing.Point(414, 246);
            this.LBL_PMedio.Name = "LBL_PMedio";
            this.LBL_PMedio.Size = new System.Drawing.Size(45, 15);
            this.LBL_PMedio.TabIndex = 15;
            this.LBL_PMedio.Tag = "OrdenDetalle.Medio";
            this.LBL_PMedio.Text = "Medio:";
            //
            // TXT_PMedio
            //
            this.TXT_PMedio.Location = new System.Drawing.Point(465, 243);
            this.TXT_PMedio.Name = "TXT_PMedio";
            this.TXT_PMedio.Size = new System.Drawing.Size(150, 22);
            this.TXT_PMedio.TabIndex = 16;
            //
            // LBL_PMotivo
            //
            this.LBL_PMotivo.AutoSize = true;
            this.LBL_PMotivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PMotivo.Location = new System.Drawing.Point(625, 246);
            this.LBL_PMotivo.Name = "LBL_PMotivo";
            this.LBL_PMotivo.Size = new System.Drawing.Size(48, 15);
            this.LBL_PMotivo.TabIndex = 17;
            this.LBL_PMotivo.Tag = "OrdenDetalle.Motivo";
            this.LBL_PMotivo.Text = "Motivo:";
            //
            // TXT_PMotivo
            //
            this.TXT_PMotivo.Location = new System.Drawing.Point(679, 243);
            this.TXT_PMotivo.Name = "TXT_PMotivo";
            this.TXT_PMotivo.Size = new System.Drawing.Size(197, 22);
            this.TXT_PMotivo.TabIndex = 18;
            //
            // LBL_PObs
            //
            this.LBL_PObs.AutoSize = true;
            this.LBL_PObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PObs.Location = new System.Drawing.Point(12, 276);
            this.LBL_PObs.Name = "LBL_PObs";
            this.LBL_PObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_PObs.TabIndex = 19;
            this.LBL_PObs.Tag = "OrdenDetalle.ObsPresupuesto";
            this.LBL_PObs.Text = "Observaciones:";
            //
            // TXT_PObs
            //
            this.TXT_PObs.Location = new System.Drawing.Point(100, 273);
            this.TXT_PObs.Multiline = true;
            this.TXT_PObs.Name = "TXT_PObs";
            this.TXT_PObs.Size = new System.Drawing.Size(776, 44);
            this.TXT_PObs.TabIndex = 20;
            //
            // LBL_Totales
            //
            this.LBL_Totales.AutoSize = true;
            this.LBL_Totales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Totales.Location = new System.Drawing.Point(12, 326);
            this.LBL_Totales.Name = "LBL_Totales";
            this.LBL_Totales.Size = new System.Drawing.Size(90, 15);
            this.LBL_Totales.TabIndex = 21;
            this.LBL_Totales.Text = "Subtotal: 0";
            //
            // LBL_EstadoPresupuesto
            //
            this.LBL_EstadoPresupuesto.AutoSize = true;
            this.LBL_EstadoPresupuesto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_EstadoPresupuesto.Location = new System.Drawing.Point(12, 348);
            this.LBL_EstadoPresupuesto.Name = "LBL_EstadoPresupuesto";
            this.LBL_EstadoPresupuesto.Size = new System.Drawing.Size(80, 15);
            this.LBL_EstadoPresupuesto.TabIndex = 22;
            this.LBL_EstadoPresupuesto.Text = "Presupuesto: -";
            //
            // BTN_Emitir
            //
            this.BTN_Emitir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Emitir.FlatAppearance.BorderSize = 0;
            this.BTN_Emitir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Emitir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Emitir.ForeColor = System.Drawing.Color.White;
            this.BTN_Emitir.Location = new System.Drawing.Point(12, 376);
            this.BTN_Emitir.Name = "BTN_Emitir";
            this.BTN_Emitir.Size = new System.Drawing.Size(130, 30);
            this.BTN_Emitir.TabIndex = 23;
            this.BTN_Emitir.Tag = "OrdenDetalle.Emitir";
            this.BTN_Emitir.Text = "Emitir";
            this.BTN_Emitir.UseVisualStyleBackColor = false;
            this.BTN_Emitir.Click += new System.EventHandler(this.BTN_Emitir_Click);
            //
            // BTN_Aprobar
            //
            this.BTN_Aprobar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Aprobar.FlatAppearance.BorderSize = 0;
            this.BTN_Aprobar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Aprobar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Aprobar.ForeColor = System.Drawing.Color.White;
            this.BTN_Aprobar.Location = new System.Drawing.Point(152, 376);
            this.BTN_Aprobar.Name = "BTN_Aprobar";
            this.BTN_Aprobar.Size = new System.Drawing.Size(130, 30);
            this.BTN_Aprobar.TabIndex = 24;
            this.BTN_Aprobar.Tag = "OrdenDetalle.Aprobar";
            this.BTN_Aprobar.Text = "Aprobar";
            this.BTN_Aprobar.UseVisualStyleBackColor = false;
            this.BTN_Aprobar.Click += new System.EventHandler(this.BTN_Aprobar_Click);
            //
            // BTN_Rechazar
            //
            this.BTN_Rechazar.BackColor = System.Drawing.Color.Maroon;
            this.BTN_Rechazar.FlatAppearance.BorderSize = 0;
            this.BTN_Rechazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Rechazar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Rechazar.ForeColor = System.Drawing.Color.White;
            this.BTN_Rechazar.Location = new System.Drawing.Point(292, 376);
            this.BTN_Rechazar.Name = "BTN_Rechazar";
            this.BTN_Rechazar.Size = new System.Drawing.Size(130, 30);
            this.BTN_Rechazar.TabIndex = 25;
            this.BTN_Rechazar.Tag = "OrdenDetalle.Rechazar";
            this.BTN_Rechazar.Text = "Rechazar";
            this.BTN_Rechazar.UseVisualStyleBackColor = false;
            this.BTN_Rechazar.Click += new System.EventHandler(this.BTN_Rechazar_Click);
            //
            // TAB_Historial
            //
            this.TAB_Historial.Controls.Add(this.DGV_Historial);
            this.TAB_Historial.Location = new System.Drawing.Point(4, 22);
            this.TAB_Historial.Name = "TAB_Historial";
            this.TAB_Historial.Size = new System.Drawing.Size(888, 454);
            this.TAB_Historial.TabIndex = 5;
            this.TAB_Historial.Tag = "OrdenDetalle.TabHistorial";
            this.TAB_Historial.Text = "Historial";
            this.TAB_Historial.UseVisualStyleBackColor = true;
            //
            // DGV_Historial
            //
            this.DGV_Historial.AllowUserToAddRows = false;
            this.DGV_Historial.AllowUserToDeleteRows = false;
            this.DGV_Historial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Historial.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Historial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Historial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Historial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_Historial.Location = new System.Drawing.Point(0, 0);
            this.DGV_Historial.MultiSelect = false;
            this.DGV_Historial.Name = "DGV_Historial";
            this.DGV_Historial.ReadOnly = true;
            this.DGV_Historial.RowHeadersVisible = false;
            this.DGV_Historial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Historial.Size = new System.Drawing.Size(888, 454);
            this.DGV_Historial.TabIndex = 0;
            //
            // TAB_Reparaciones
            //
            this.TAB_Reparaciones.Controls.Add(this.DGV_Reparaciones);
            this.TAB_Reparaciones.Controls.Add(this.BTN_IniciarReparacion);
            this.TAB_Reparaciones.Controls.Add(this.LBL_CRepuesto);
            this.TAB_Reparaciones.Controls.Add(this.CBO_ConsumoRepuesto);
            this.TAB_Reparaciones.Controls.Add(this.LBL_CCantidad);
            this.TAB_Reparaciones.Controls.Add(this.NUM_ConsumoCantidad);
            this.TAB_Reparaciones.Controls.Add(this.BTN_Consumir);
            this.TAB_Reparaciones.Controls.Add(this.LBL_Consumidos);
            this.TAB_Reparaciones.Controls.Add(this.DGV_Consumidos);
            this.TAB_Reparaciones.Controls.Add(this.LBL_CostoTotal);
            this.TAB_Reparaciones.Controls.Add(this.LBL_FTrabajo);
            this.TAB_Reparaciones.Controls.Add(this.TXT_FTrabajo);
            this.TAB_Reparaciones.Controls.Add(this.LBL_FObs);
            this.TAB_Reparaciones.Controls.Add(this.TXT_FObs);
            this.TAB_Reparaciones.Controls.Add(this.BTN_FinalizarReparacion);
            this.TAB_Reparaciones.Location = new System.Drawing.Point(4, 22);
            this.TAB_Reparaciones.Name = "TAB_Reparaciones";
            this.TAB_Reparaciones.Size = new System.Drawing.Size(888, 454);
            this.TAB_Reparaciones.TabIndex = 3;
            this.TAB_Reparaciones.Tag = "OrdenDetalle.TabReparaciones";
            this.TAB_Reparaciones.Text = "Reparaciones";
            this.TAB_Reparaciones.UseVisualStyleBackColor = true;
            //
            // DGV_Reparaciones
            //
            this.DGV_Reparaciones.AllowUserToAddRows = false;
            this.DGV_Reparaciones.AllowUserToDeleteRows = false;
            this.DGV_Reparaciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Reparaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Reparaciones.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Reparaciones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Reparaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Reparaciones.Location = new System.Drawing.Point(12, 8);
            this.DGV_Reparaciones.MultiSelect = false;
            this.DGV_Reparaciones.Name = "DGV_Reparaciones";
            this.DGV_Reparaciones.ReadOnly = true;
            this.DGV_Reparaciones.RowHeadersVisible = false;
            this.DGV_Reparaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Reparaciones.Size = new System.Drawing.Size(864, 120);
            this.DGV_Reparaciones.TabIndex = 0;
            this.DGV_Reparaciones.SelectionChanged += new System.EventHandler(this.DGV_Reparaciones_SelectionChanged);
            //
            // BTN_IniciarReparacion
            //
            this.BTN_IniciarReparacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_IniciarReparacion.FlatAppearance.BorderSize = 0;
            this.BTN_IniciarReparacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_IniciarReparacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_IniciarReparacion.ForeColor = System.Drawing.Color.White;
            this.BTN_IniciarReparacion.Location = new System.Drawing.Point(12, 136);
            this.BTN_IniciarReparacion.Name = "BTN_IniciarReparacion";
            this.BTN_IniciarReparacion.Size = new System.Drawing.Size(160, 28);
            this.BTN_IniciarReparacion.TabIndex = 1;
            this.BTN_IniciarReparacion.Tag = "OrdenDetalle.IniciarReparacion";
            this.BTN_IniciarReparacion.Text = "Iniciar reparacion";
            this.BTN_IniciarReparacion.UseVisualStyleBackColor = false;
            this.BTN_IniciarReparacion.Click += new System.EventHandler(this.BTN_IniciarReparacion_Click);
            //
            // LBL_CRepuesto
            //
            this.LBL_CRepuesto.AutoSize = true;
            this.LBL_CRepuesto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_CRepuesto.Location = new System.Drawing.Point(185, 143);
            this.LBL_CRepuesto.Name = "LBL_CRepuesto";
            this.LBL_CRepuesto.Size = new System.Drawing.Size(61, 15);
            this.LBL_CRepuesto.TabIndex = 2;
            this.LBL_CRepuesto.Tag = "OrdenDetalle.Repuesto";
            this.LBL_CRepuesto.Text = "Repuesto:";
            //
            // CBO_ConsumoRepuesto
            //
            this.CBO_ConsumoRepuesto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_ConsumoRepuesto.Location = new System.Drawing.Point(252, 140);
            this.CBO_ConsumoRepuesto.Name = "CBO_ConsumoRepuesto";
            this.CBO_ConsumoRepuesto.Size = new System.Drawing.Size(230, 21);
            this.CBO_ConsumoRepuesto.TabIndex = 3;
            //
            // LBL_CCantidad
            //
            this.LBL_CCantidad.AutoSize = true;
            this.LBL_CCantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_CCantidad.Location = new System.Drawing.Point(492, 143);
            this.LBL_CCantidad.Name = "LBL_CCantidad";
            this.LBL_CCantidad.Size = new System.Drawing.Size(58, 15);
            this.LBL_CCantidad.TabIndex = 4;
            this.LBL_CCantidad.Tag = "OrdenDetalle.Cantidad";
            this.LBL_CCantidad.Text = "Cantidad:";
            //
            // NUM_ConsumoCantidad
            //
            this.NUM_ConsumoCantidad.Location = new System.Drawing.Point(556, 140);
            this.NUM_ConsumoCantidad.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUM_ConsumoCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUM_ConsumoCantidad.Name = "NUM_ConsumoCantidad";
            this.NUM_ConsumoCantidad.Size = new System.Drawing.Size(60, 22);
            this.NUM_ConsumoCantidad.TabIndex = 5;
            this.NUM_ConsumoCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // BTN_Consumir
            //
            this.BTN_Consumir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.BTN_Consumir.FlatAppearance.BorderSize = 0;
            this.BTN_Consumir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Consumir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Consumir.ForeColor = System.Drawing.Color.White;
            this.BTN_Consumir.Location = new System.Drawing.Point(626, 136);
            this.BTN_Consumir.Name = "BTN_Consumir";
            this.BTN_Consumir.Size = new System.Drawing.Size(110, 28);
            this.BTN_Consumir.TabIndex = 6;
            this.BTN_Consumir.Tag = "OrdenDetalle.Consumir";
            this.BTN_Consumir.Text = "Consumir";
            this.BTN_Consumir.UseVisualStyleBackColor = false;
            this.BTN_Consumir.Click += new System.EventHandler(this.BTN_Consumir_Click);
            //
            // LBL_Consumidos
            //
            this.LBL_Consumidos.AutoSize = true;
            this.LBL_Consumidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_Consumidos.Location = new System.Drawing.Point(12, 174);
            this.LBL_Consumidos.Name = "LBL_Consumidos";
            this.LBL_Consumidos.Size = new System.Drawing.Size(80, 15);
            this.LBL_Consumidos.TabIndex = 7;
            this.LBL_Consumidos.Tag = "OrdenDetalle.Consumidos";
            this.LBL_Consumidos.Text = "Consumidos";
            //
            // DGV_Consumidos
            //
            this.DGV_Consumidos.AllowUserToAddRows = false;
            this.DGV_Consumidos.AllowUserToDeleteRows = false;
            this.DGV_Consumidos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Consumidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Consumidos.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Consumidos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Consumidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Consumidos.Location = new System.Drawing.Point(12, 194);
            this.DGV_Consumidos.MultiSelect = false;
            this.DGV_Consumidos.Name = "DGV_Consumidos";
            this.DGV_Consumidos.ReadOnly = true;
            this.DGV_Consumidos.RowHeadersVisible = false;
            this.DGV_Consumidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Consumidos.Size = new System.Drawing.Size(864, 105);
            this.DGV_Consumidos.TabIndex = 8;
            //
            // LBL_CostoTotal
            //
            this.LBL_CostoTotal.AutoSize = true;
            this.LBL_CostoTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.LBL_CostoTotal.Location = new System.Drawing.Point(12, 306);
            this.LBL_CostoTotal.Name = "LBL_CostoTotal";
            this.LBL_CostoTotal.Size = new System.Drawing.Size(90, 15);
            this.LBL_CostoTotal.TabIndex = 9;
            this.LBL_CostoTotal.Text = "Costo total: 0";
            //
            // LBL_FTrabajo
            //
            this.LBL_FTrabajo.AutoSize = true;
            this.LBL_FTrabajo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_FTrabajo.Location = new System.Drawing.Point(12, 332);
            this.LBL_FTrabajo.Name = "LBL_FTrabajo";
            this.LBL_FTrabajo.Size = new System.Drawing.Size(110, 15);
            this.LBL_FTrabajo.TabIndex = 10;
            this.LBL_FTrabajo.Tag = "OrdenDetalle.Trabajo";
            this.LBL_FTrabajo.Text = "Trabajo realizado:";
            //
            // TXT_FTrabajo
            //
            this.TXT_FTrabajo.Location = new System.Drawing.Point(130, 329);
            this.TXT_FTrabajo.Multiline = true;
            this.TXT_FTrabajo.Name = "TXT_FTrabajo";
            this.TXT_FTrabajo.Size = new System.Drawing.Size(380, 45);
            this.TXT_FTrabajo.TabIndex = 11;
            //
            // LBL_FObs
            //
            this.LBL_FObs.AutoSize = true;
            this.LBL_FObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_FObs.Location = new System.Drawing.Point(520, 332);
            this.LBL_FObs.Name = "LBL_FObs";
            this.LBL_FObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_FObs.TabIndex = 12;
            this.LBL_FObs.Tag = "OrdenDetalle.ObsReparacion";
            this.LBL_FObs.Text = "Observaciones:";
            //
            // TXT_FObs
            //
            this.TXT_FObs.Location = new System.Drawing.Point(613, 329);
            this.TXT_FObs.Multiline = true;
            this.TXT_FObs.Name = "TXT_FObs";
            this.TXT_FObs.Size = new System.Drawing.Size(263, 45);
            this.TXT_FObs.TabIndex = 13;
            //
            // BTN_FinalizarReparacion
            //
            this.BTN_FinalizarReparacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.BTN_FinalizarReparacion.FlatAppearance.BorderSize = 0;
            this.BTN_FinalizarReparacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_FinalizarReparacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_FinalizarReparacion.ForeColor = System.Drawing.Color.White;
            this.BTN_FinalizarReparacion.Location = new System.Drawing.Point(12, 384);
            this.BTN_FinalizarReparacion.Name = "BTN_FinalizarReparacion";
            this.BTN_FinalizarReparacion.Size = new System.Drawing.Size(160, 30);
            this.BTN_FinalizarReparacion.TabIndex = 14;
            this.BTN_FinalizarReparacion.Tag = "OrdenDetalle.FinalizarReparacion";
            this.BTN_FinalizarReparacion.Text = "Finalizar reparacion";
            this.BTN_FinalizarReparacion.UseVisualStyleBackColor = false;
            this.BTN_FinalizarReparacion.Click += new System.EventHandler(this.BTN_FinalizarReparacion_Click);
            //
            // TAB_Pruebas
            //
            this.TAB_Pruebas.Controls.Add(this.LBL_PIntervencion);
            this.TAB_Pruebas.Controls.Add(this.CBO_PruebaReparacion);
            this.TAB_Pruebas.Controls.Add(this.DGV_Pruebas);
            this.TAB_Pruebas.Controls.Add(this.LBL_PDDesc);
            this.TAB_Pruebas.Controls.Add(this.TXT_PruebaDesc);
            this.TAB_Pruebas.Controls.Add(this.RDO_Aprobada);
            this.TAB_Pruebas.Controls.Add(this.RDO_Fallida);
            this.TAB_Pruebas.Controls.Add(this.LBL_PDObs);
            this.TAB_Pruebas.Controls.Add(this.TXT_PruebaObs);
            this.TAB_Pruebas.Controls.Add(this.BTN_RegistrarPrueba);
            this.TAB_Pruebas.Location = new System.Drawing.Point(4, 22);
            this.TAB_Pruebas.Name = "TAB_Pruebas";
            this.TAB_Pruebas.Size = new System.Drawing.Size(888, 454);
            this.TAB_Pruebas.TabIndex = 4;
            this.TAB_Pruebas.Tag = "OrdenDetalle.TabPruebas";
            this.TAB_Pruebas.Text = "Pruebas";
            this.TAB_Pruebas.UseVisualStyleBackColor = true;
            //
            // LBL_PIntervencion
            //
            this.LBL_PIntervencion.AutoSize = true;
            this.LBL_PIntervencion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PIntervencion.Location = new System.Drawing.Point(12, 14);
            this.LBL_PIntervencion.Name = "LBL_PIntervencion";
            this.LBL_PIntervencion.Size = new System.Drawing.Size(76, 15);
            this.LBL_PIntervencion.TabIndex = 0;
            this.LBL_PIntervencion.Tag = "OrdenDetalle.Intervencion";
            this.LBL_PIntervencion.Text = "Intervencion:";
            //
            // CBO_PruebaReparacion
            //
            this.CBO_PruebaReparacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBO_PruebaReparacion.Location = new System.Drawing.Point(150, 11);
            this.CBO_PruebaReparacion.Name = "CBO_PruebaReparacion";
            this.CBO_PruebaReparacion.Size = new System.Drawing.Size(250, 21);
            this.CBO_PruebaReparacion.TabIndex = 1;
            this.CBO_PruebaReparacion.SelectedIndexChanged += new System.EventHandler(this.CBO_PruebaReparacion_SelectedIndexChanged);
            //
            // DGV_Pruebas
            //
            this.DGV_Pruebas.AllowUserToAddRows = false;
            this.DGV_Pruebas.AllowUserToDeleteRows = false;
            this.DGV_Pruebas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Pruebas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Pruebas.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Pruebas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Pruebas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Pruebas.Location = new System.Drawing.Point(12, 42);
            this.DGV_Pruebas.MultiSelect = false;
            this.DGV_Pruebas.Name = "DGV_Pruebas";
            this.DGV_Pruebas.ReadOnly = true;
            this.DGV_Pruebas.RowHeadersVisible = false;
            this.DGV_Pruebas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Pruebas.Size = new System.Drawing.Size(864, 180);
            this.DGV_Pruebas.TabIndex = 2;
            //
            // LBL_PDDesc
            //
            this.LBL_PDDesc.AutoSize = true;
            this.LBL_PDDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PDDesc.Location = new System.Drawing.Point(12, 234);
            this.LBL_PDDesc.Name = "LBL_PDDesc";
            this.LBL_PDDesc.Size = new System.Drawing.Size(72, 15);
            this.LBL_PDDesc.TabIndex = 3;
            this.LBL_PDDesc.Tag = "OrdenDetalle.DescripcionItem";
            this.LBL_PDDesc.Text = "Descripcion:";
            //
            // TXT_PruebaDesc
            //
            this.TXT_PruebaDesc.Location = new System.Drawing.Point(150, 231);
            this.TXT_PruebaDesc.Multiline = true;
            this.TXT_PruebaDesc.Name = "TXT_PruebaDesc";
            this.TXT_PruebaDesc.Size = new System.Drawing.Size(726, 44);
            this.TXT_PruebaDesc.TabIndex = 4;
            //
            // RDO_Aprobada
            //
            this.RDO_Aprobada.AutoSize = true;
            this.RDO_Aprobada.Checked = true;
            this.RDO_Aprobada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RDO_Aprobada.Location = new System.Drawing.Point(150, 284);
            this.RDO_Aprobada.Name = "RDO_Aprobada";
            this.RDO_Aprobada.Size = new System.Drawing.Size(80, 19);
            this.RDO_Aprobada.TabIndex = 5;
            this.RDO_Aprobada.TabStop = true;
            this.RDO_Aprobada.Tag = "OrdenDetalle.Aprobada";
            this.RDO_Aprobada.Text = "Aprobada";
            this.RDO_Aprobada.UseVisualStyleBackColor = true;
            //
            // RDO_Fallida
            //
            this.RDO_Fallida.AutoSize = true;
            this.RDO_Fallida.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.RDO_Fallida.Location = new System.Drawing.Point(270, 284);
            this.RDO_Fallida.Name = "RDO_Fallida";
            this.RDO_Fallida.Size = new System.Drawing.Size(120, 19);
            this.RDO_Fallida.TabIndex = 6;
            this.RDO_Fallida.Tag = "OrdenDetalle.Fallida";
            this.RDO_Fallida.Text = "Requiere revision";
            this.RDO_Fallida.UseVisualStyleBackColor = true;
            //
            // LBL_PDObs
            //
            this.LBL_PDObs.AutoSize = true;
            this.LBL_PDObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_PDObs.Location = new System.Drawing.Point(12, 314);
            this.LBL_PDObs.Name = "LBL_PDObs";
            this.LBL_PDObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_PDObs.TabIndex = 7;
            this.LBL_PDObs.Tag = "OrdenDetalle.ObsPrueba";
            this.LBL_PDObs.Text = "Observaciones:";
            //
            // TXT_PruebaObs
            //
            this.TXT_PruebaObs.Location = new System.Drawing.Point(150, 311);
            this.TXT_PruebaObs.Multiline = true;
            this.TXT_PruebaObs.Name = "TXT_PruebaObs";
            this.TXT_PruebaObs.Size = new System.Drawing.Size(726, 60);
            this.TXT_PruebaObs.TabIndex = 8;
            //
            // BTN_RegistrarPrueba
            //
            this.BTN_RegistrarPrueba.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_RegistrarPrueba.FlatAppearance.BorderSize = 0;
            this.BTN_RegistrarPrueba.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_RegistrarPrueba.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_RegistrarPrueba.ForeColor = System.Drawing.Color.White;
            this.BTN_RegistrarPrueba.Location = new System.Drawing.Point(150, 382);
            this.BTN_RegistrarPrueba.Name = "BTN_RegistrarPrueba";
            this.BTN_RegistrarPrueba.Size = new System.Drawing.Size(160, 30);
            this.BTN_RegistrarPrueba.TabIndex = 9;
            this.BTN_RegistrarPrueba.Tag = "OrdenDetalle.RegistrarPrueba";
            this.BTN_RegistrarPrueba.Text = "Registrar prueba";
            this.BTN_RegistrarPrueba.UseVisualStyleBackColor = false;
            this.BTN_RegistrarPrueba.Click += new System.EventHandler(this.BTN_RegistrarPrueba_Click);
            //
            // TAB_Entrega
            //
            this.TAB_Entrega.Controls.Add(this.LBL_EEntregadoA);
            this.TAB_Entrega.Controls.Add(this.TXT_EEntregadoA);
            this.TAB_Entrega.Controls.Add(this.LBL_EDocumento);
            this.TAB_Entrega.Controls.Add(this.TXT_EDocumento);
            this.TAB_Entrega.Controls.Add(this.LBL_EObs);
            this.TAB_Entrega.Controls.Add(this.TXT_EObs);
            this.TAB_Entrega.Controls.Add(this.LBL_FechaEntrega);
            this.TAB_Entrega.Controls.Add(this.BTN_Entregar);
            this.TAB_Entrega.Location = new System.Drawing.Point(4, 22);
            this.TAB_Entrega.Name = "TAB_Entrega";
            this.TAB_Entrega.Size = new System.Drawing.Size(888, 454);
            this.TAB_Entrega.TabIndex = 6;
            this.TAB_Entrega.Tag = "OrdenDetalle.TabEntrega";
            this.TAB_Entrega.Text = "Entrega";
            this.TAB_Entrega.UseVisualStyleBackColor = true;
            //
            // LBL_EEntregadoA
            //
            this.LBL_EEntregadoA.AutoSize = true;
            this.LBL_EEntregadoA.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_EEntregadoA.Location = new System.Drawing.Point(12, 14);
            this.LBL_EEntregadoA.Name = "LBL_EEntregadoA";
            this.LBL_EEntregadoA.Size = new System.Drawing.Size(80, 15);
            this.LBL_EEntregadoA.TabIndex = 0;
            this.LBL_EEntregadoA.Tag = "OrdenDetalle.EntregadoA";
            this.LBL_EEntregadoA.Text = "Entregado a:";
            //
            // TXT_EEntregadoA
            //
            this.TXT_EEntregadoA.Location = new System.Drawing.Point(150, 11);
            this.TXT_EEntregadoA.Name = "TXT_EEntregadoA";
            this.TXT_EEntregadoA.Size = new System.Drawing.Size(330, 22);
            this.TXT_EEntregadoA.TabIndex = 1;
            //
            // LBL_EDocumento
            //
            this.LBL_EDocumento.AutoSize = true;
            this.LBL_EDocumento.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_EDocumento.Location = new System.Drawing.Point(12, 44);
            this.LBL_EDocumento.Name = "LBL_EDocumento";
            this.LBL_EDocumento.Size = new System.Drawing.Size(73, 15);
            this.LBL_EDocumento.TabIndex = 2;
            this.LBL_EDocumento.Tag = "OrdenDetalle.Documento";
            this.LBL_EDocumento.Text = "Documento:";
            //
            // TXT_EDocumento
            //
            this.TXT_EDocumento.Location = new System.Drawing.Point(150, 41);
            this.TXT_EDocumento.Name = "TXT_EDocumento";
            this.TXT_EDocumento.Size = new System.Drawing.Size(330, 22);
            this.TXT_EDocumento.TabIndex = 3;
            //
            // LBL_EObs
            //
            this.LBL_EObs.AutoSize = true;
            this.LBL_EObs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_EObs.Location = new System.Drawing.Point(12, 74);
            this.LBL_EObs.Name = "LBL_EObs";
            this.LBL_EObs.Size = new System.Drawing.Size(87, 15);
            this.LBL_EObs.TabIndex = 4;
            this.LBL_EObs.Tag = "OrdenDetalle.ObsEntrega";
            this.LBL_EObs.Text = "Observaciones:";
            //
            // TXT_EObs
            //
            this.TXT_EObs.Location = new System.Drawing.Point(150, 71);
            this.TXT_EObs.Multiline = true;
            this.TXT_EObs.Name = "TXT_EObs";
            this.TXT_EObs.Size = new System.Drawing.Size(720, 80);
            this.TXT_EObs.TabIndex = 5;
            //
            // LBL_FechaEntrega
            //
            this.LBL_FechaEntrega.AutoSize = true;
            this.LBL_FechaEntrega.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LBL_FechaEntrega.Location = new System.Drawing.Point(12, 162);
            this.LBL_FechaEntrega.Name = "LBL_FechaEntrega";
            this.LBL_FechaEntrega.Size = new System.Drawing.Size(90, 15);
            this.LBL_FechaEntrega.TabIndex = 6;
            this.LBL_FechaEntrega.Text = "Fecha entrega: -";
            //
            // BTN_Entregar
            //
            this.BTN_Entregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.BTN_Entregar.FlatAppearance.BorderSize = 0;
            this.BTN_Entregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Entregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Entregar.ForeColor = System.Drawing.Color.White;
            this.BTN_Entregar.Location = new System.Drawing.Point(150, 192);
            this.BTN_Entregar.Name = "BTN_Entregar";
            this.BTN_Entregar.Size = new System.Drawing.Size(140, 30);
            this.BTN_Entregar.TabIndex = 7;
            this.BTN_Entregar.Tag = "OrdenDetalle.Entregar";
            this.BTN_Entregar.Text = "Entregar";
            this.BTN_Entregar.UseVisualStyleBackColor = false;
            this.BTN_Entregar.Click += new System.EventHandler(this.BTN_Entregar_Click);
            //
            // BTN_Cerrar
            //
            this.BTN_Cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BTN_Cerrar.BackColor = System.Drawing.Color.Gray;
            this.BTN_Cerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_Cerrar.FlatAppearance.BorderSize = 0;
            this.BTN_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Cerrar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_Cerrar.ForeColor = System.Drawing.Color.White;
            this.BTN_Cerrar.Location = new System.Drawing.Point(793, 610);
            this.BTN_Cerrar.Name = "BTN_Cerrar";
            this.BTN_Cerrar.Size = new System.Drawing.Size(115, 32);
            this.BTN_Cerrar.TabIndex = 2;
            this.BTN_Cerrar.Tag = "OrdenDetalle.Cerrar";
            this.BTN_Cerrar.Text = "Cerrar";
            this.BTN_Cerrar.UseVisualStyleBackColor = false;
            //
            // FrmOrdenServicioDetalle
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.CancelButton = this.BTN_Cerrar;
            this.ClientSize = new System.Drawing.Size(920, 654);
            this.Controls.Add(this.BTN_Cerrar);
            this.Controls.Add(this.TAB_Detalle);
            this.Controls.Add(this.PNL_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOrdenServicioDetalle";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "OrdenDetalle.TituloDetalle";
            this.Text = "Detalle de orden";
            this.Load += new System.EventHandler(this.FrmOrdenServicioDetalle_Load);
            this.PNL_Header.ResumeLayout(false);
            this.PNL_Header.PerformLayout();
            this.TAB_Detalle.ResumeLayout(false);
            this.TAB_Recepcion.ResumeLayout(false);
            this.TAB_Recepcion.PerformLayout();
            this.TAB_Diagnostico.ResumeLayout(false);
            this.TAB_Diagnostico.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_DDias)).EndInit();
            this.TAB_Presupuesto.ResumeLayout(false);
            this.TAB_Presupuesto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Detalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PCant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PPrecio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PDescuento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_PGarantia)).EndInit();
            this.TAB_Historial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Historial)).EndInit();
            this.TAB_Reparaciones.ResumeLayout(false);
            this.TAB_Reparaciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Reparaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_ConsumoCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Consumidos)).EndInit();
            this.TAB_Pruebas.ResumeLayout(false);
            this.TAB_Pruebas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Pruebas)).EndInit();
            this.TAB_Entrega.ResumeLayout(false);
            this.TAB_Entrega.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel PNL_Header;
        private System.Windows.Forms.Label LBL_Numero;
        private System.Windows.Forms.Label LBL_Estado;
        private System.Windows.Forms.Label LBL_Resultado;
        private System.Windows.Forms.Label LBL_Tipo;
        private System.Windows.Forms.Label LBL_HCliente;
        private System.Windows.Forms.Label LBL_HEquipo;
        private System.Windows.Forms.Label LBL_HTecnico;
        private System.Windows.Forms.Label LBL_HFecha;
        private System.Windows.Forms.TabControl TAB_Detalle;
        private System.Windows.Forms.TabPage TAB_Recepcion;
        private System.Windows.Forms.Label LBL_RCliente;
        private System.Windows.Forms.ComboBox CBO_RCliente;
        private System.Windows.Forms.Label LBL_REquipo;
        private System.Windows.Forms.ComboBox CBO_REquipo;
        private System.Windows.Forms.Label LBL_RTecnico;
        private System.Windows.Forms.ComboBox CBO_RTecnico;
        private System.Windows.Forms.Label LBL_RProblema;
        private System.Windows.Forms.TextBox TXT_RProblema;
        private System.Windows.Forms.Label LBL_REstadoFisico;
        private System.Windows.Forms.TextBox TXT_REstadoFisico;
        private System.Windows.Forms.Label LBL_RAccesorios;
        private System.Windows.Forms.TextBox TXT_RAccesorios;
        private System.Windows.Forms.Label LBL_RObs;
        private System.Windows.Forms.TextBox TXT_RObs;
        private System.Windows.Forms.Button BTN_CrearOrden;
        private System.Windows.Forms.Button BTN_GuardarRecepcion;
        private System.Windows.Forms.Button BTN_AsignarTecnico;
        private System.Windows.Forms.TabPage TAB_Diagnostico;
        private System.Windows.Forms.Label LBL_DDescripcion;
        private System.Windows.Forms.TextBox TXT_DDescripcion;
        private System.Windows.Forms.CheckBox CHK_EsReparable;
        private System.Windows.Forms.Label LBL_DDias;
        private System.Windows.Forms.NumericUpDown NUM_DDias;
        private System.Windows.Forms.Label LBL_DObs;
        private System.Windows.Forms.TextBox TXT_DObs;
        private System.Windows.Forms.Label LBL_AvisoNoReparable;
        private System.Windows.Forms.Button BTN_Iniciar;
        private System.Windows.Forms.Button BTN_Finalizar;
        private System.Windows.Forms.TabPage TAB_Presupuesto;
        private System.Windows.Forms.DataGridView DGV_Detalle;
        private System.Windows.Forms.Label LBL_PTipo;
        private System.Windows.Forms.ComboBox CBO_PTipo;
        private System.Windows.Forms.Label LBL_PDesc;
        private System.Windows.Forms.TextBox TXT_PDesc;
        private System.Windows.Forms.Label LBL_PCant;
        private System.Windows.Forms.NumericUpDown NUM_PCant;
        private System.Windows.Forms.Label LBL_PPrecio;
        private System.Windows.Forms.NumericUpDown NUM_PPrecio;
        private System.Windows.Forms.Button BTN_AgregarItem;
        private System.Windows.Forms.Button BTN_QuitarItem;
        private System.Windows.Forms.Label LBL_PDescuento;
        private System.Windows.Forms.NumericUpDown NUM_PDescuento;
        private System.Windows.Forms.Label LBL_PGarantia;
        private System.Windows.Forms.NumericUpDown NUM_PGarantia;
        private System.Windows.Forms.Label LBL_PMedio;
        private System.Windows.Forms.TextBox TXT_PMedio;
        private System.Windows.Forms.Label LBL_PMotivo;
        private System.Windows.Forms.TextBox TXT_PMotivo;
        private System.Windows.Forms.Label LBL_PObs;
        private System.Windows.Forms.TextBox TXT_PObs;
        private System.Windows.Forms.Label LBL_Totales;
        private System.Windows.Forms.Label LBL_EstadoPresupuesto;
        private System.Windows.Forms.Button BTN_Emitir;
        private System.Windows.Forms.Button BTN_Aprobar;
        private System.Windows.Forms.Button BTN_Rechazar;
        private System.Windows.Forms.TabPage TAB_Historial;
        private System.Windows.Forms.DataGridView DGV_Historial;
        private System.Windows.Forms.TabPage TAB_Reparaciones;
        private System.Windows.Forms.DataGridView DGV_Reparaciones;
        private System.Windows.Forms.Button BTN_IniciarReparacion;
        private System.Windows.Forms.Label LBL_CRepuesto;
        private System.Windows.Forms.ComboBox CBO_ConsumoRepuesto;
        private System.Windows.Forms.Label LBL_CCantidad;
        private System.Windows.Forms.NumericUpDown NUM_ConsumoCantidad;
        private System.Windows.Forms.Button BTN_Consumir;
        private System.Windows.Forms.Label LBL_Consumidos;
        private System.Windows.Forms.DataGridView DGV_Consumidos;
        private System.Windows.Forms.Label LBL_CostoTotal;
        private System.Windows.Forms.Label LBL_FTrabajo;
        private System.Windows.Forms.TextBox TXT_FTrabajo;
        private System.Windows.Forms.Label LBL_FObs;
        private System.Windows.Forms.TextBox TXT_FObs;
        private System.Windows.Forms.Button BTN_FinalizarReparacion;
        private System.Windows.Forms.TabPage TAB_Pruebas;
        private System.Windows.Forms.Label LBL_PIntervencion;
        private System.Windows.Forms.ComboBox CBO_PruebaReparacion;
        private System.Windows.Forms.DataGridView DGV_Pruebas;
        private System.Windows.Forms.Label LBL_PDDesc;
        private System.Windows.Forms.TextBox TXT_PruebaDesc;
        private System.Windows.Forms.RadioButton RDO_Aprobada;
        private System.Windows.Forms.RadioButton RDO_Fallida;
        private System.Windows.Forms.Label LBL_PDObs;
        private System.Windows.Forms.TextBox TXT_PruebaObs;
        private System.Windows.Forms.Button BTN_RegistrarPrueba;
        private System.Windows.Forms.TabPage TAB_Entrega;
        private System.Windows.Forms.Label LBL_EEntregadoA;
        private System.Windows.Forms.TextBox TXT_EEntregadoA;
        private System.Windows.Forms.Label LBL_EDocumento;
        private System.Windows.Forms.TextBox TXT_EDocumento;
        private System.Windows.Forms.Label LBL_EObs;
        private System.Windows.Forms.TextBox TXT_EObs;
        private System.Windows.Forms.Label LBL_FechaEntrega;
        private System.Windows.Forms.Button BTN_Entregar;
        private System.Windows.Forms.Button BTN_Cerrar;
    }
}
