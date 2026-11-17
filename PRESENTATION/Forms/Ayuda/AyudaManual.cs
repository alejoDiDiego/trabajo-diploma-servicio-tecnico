using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using SERVICES.Idiomas;

namespace UI.Forms.Ayuda
{
    internal static class AyudaManual
    {
        internal static readonly HashSet<string> Secciones = new HashSet<string>(StringComparer.Ordinal)
        {
            "indice", "inicio", "acceso", "usuarios", "permisos", "asignar-permisos", "backups",
            "bitacora", "cambios-traducciones", "idiomas-traducciones", "tipos-equipo", "marcas",
            "catalogos-nombre", "clientes", "cliente-editar", "equipos", "equipo-editar", "ordenes",
            "orden-recepcion", "diagnostico", "presupuestos", "reparaciones", "pruebas", "orden-historial",
            "entrega", "garantia-orden", "repuestos", "repuesto-editar", "stock-ajuste", "stock-movimientos",
            "proveedores", "proveedor-editar", "compras", "compra-detalle", "garantias", "dashboard", "reportes",
            "orden-cancelar", "compra-anular", "garantia-reingreso", "prueba-anular", "presupuesto-anular",
            "adicionales", "entrega-cancelar", "pruebas-reabrir", "consumo-devolver", "garantia-evaluar"
        };

        internal static void Mostrar(IWin32Window owner, string seccion)
        {
            using (var visor = new FrmAyuda(seccion)) visor.ShowDialog(owner);
        }

        internal static string Texto(string clave, string respaldo, IIdioma idioma = null)
        {
            idioma = idioma ?? SesionIdioma.GetInstance().idioma;
            string texto = idioma == null ? null : idioma.BuscarTraduccion(clave);
            return string.IsNullOrEmpty(texto) || texto == clave ? respaldo : texto;
        }
    }
}