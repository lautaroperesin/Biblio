using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Service.Interfaces;
using Service.Utils;

namespace Service.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        protected readonly JsonSerializerOptions _options;
        public static string? token;

        public GenericService(HttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _endpoint = Properties.Resources.urlApi + ApiEndpoints.GetEndpoint(typeof(T).Name);

            if(!string.IsNullOrEmpty(GenericService<object>.token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenericService<object>.token);
            }
            else
            {
                throw new Exception(token);
            }
        }

        public async Task<T?> AddAsync(T? entity)
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, entity);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al agregar el dato: {response.StatusCode} - {content}");
            }
            return JsonSerializer.Deserialize<T>(content, _options);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al eliminar el dato: {response.StatusCode}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<List<T>?> GetAllAsync(string? filtro = "")
        {
            var response = await _httpClient.GetAsync($"{_endpoint}?filtro={filtro}");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(content, _options);
            }
            else
            {
                throw new Exception("Error al obtener los datos.");
            }
        }

        public async Task<List<T>?> GetAllDeletedsAsync()
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/deleteds");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener los datos: {response.StatusCode}");
            }
            return JsonSerializer.Deserialize<List<T>>(content, _options);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al obtener los datos: {response.StatusCode}");
            }
            return JsonSerializer.Deserialize<T>(content, _options);
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var response = await _httpClient.PutAsync($"{_endpoint}/restore/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al restaurar el dato: {response.StatusCode}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(T? entity)
        {
            var idValue = entity.GetType().GetProperty("Id").GetValue(entity);
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{idValue}", entity);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Hubo un problema al actualizar");
            }
            else
            {
                return response.IsSuccessStatusCode;
            }
        }
    }
}
