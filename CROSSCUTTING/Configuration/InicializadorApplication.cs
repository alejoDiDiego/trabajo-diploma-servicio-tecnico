using APPLICATION.Interfaces;
using CROSSCUTTING.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROSSCUTTING.Configuration
{
    public static class InicializadorAplicacion
    {
        private static readonly string cadenaConexion = "Server=localhost,1433;Database=EvaluacionDiagnostico;User Id=sa;Password=TuPasswordSeguro123!;TrustServerCertificate=True";

        public static ISesionUsuario ObtenerSesion()
        {
            return SessionManager.GetInstance();
        }
    }
}
