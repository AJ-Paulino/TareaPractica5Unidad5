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

        public Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion)
        {
            throw new NotImplementedException();
        }
    }
}
