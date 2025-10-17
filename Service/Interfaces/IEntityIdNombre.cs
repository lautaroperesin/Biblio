using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEntityIdNombre
    {
        int Id { get; set; }
        string Nombre { get; set; }
    }
}
