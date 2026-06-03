using System.Collections.Generic;
using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Permisos
{
    public interface IPermisoComponent : IEntity
    {
        string Nombre { get; }
        string Codigo { get; }
        string Descripcion { get; }
        bool EsFamilia { get; }
        IList<IPermisoComponent> Hijos { get; }

        void AgregarHijo(IPermisoComponent permiso);
        void QuitarHijo(IPermisoComponent permiso);
        bool Contiene(IPermisoComponent permiso);
    }
}
