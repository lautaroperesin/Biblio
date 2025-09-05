using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using Service.Utils;

namespace Service.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = new HttpClient();
        public static string? token;

        public GeminiService(IConfiguration configuration)
        {
            _configuration = configuration;
            if (!string.IsNullOrEmpty(GeminiService.token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GeminiService.token);
            }
            else
            {
                throw new Exception(token);
            }
        }

        public async Task<string?> GetPromptResponse(string textPrompt)
        {
            if (string.IsNullOrWhiteSpace(textPrompt))
            {
                throw new ArgumentException("El prompt no puede estar vacío.");
            }

            try
            {
                var urlApi = _configuration["UrlApi"];
                var endpointGemini = ApiEndpoints.GetEndpoint("Gemini");
                var response = await _httpClient.GetAsync($"{urlApi}{endpointGemini}/prompt/{textPrompt}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al obtener respuesta de Gemini: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio Gemini: {ex.Message}");
            }
        }
    }
}
