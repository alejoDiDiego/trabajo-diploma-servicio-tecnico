using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace UI.Forms.Equipos
{
    // A UI snapshot, not an entity serializer: no repository access or hidden fields.
    internal sealed class EquipoExportacion
    {
        internal readonly List<string> Campos = new List<string>();
        internal readonly List<string> Encabezados = new List<string>();
        internal readonly List<Dictionary<string, object>> Filas = new List<Dictionary<string, object>>();
        internal readonly DateTime Fecha = DateTime.Now;
    }

    internal static class EquipoExportador
    {
        internal static EquipoExportacion Capturar(DataGridView grid)
        {
            if (grid.InvokeRequired)
                throw new InvalidOperationException("Capture must run on the UI thread.");

            var resultado = new EquipoExportacion();
            var columnas = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
            foreach (var columna in columnas)
            {
                resultado.Campos.Add(string.IsNullOrEmpty(columna.DataPropertyName) ? columna.Name : columna.DataPropertyName);
                resultado.Encabezados.Add(columna.HeaderText);
            }
            foreach (DataGridViewRow fila in grid.Rows)
            {
                if (fila.IsNewRow || !fila.Visible)
                    continue;
                var valores = new Dictionary<string, object>();
                for (int i = 0; i < columnas.Count; i++)
                {
                    object valor = fila.Cells[columnas[i].Index].Value;
                    string campo = resultado.Campos[i];
                    if (campo == "Id")
                        valores.Add(campo, Convert.ToInt32(valor, CultureInfo.InvariantCulture));
                    else if (campo == "Activo")
                        valores.Add(campo, Convert.ToBoolean(valor, CultureInfo.InvariantCulture));
                    else
                        valores.Add(campo, valor == null || valor == DBNull.Value ? "" : Convert.ToString(valor, CultureInfo.CurrentCulture));
                }
                resultado.Filas.Add(valores);
            }
            return resultado;
        }

        internal static void EscribirJson(Stream destino, EquipoExportacion datos)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<Dictionary<string, object>>),
                new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            serializer.WriteObject(destino, datos.Filas);
        }

        internal static void EscribirPdf(Stream destino, EquipoExportacion datos, string titulo,
            string resumen, string pagina, string si, string no)
        {
            var documento = new Document();
            documento.Info.Title = titulo;
            documento.Info.Author = "Servicio Tecnico";
            documento.Styles[StyleNames.Normal].Font.Name = "Segoe UI";
            documento.Styles[StyleNames.Normal].Font.Size = 8;
            documento.Styles[StyleNames.Normal].ParagraphFormat.SpaceAfter = 0;
            var seccion = documento.AddSection();
            seccion.PageSetup.PageFormat = PageFormat.A4;
            seccion.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            seccion.PageSetup.TopMargin = Unit.FromCentimeter(1.4);
            seccion.PageSetup.BottomMargin = Unit.FromCentimeter(1.7);
            seccion.PageSetup.FooterDistance = Unit.FromCentimeter(0.6);
            seccion.PageSetup.LeftMargin = Unit.FromCentimeter(1.2);
            seccion.PageSetup.RightMargin = Unit.FromCentimeter(1.2);
            var encabezado = seccion.AddParagraph(titulo);
            encabezado.Format.Font.Size = 18;
            encabezado.Format.Font.Bold = true;
            encabezado.Format.Font.Color = Colors.DarkSlateBlue;
            encabezado.Format.SpaceAfter = Unit.FromPoint(6);
            var descripcion = seccion.AddParagraph(string.Format(CultureInfo.CurrentCulture, resumen,
                datos.Fecha.ToString("g", CultureInfo.CurrentCulture), datos.Filas.Count));
            descripcion.Format.SpaceAfter = Unit.FromPoint(12);
            var pie = seccion.Footers.Primary.AddParagraph();
            pie.Format.Alignment = ParagraphAlignment.Right;
            pie.AddText(pagina + " ");
            pie.AddPageField();
            pie.AddText(" / ");
            pie.AddNumPagesField();

            var tabla = seccion.AddTable();
            tabla.Borders.Color = Colors.LightGray;
            tabla.Borders.Width = 0.4;
            tabla.TopPadding = Unit.FromPoint(4);
            tabla.BottomPadding = Unit.FromPoint(4);
            // Relative widths follow the visible grid field, including reordered columns.
            double total = datos.Campos.Sum(Peso);
            foreach (string campo in datos.Campos)
                tabla.AddColumn(Unit.FromCentimeter(27.3 * Peso(campo) / total));
            var cabecera = tabla.AddRow();
            cabecera.HeadingFormat = true;
            cabecera.Shading.Color = Colors.DarkSlateBlue;
            cabecera.Format.Font.Color = Colors.White;
            cabecera.Format.Font.Bold = true;
            for (int i = 0; i < datos.Campos.Count; i++)
                cabecera.Cells[i].AddParagraph(datos.Encabezados[i]);

            for (int n = 0; n < datos.Filas.Count; n++)
            {
                var textos = datos.Campos.Select(c => Partir(Texto(datos.Filas[n][c], si, no))).ToList();
                // MigraDoc cannot split a single oversized table row across pages.
                // Continuation rows keep arbitrary long values without clipping/truncation.
                int partes = textos.Max(t => t.Count);
                for (int parte = 0; parte < partes; parte++)
                {
                    Row row = tabla.AddRow();
                    row.VerticalAlignment = VerticalAlignment.Top;
                    if (n % 2 == 1)
                        row.Shading.Color = Colors.WhiteSmoke;
                    for (int i = 0; i < textos.Count; i++)
                    {
                        string fragmento = parte < textos[i].Count ? textos[i][parte] : "";
                        row.Cells[i].AddParagraph(Envolver(fragmento));
                    }
                }
            }
            var renderer = new PdfDocumentRenderer { Document = documento };
            renderer.RenderDocument();
            using (var pdf = renderer.PdfDocument)
                pdf.Save(destino, false);
        }

        private static double Peso(string campo)
        {
            switch (campo)
            {
                case "Id": return 0.6;
                case "Activo": return 0.8;
                case "Cliente": return 2.8;
                case "Modelo": return 1.8;
                case "NumeroSerie": case "Imei": return 1.7;
                default: return 1.4;
            }
        }

        private static string Texto(object valor, string si, string no)
        {
            return valor is bool ? ((bool)valor ? si : no) : Convert.ToString(valor, CultureInfo.CurrentCulture);
        }

        private static List<string> Partir(string texto)
        {
            var partes = new List<string>();
            var parte = new StringBuilder();
            int saltos = 0;
            var elementos = StringInfo.GetTextElementEnumerator(texto);
            while (elementos.MoveNext())
            {
                string elemento = elementos.GetTextElement();
                if (parte.Length >= 120 || saltos >= 6)
                {
                    partes.Add(parte.ToString());
                    parte.Clear();
                    saltos = 0;
                }
                parte.Append(elemento);
                if (elemento.Contains("\n") || elemento.Contains("\r"))
                    saltos++;
            }
            partes.Add(parte.ToString());
            return partes;
        }

        private static string Envolver(string texto)
        {
            // Soft break opportunities also handle serials/IMEIs without spaces.
            var salida = new StringBuilder();
            int longitud = 0;
            foreach (char c in texto)
            {
                salida.Append(c);
                longitud = char.IsWhiteSpace(c) ? 0 : longitud + 1;
                if (longitud >= 12 && !char.IsHighSurrogate(c))
                {
                    salida.Append('\u200B');
                    longitud = 0;
                }
            }
            return salida.ToString();
        }

        internal static void Guardar(string destino, bool reemplazar, Action<Stream> generar)
        {
            string ruta = Path.GetFullPath(destino);
            string temporal = Path.Combine(Path.GetDirectoryName(ruta), ".equipos-" + Guid.NewGuid().ToString("N") + ".tmp");
            bool creado = false;
            try
            {
                using (var archivo = new FileStream(temporal, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    creado = true;
                    generar(archivo);
                    archivo.Flush(true);
                }
                // No delete-and-move fallback: unsupported atomic replacement fails safely.
                if (reemplazar)
                    File.Replace(temporal, ruta, null);
                else
                    File.Move(temporal, ruta);
                creado = false;
            }
            finally
            {
                if (creado)
                {
                    try { File.Delete(temporal); }
                    catch (IOException) { /* Preserve original export failure; owned temp only. */ }
                    catch (UnauthorizedAccessException) { }
                }
            }
        }
    }
}
