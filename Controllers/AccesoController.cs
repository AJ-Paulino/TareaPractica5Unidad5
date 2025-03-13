using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareaPractica5Unidad5.Custom;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using TareaPractica5Unidad5.DB;

namespace TareaPractica5Unidad5.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly TareaPractica5Context _dbContext;
        private readonly Utilidades _utilidades;
        public AccesoController(TareaPractica5Context dbContext, Utilidades utilidades)
        {
            _dbContext = dbContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("CrearCuenta")]
        public async Task<IActionResult> CrearCuenta(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Correo = objeto.Correo,
                FechaDeNacimiento = objeto.FechaDeNacimiento,
                Password = _utilidades.EncriptarSHA256(objeto.Password!)
            };

            await _dbContext.Usuarios.AddAsync(modeloUsuario);
            await _dbContext.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
            {
                return StatusCode(StatusCodes.Status200OK ,new { isSuccess = true, 
                    mensaje = "Usuario creado."});
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, 
                    mensaje = "No se pudo crear el usuario." });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UsuarioDTO objeto)
        {
            var usuarioEncontrado = await _dbContext.Usuarios.Where(u => 
            u.Correo == objeto.Correo && 
            u.Password == _utilidades.EncriptarSHA256(objeto.Password!)).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "",
                mensaje = "No se pudo iniciar sesión."});
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true,
                    mensaje = "Sesión iniciada.",
                    token = _utilidades.GenerarJWT(usuarioEncontrado) });
            }
        }
    }
}
