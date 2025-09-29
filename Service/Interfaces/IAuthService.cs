using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.DTOs;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<string?> Login(LoginDTO login);
        Task<bool> ResetPassword(LoginDTO? login);
        Task<bool> CreateUserWithEmailAndPassword(string email, string password, string nombre);
    }
}
