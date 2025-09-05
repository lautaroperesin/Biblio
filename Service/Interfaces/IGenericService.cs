using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        // Firma del método
        public Task<List<T>?> GetAllAsync(string? filtro);
        public Task<List<T>?> GetAllDeletedsAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> AddAsync(T? entity);
        public Task<bool> UpdateAsync(T? entity);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> RestoreAsync(int id);
    }
}
