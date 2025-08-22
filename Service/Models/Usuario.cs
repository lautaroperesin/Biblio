using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Enums;

namespace Service.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public TipoRolEnum TipoRol { get; set; } = TipoRolEnum.Alumno;
        public DateTime FechaRegistracion { get; set; } = DateTime.Now;
        [Required]
        public string Dni { get; set; } = string.Empty;
        public string Domicilio { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
        public bool isDeleted { get; set; } = false;

    }
}
