using MagicVilla_API.Modelos;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
