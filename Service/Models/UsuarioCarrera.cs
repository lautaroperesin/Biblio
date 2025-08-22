using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class UsuarioCarrera
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }
        public bool isDeleted { get; set; } = false;

    }
}
