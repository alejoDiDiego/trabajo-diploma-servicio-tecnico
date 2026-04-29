using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN.Features.Usuarios;

namespace APPLICATION.Features.Usuarios.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario Agregar(Usuario p);
        List<Usuario> Listar();
        Usuario ObtenerPorUsername(string userName);
        Usuario ObtenerPorId(int id);
        void Eliminar(int id);
        void Modificar(Usuario u);
    }
}
