using System;
using System.Collections.Generic;
using ABSTRACTIONS.Features.Permisos;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Permisos
{
    public abstract class PermisoComponent : IPermisoComponent
    {
        private readonly IList<IPermisoComponent> _hijos;

        // En el Composite, familias y permisos simples comparten identidad por id y nombre.
        // Codigo identifica permisos simples desde codigo de aplicacion; las familias no lo usan.
        public int Id { get; protected set; }
        public string Nombre { get; protected set; }
        public string Codigo { get; protected set; }
        public abstract bool EsFamilia { get; }
        public IList<IPermisoComponent> Hijos { get { return _hijos; } }

        protected PermisoComponent()
        {
            _hijos = new List<IPermisoComponent>();
        }

        public abstract void AgregarHijo(IPermisoComponent permiso);

        public virtual bool Contiene(IPermisoComponent permiso)
        {
            if (permiso == null)
                return false;

            return TieneMismoIdentificador(permiso);
        }

        protected static void ValidarDatos(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre del permiso es obligatorio.");
        }

        protected bool TieneMismoIdentificador(IPermisoComponent permiso)
        {
            if (permiso == null)
                return false;

            // Si ambos objetos vienen de base, el id es la identidad principal.
            if (Id > 0 && permiso.Id > 0)
                return Id == permiso.Id;

            // Para objetos aun no persistidos, el nombre unico permite validar duplicados.
            return string.Equals(Nombre, permiso.Nombre, StringComparison.OrdinalIgnoreCase);
        }
    }
}
