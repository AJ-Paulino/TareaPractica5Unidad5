using CFDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TareaPractica5Unidad5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private Practica5Context _context;
        public ProductoController(Practica5Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Producto>> GetAll()
            => await _context.Productos.ToListAsync();
    }
}
