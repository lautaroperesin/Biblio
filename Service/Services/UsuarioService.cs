using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Interfaces;
using Service.Models;
using Service.Utils;

namespace Service.Services
{
    public class UsuarioService : GenericService<Usuario>, IUsuarioService
    {
        public UsuarioService(HttpClient? httpClient = null, IMemoryCache? memoryCache = null) : base(httpClient, memoryCache)
        {

        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            SetAuthorizationHeader();
            try
            {
                var response = await _httpClient.GetAsync($"{_endpoint}/byemail?email={email}");
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al obtener el usuario por email: {response.StatusCode} - {content}");
                }
                return JsonSerializer.Deserialize<Usuario>(content, _options);
            }
            catch(Exception ex)
            {
                throw new Exception($"Error en UsuarioService al obtener por email: {ex.Message}");
            }
        }
    }
}
