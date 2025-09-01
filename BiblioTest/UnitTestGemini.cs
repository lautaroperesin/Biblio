using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Service.Services;

namespace BiblioTest
{
    public class UnitTestGemini
    {
        [Fact]
        public async void TestObtenerResumenLibroConIA()
        {
            var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            var apiKey = configuration["ApiKeyGemini"];

            var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key= " + apiKey;

            var prompt = $"Me puedes dar un resumen de 100 palabras como máximo de el libro Habitos Atomicos.";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
            };


            var json = JsonSerializer.Serialize(payload);
            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var texto = doc.RootElement
               .GetProperty("candidates")[0]
               .GetProperty("content")
               .GetProperty("parts")[0]
               .GetProperty("text")
               .GetString();

            Console.WriteLine($"Respuesta de IA: {texto}");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TestServiceGeminiGetPrompt()
        {
            var configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .Build();
            var prompt = $"Me puedes dar un resumen de 100 palabras como máximo del libro Sin Red: Nadal, Federer y la historia detrás del duelo que cambió el tenis";
            var servicio = new GeminiService(configuration);
            var resultado = await servicio.GetPromptResponse(prompt);
            Assert.NotNull(resultado);
        }
    }
}