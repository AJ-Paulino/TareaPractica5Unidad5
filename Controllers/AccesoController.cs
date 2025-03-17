using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareaPractica5Unidad5.Custom;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using TareaPractica5Unidad5.DB;
using System.IdentityModel.Tokens.Jwt;
using TareaPractica5Unidad5.Services;
//using System.Xml;
using Newtonsoft.Json;

namespace TareaPractica5Unidad5.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly TareaPractica5Context _dbContext;
        private readonly Utilidades _utilidades;
        private readonly IAutorizacionService _autorizacionService;
        public AccesoController(TareaPractica5Context dbContext, Utilidades utilidades, IAutorizacionService autorizacionService)
        {
            _dbContext = dbContext;
            _utilidades = utilidades;
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("CrearCuenta")]
        public async Task<IActionResult> CrearCuenta(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre!,
                Correo = objeto.Correo!,
                FechaDeNacimiento = objeto.FechaDeNacimiento,
                Password = _utilidades.EncriptarSHA256(objeto.Password!)
            };

            await _dbContext.Usuarios.AddAsync(modeloUsuario);
            await _dbContext.SaveChangesAsync();

            // Guardar información en archivo JSON
            var usuarios = await _dbContext.Usuarios.ToListAsync();
            var json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");

            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Dispose();
            }

            await System.IO.File.WriteAllTextAsync(filePath, json);

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

        [HttpGet]
        [Route("HistorialLogs")]
        public IActionResult ObtenerHistorialLogs()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { isSuccess = false, mensaje = "No se encontró el archivo de logs." });
            }

            var json = System.IO.File.ReadAllText(filePath);
            var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

            return Ok(new { isSuccess = true, usuarios });
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

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiradoRecibido = tokenHandler.ReadJwtToken(request.TokenExpirado);

            if (tokenExpiradoRecibido.ValidTo > DateTime.UtcNow)
                return BadRequest(new AutorizacionResponse 
                {
                    Resultado = false, 
                    Mensaje = "El token no ha expirado." 
                });

            string idUsuario = tokenExpiradoRecibido.Claims.First(x=>
            x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var autorizacionResponse = await _autorizacionService.DevolverRefreshToken(request, int.Parse(idUsuario));

            if (autorizacionResponse.Resultado)
                return Ok(autorizacionResponse);
            else
                return BadRequest(autorizacionResponse);
        }
    }
}
