using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using CFDB;

namespace TareaPractica5Unidad5.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly Models.Practica5Context _context;
        private readonly IConfiguration _configuration;

        public AutorizacionService(Models.Practica5Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerarToken(string idUsuario)
        {
            var key = _configuration.GetValue<string>("JWT:Key");
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

        private string GenerarRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }

            return refreshToken;
        }

        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(
            int idUsuario,
            string token,
            string refreshToken
            )
        {
            var historialRefreshToken = new HistorialRefreshToken
            {
                IdUsuario = idUsuario,
                Token = token,
                RefreshToken = refreshToken,
                FechaCreacion = System.DateTime.UtcNow,
                FechaExpiracion = System.DateTime.UtcNow.AddMinutes(60)
            };

            await _context.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _context.SaveChangesAsync();

            return new AutorizacionResponse() 
            { 
                Token = token, 
                RefreshToken = refreshToken, 
                Resultado = true, 
                Mensaje = "Ok." 
            };
        }

        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            var usuarioEncontrado = _context.Usuarios.FirstOrDefault(u => 
            u.Correo == autorizacion.NombreUsuario &&
            u.Password == autorizacion.Clave);

            if (usuarioEncontrado == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null!);
            }

            string tokenCreado = GenerarToken(usuarioEncontrado.Id.ToString());

            string refreshTokenCreado = GenerarRefreshToken();

            //return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Mensaje = "Token actualizado." };

            return await GuardarHistorialRefreshToken(usuarioEncontrado.Id, tokenCreado, refreshTokenCreado);
        }

        public async Task<AutorizacionResponse> DevolverRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUsuario)
        {
            var refreshTokenEncontrado = _context.HistorialRefreshTokens.FirstOrDefault(rt =>
            rt.Token == refreshTokenRequest.TokenExpirado &&
            rt.RefreshToken == refreshTokenRequest.RefreshToken &&
            rt.IdUsuario == idUsuario);

            if (refreshTokenEncontrado == null)
                return new AutorizacionResponse 
                { 
                    Resultado = false, 
                    Mensaje = "RefreshToken no encontrado." 
                };

            var refreshTokenCreado = GenerarRefreshToken();
            var tokenCreado = GenerarToken(idUsuario.ToString());

            return await GuardarHistorialRefreshToken(idUsuario, tokenCreado, refreshTokenCreado);
        }
    }
}
