using Microsoft.EntityFrameworkCore;
using TareaPractica5Unidad5.Models;

namespace TareaPractica5Unidad5.DB
{
    public class TareaPractica5Context : DbContext
    {
        public TareaPractica5Context(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}