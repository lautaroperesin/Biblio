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
        public string? Editorial { get; set; }
        public int? Anio { get; set; }
        public List<string>? Isbn { get; set; }         // acepta ISBN-10/13
        public string? Edicion { get; set; }
        public string? Subtitulo { get; set; }
        public string? Idioma { get; set; }
        public double? Confianza { get; set; }          // heurística del modelo
    }
}
