﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TareaPractica5Unidad5.Models;

public partial class HistorialRefreshToken
{
    [Key]
    public int IdHistorialToken { get; set; }

    public int? IdUsuario { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    public bool? EsActivo { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
