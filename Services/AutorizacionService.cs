using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace TareaPractica5Unidad5.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly Practica5Context? _context;
        private readonly IConfiguration? _configuration;

        public AutorizacionService(Practica5Context? context, IConfiguration? configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerarToken(string idUsuario)
        {
            var key = _configuration?.GetValue<string>("JWT:Key");
            var keyBytes = Encoding.ASCII.GetBytes(key!);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            var credencialesToken = new SigningCredentials(new SymmetricSecurityKey(keyBytes), 
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = System.DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }

        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            var usuarioEncontrado = _context?.Usuarios.FirstOrDefault(u => 
            u.Correo == autorizacion.NombreUsuario &&
            u.Password == autorizacion.Clave);

            if (usuarioEncontrado == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null!);
            }

            string tokenCreado = GenerarToken(usuarioEncontrado.Id.ToString());

            return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Mensaje = "Token actualizado." };
        }
    }
}
