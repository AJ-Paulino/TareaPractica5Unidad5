using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Services.UsuariosServices;

namespace TareaPractica5Unidad5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<Usuario>> GetAll()
            => _service.GetAll();

        [HttpGet("Id")]
        public Task<Usuario> GetId(int id)
            => _service.GetId(id);

        [HttpPost]
        public Task<string> Post(Usuario usuario)
            => _service.Post(usuario);

        [HttpPut("Id")]
        public Task<Usuario> PutId(int id, Usuario usuario)
            => _service.PutId(id, usuario);

        [HttpDelete("Id")]
        public Task<Usuario> DeleteId(int id, Usuario usuario) 
            => _service.DeleteId(id, usuario);
    }
}
