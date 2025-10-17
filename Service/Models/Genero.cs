using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Models
{
    public class Genero : IEntityIdNombre
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public bool isDeleted { get; set; } = false;

        public override string ToString()
        {
            return Nombre;
        }
    }
}
