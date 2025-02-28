using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TareaPractica5Unidad5.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "Debe ingresar su nombre y apellido.")]
        public string Nombre { get; set; }

        [EmailAddress(ErrorMessage = "Correo no válido.")]
        public string Correo { get; set; }
        public DateTime FechaDeNacimiento { get; set; }

        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }
    }
}
