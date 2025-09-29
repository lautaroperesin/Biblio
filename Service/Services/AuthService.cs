using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        public AuthService() { }

        protected void SetAuthorizationHeader(HttpClient _httpClient)
        {
            if (!string.IsNullOrEmpty(GenericService<object>.token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenericService<object>.token);
            else
                throw new ArgumentException("Token no definido.", nameof(GenericService<object>.token));
        }

        public async Task<string?> Login(LoginDTO? login)
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
                    return null;
                }
                else
                {
                    // Retornar el mensaje de error desde la respuesta
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en AuthService al loguearse: {ex.Message}");
            }
        }

        public async Task<bool> ResetPassword(LoginDTO? login)
        {
            if (login == null)
            {
                throw new ArgumentException("El objeto login no llego.");
            }
            try
            {
                var UrlApi = Properties.Resources.urlApi;
                var endpointAuth = ApiEndpoints.GetEndpoint("Login");
                var client = new HttpClient();
                SetAuthorizationHeader(client);
                var response = await client.PostAsJsonAsync($"{UrlApi}{endpointAuth}/resetpassword/", login);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resetear" + ex.Message);
            }
        }

        public async Task<bool> CreateUserWithEmailAndPassword(string email, string password, string nombre)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException("Email, password o nombre no pueden ser nulos o vacíos.");
            }
            try
            {
                var UrlApi = Properties.Resources.urlApi;
                var endpointAuth = ApiEndpoints.GetEndpoint("Login");
                var client = new HttpClient();
                var newUser = new RegisterDTO
                {
                    Email = email,
                    Password = password,
                    Nombre = nombre
                };
                var response = await client.PostAsJsonAsync($"{UrlApi}{endpointAuth}/register/", newUser);
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
                throw new Exception("Error al crear usuario" + ex.Message);
            }
        }
        }
    }
