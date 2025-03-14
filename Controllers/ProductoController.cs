using CFDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TareaPractica5Unidad5.Custom;
using TareaPractica5Unidad5.Models;
using TareaPractica5Unidad5.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using TareaPractica5Unidad5.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TareaPractica5Unidad5.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private CFDB.Practica5Context _Practica5Context;
        public ProductoController(CFDB.Practica5Context Practica5Context)
        {
            _Practica5Context = Practica5Context;
        }

        [HttpGet]
        [Route("ListaProductos")]
        public async Task<IActionResult> ListaProductos()
        {
            var listaProductos = await _Practica5Context.Productos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new {value = listaProductos});
        }
    }
}
