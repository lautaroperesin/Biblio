using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Enums;

namespace Service.Models
{
    public class Ejemplar
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
        public bool Disponible { get; set; } = true;
        [Required]
        public EstadoEjemplarEnum Estado { get; set; } = EstadoEjemplarEnum.Excelente;
        public bool isDeleted { get; set; } = false;

    }
}
