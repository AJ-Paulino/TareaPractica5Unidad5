using TareaPractica5Unidad5.Models;

namespace TareaPractica5Unidad5.Services.UsuariosServices
{
    public interface IUsuarioService
    {
        Task<Usuario> DeleteId(int id, Usuario usuario);
        Task<List<Usuario>> GetAll();
        Task<Usuario> GetId(int id);
        Task<string> Post(Usuario usuario);
        Task<Usuario> PutId(int id, Usuario usuario);
    }
}
