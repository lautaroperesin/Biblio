using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GeminiController : ControllerBase
    {

        [HttpGet("prompt/{textPrompt}")]
        public async Task<IActionResult> GetPromt(string textPrompt)
        {
            try
            {
                //leemos la api key desde appsettings.json
                var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .Build();
                var apiKey = configuration["ApiKeyGemini"];
                var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key= " + apiKey;

                var payload = new
                {
                    contents = new[]
                    {
                    new
                    {
                        parts = new[]
                        {
                            new { text = textPrompt }
                        }
                    }
                }
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
                //Console.WriteLine($"Respuesta de IA: {texto}");
                return Ok(texto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }

        /// <summary>
        /// Envía una imagen de portada y devuelve metadatos estructurados del libro.
        /// </summary>
        [HttpPost("ocr-portada")]
        [Consumes("image/jpeg", "image/png", "application/octet-stream")]
        [Produces("application/json")]
        [RequestSizeLimit(20_000_000)] // 20 MB
        public async Task<ActionResult<BookMetaDataDTO>> ReconocerPortada([FromQuery] string imageUrl, CancellationToken ct)
        {
            if(string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest("La URL de la imagen no puede estar vacía.");
            }

            // Lee el body a memoria (si preferís no cargar todo, podés streamear a archivo temporal)
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                //await Request.Body.CopyToAsync(ms, ct);
                using (var httpClient = new HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                }
                bytes = ms.ToArray();
            }

            // Determina MIME
            var mime = DetectMime(Request.ContentType, bytes);
            if (mime is null || (mime != "image/jpeg" && mime != "image/png"))
                return BadRequest("La imagen debe ser JPEG o PNG.");

            // Modelo y API key (appsettings.json → "Gemini:ApiKey")
            var configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables()
                     .Build();
            var apiKey = configuration["ApiKeyGemini"];
            var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key= " + apiKey;
            if (string.IsNullOrWhiteSpace(apiKey))
                return StatusCode(500, "Falta configurar Gemini:ApiKey.");

            var base64 = Convert.ToBase64String(bytes);

            // Prompt en español + salida JSON estricta (response_schema)
            var prompt = """
Eres un asistente experto en catalogación bibliográfica. Analiza EXCLUSIVAMENTE el contenido visible de la portada (título, subtítulo, autora/es, editorial, edición, año, idioma, ISBN visibles).
- Devuelve SOLO lo que se ve con claridad en la imagen.
- Si un campo no está en la portada, devuélvelo como null o lista vacía.
- No inventes datos.
""";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new object[]
                        {
                            new { text = prompt },
                            new {
                                inline_data = new {
                                    mime_type = mime,
                                    data = base64
                                }
                            }
                        }
                    }
                },
                generationConfig = new
                {
                    response_mime_type = "application/json",
                    // JSON Schema mínimo para guiar la extracción (Structured Output)
                    response_schema = new
                    {
                        type = "object",
                        properties = new Dictionary<string, object>
                        {
                            ["Titulo"] = new { type = "string", nullable = true },
                            ["Subtitulo"] = new { type = "string", nullable = true },
                            ["Autores"] = new { type = "array", items = new { type = "string" } },
                            ["Editorial"] = new { type = "string", nullable = true },
                            ["Anio"] = new { type = "integer", nullable = true },
                            ["Isbn"] = new { type = "array", items = new { type = "string" } },
                            ["Edicion"] = new { type = "string", nullable = true },
                            ["Idioma"] = new { type = "string", nullable = true },
                            ["Confianza"] = new { type = "number", nullable = true }
                        },
                        required = new[] { "Titulo", "Autores", "Editorial", "Isbn" }
                    }
                }
            };

            var http = new HttpClient();
            using var msg = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };
            msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var resp = await http.SendAsync(msg, ct);
            var json = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode, $"Gemini error: {json}");

            // La respuesta de Gemini trae candidates[0].content.parts[0].text con el JSON
            using var doc = JsonDocument.Parse(json);
            string? jsonPayload =
                doc.RootElement
                   .GetProperty("candidates")[0]
                   .GetProperty("content")
                   .GetProperty("parts")[0]
                   .GetProperty("text")
                   .GetString();

            if (string.IsNullOrWhiteSpace(jsonPayload))
                return StatusCode(502, "No se recibió salida JSON del modelo.");

            BookMetaDataDTO? data;
            try
            {
                data = JsonSerializer.Deserialize<BookMetaDataDTO>(jsonPayload, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // Si el modelo devolvió JSON con campos inesperados, devolvemos el raw para diagnóstico
                return StatusCode(502, $"Error parseando JSON del modelo: {ex.Message}. Raw: {jsonPayload}");
            }

            return Ok(data);
        }

        // Detección simple de MIME con fallback por magic numbers
        private static string? DetectMime(string? contentType, byte[] bytes)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                var ct = contentType.Split(';')[0].Trim().ToLowerInvariant();
                if (ct is "image/jpeg" or "image/jpg" or "image/png") return ct == "image/jpg" ? "image/jpeg" : ct;
                if (ct == "application/octet-stream")
                {
                    // cae a magic numbers
                }
            }

            // JPEG: FF D8 FF
            if (bytes.Length > 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) return "image/jpeg";
            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (bytes.Length > 8 &&
                bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 &&
                bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A) return "image/png";

            return null;
        }

    }
}