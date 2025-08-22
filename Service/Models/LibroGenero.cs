using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class LibroGenero
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public bool isDeleted { get; set; } = false;

    }
}
