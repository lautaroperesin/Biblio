using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Service.DTOs;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Service.Services;
using Microsoft.VisualStudio.TestPlatform.TestHost;

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
            // Primero nos autenticamos para obtener el token
            var serviceAuth = new AuthService();
            var token = await serviceAuth.Login(new LoginDTO
            {
                Username = "lautiperesin@gmail.com",
                Password = "1234lauti"
            });
        }

        [Fact]
        public async Task TestReconocerPortadaGeminiController()
        {
            // Autenticación (si tu API requiere token, obténlo aquí)
            await LoginTest();

            // Ruta de la imagen de prueba (debe existir en la carpeta del proyecto)
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "portada_test.jpg");
            Assert.True(File.Exists(imagePath), $"No se encontró la imagen de prueba: {imagePath}");

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7000/"); // Cambia el puerto si tu backend usa otro

            // Si necesitas token:
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var form = new MultipartFormDataContent();
            using var imageStream = File.OpenRead(imagePath);
            var imageContent = new StreamContent(imageStream);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(imageContent, "Image", "portada_test.jpg");

            // Puedes agregar otros campos si BookCoverExtractionRequestDTO los requiere

            var response = await client.PostAsync("api/gemini/ocr-portada", form);
            var result = await response.Content.ReadAsStringAsync();

            Assert.True(response.IsSuccessStatusCode, $"Error en la API: {result}");

            // Deserializa el resultado
            var metadata = JsonSerializer.Deserialize<BookMetaDataDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(metadata);
            Assert.False(string.IsNullOrWhiteSpace(metadata.Titulo));
            Assert.NotNull(metadata.Autores);
            Assert.NotNull(metadata.Editorial);
        }


    }
}