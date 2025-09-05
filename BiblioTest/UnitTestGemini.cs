using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Service.DTOs;
using Service.Services;

namespace BiblioTest
{
    public class UnitTestGemini
    {
        [Fact]
        public async void TestObtenerResumenLibroConIA()
        {
            await LoginTest();

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

            await LoginTest();

            var prompt = $"Me puedes dar un resumen de 100 palabras como máximo del libro Deja de ser tú de Joe Dispenza";
            var servicio = new GeminiService(configuration);
            var resultado = await servicio.GetPromptResponse(prompt);
            Assert.NotNull(resultado);
        }

        private async Task LoginTest()
        {
            // Construimos la configuración para pasarsela a AuthService
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Primero nos autenticamos para obtener el token
            var serviceAuth = new AuthService(config);
            var token = await serviceAuth.Login(new LoginDTO
            {
                Username = "lautiperesin@gmail.com",
                Password = "1234lauti"
            });

            GeminiService.token = token;
        }
    }
}