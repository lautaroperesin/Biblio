using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Interfaces;
using Service.Models;
using Service.Utils;

namespace Service.Services
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        protected readonly JsonSerializerOptions _options;
        public static string? token;
        public UsuarioService(HttpClient? httpClient=null)
        {
            httpClient = httpClient ?? new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _endpoint = Properties.Resources.urlApi + ApiEndpoints.GetEndpoint(typeof(Usuario).Name);

            if (!string.IsNullOrEmpty(GenericService<object>.token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenericService<object>.token);
            }
            else
            {
                throw new Exception(token);
            }
        }
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/byemail?email={email}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener el usuario por email: {response.StatusCode} - {content}");
            }
            return JsonSerializer.Deserialize<Usuario>(content, _options);
        }
    }
}
