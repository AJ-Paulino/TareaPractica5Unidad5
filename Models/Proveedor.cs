using System;
using System.Collections.Generic;

namespace TareaPractica5Unidad5.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contacto { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Categorium> CategoriasIdCategoria { get; set; } = new List<Categorium>();
}
