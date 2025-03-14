using System;
using System.Collections.Generic;

namespace TareaPractica5Unidad5.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Proveedor> ProveedoresIdProveedors { get; set; } = new List<Proveedor>();
}
