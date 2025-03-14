using System;
using System.Collections.Generic;

namespace TareaPractica5Unidad5.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime FechaDeNacimiento { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<HistorialRefreshToken> HistorialRefreshTokens { get; set; } = new List<HistorialRefreshToken>();
}
