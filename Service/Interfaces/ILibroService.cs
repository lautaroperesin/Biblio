using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTOs;
using Service.Models;

namespace Service.Interfaces
{
    public interface ILibroService : IGenericService<Libro>
    {
        public Task<List<Libro>?> GetWithFilterAsync(FilterLibroDTO filter);
    }
}
