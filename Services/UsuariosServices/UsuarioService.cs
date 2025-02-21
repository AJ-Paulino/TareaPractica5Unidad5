using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TareaPractica5Unidad5.DB;
using TareaPractica5Unidad5.Models;

namespace TareaPractica5Unidad5.Services.UsuariosServices
{
    public class UsuarioService : IUsuarioService
    {
        private TareaPractica5Context _context;
        public UsuarioService(TareaPractica5Context context)
        {
            _context = context;
        }
        public async Task<Usuario> GetId(int id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            return user!;
        }

        public async Task<List<Usuario>> GetAll() 
            => await _context.Usuarios.ToListAsync();

        public async Task<string> Post(Usuario usuario)
        {
            try
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return "Se guardó el usuario.";
            }
            catch (Exception e)
            {
                return $"\nOcurrió un error: {e}";
            }
        }

        public async Task<Usuario> DeleteId(int id, Usuario usuario)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(d => d.Id == id);

            if (user == null)
            {
                throw new Exception("No se encontró el usuario.");
            }

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            Console.WriteLine("Se eliminó el usuario.");

            return user;
        }

        public async Task<Usuario> PutId(int id, Usuario usuario)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(p => p.Id == id);

            if (user == null)
            {
                throw new Exception("No se encontró el usuario.");
            }

            user.Nombre = usuario.Nombre;
            user.Correo = usuario.Correo;
            
            return user;
        }
    }
}
