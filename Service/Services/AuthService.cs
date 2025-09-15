using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Interfaces;
using Service.Utils;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
        }

        public async Task<bool> Login(LoginDTO? login)
        {
            if (login == null)
            {
                throw new ArgumentException("El objeto login es nulo.");
            }

            try
            {
                var urlApi = Properties.Resources.urlApi;
                var endpointAuth = ApiEndpoints.GetEndpoint("Login");
                var client = new HttpClient();
                var response = await client.PostAsJsonAsync($"{urlApi}{endpointAuth}/login", login);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    GenericService<object>.token = result;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en AuthService al loguearse: {ex.Message}");
            }
        }
    }
}
