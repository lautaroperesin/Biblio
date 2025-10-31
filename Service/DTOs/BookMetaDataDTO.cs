using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class BookMetaDataDTO
    {
        public string? Titulo { get; set; }
        public List<string>? Autores { get; set; }
        public string? Generos { get; set; }
        public string? Editorial { get; set; }
        public int? Anio { get; set; }
        public int? Paginas { get; set; }
        public string? Descripcion { get; set; }
        public string? Sinopsis { get; set; }
        public string? CDU { get; set; }
        public string? Libristica { get; set; }
        public List<string>? PalabrasClave
        {
            get; set;

        }
    }
}
