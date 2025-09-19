using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class FilterLibroDTO
    {
        public string SearchText { get; set; } = "";
        public bool ForTitulo { get; set; } = false;
        public bool ForAutor { get; set; } = false;
        public bool ForEditorial { get; set; } = false;
        public bool ForGenero { get; set; } = false;
    }
}
