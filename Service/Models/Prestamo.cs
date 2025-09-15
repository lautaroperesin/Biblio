using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int EjemplarId { get; set; }
        public Ejemplar? Ejemplar { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime? FechaDevolucion { get; set; }
        public bool isDeleted { get; set; } = false;

        public override string ToString()
        {
            return $"{Ejemplar.Libro.Titulo} - {FechaPrestamo.ToShortDateString()} - {FechaDevolucion?.ToShortDateString()}";
        }

    }
}
