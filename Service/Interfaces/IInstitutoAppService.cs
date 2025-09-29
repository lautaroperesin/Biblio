using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models.InstitutoApp;

namespace Service.Interfaces
{
    public interface IInstitutoAppService
    {
        Task<Usuario?> GetUsuarioByEmailAsync(string email); 
    }
}
