using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Models;
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
        [HttpGet("ocr-portada")]
        [Produces("application/json")]
        public async Task<ActionResult<Libro>> ReconocerPortada([FromQuery] string imageUrl, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return BadRequest("El parámetro 'imageUrl' es obligatorio.");

            // Descarga la imagen desde la URL proporcionada
            byte[] bytes;
            string? remoteContentType;
            using (var httpClient = new HttpClient())
            using (var resp = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, ct))
            {
                if (!resp.IsSuccessStatusCode)
                {
                    var status = (int)resp.StatusCode;
                    return BadRequest($"No se pudo descargar la imagen. Código HTTP: {status}");
                }

                remoteContentType = resp.Content.Headers.ContentType?.MediaType;

                bytes = await resp.Content.ReadAsByteArrayAsync(ct);
            }

            // Determina MIME a partir de la respuesta remota y, en su defecto, por magic numbers
            var mime = DetectMime(remoteContentType, bytes);
            if (mime is null || (mime != "image/jpeg" && mime != "image/png"))
                return BadRequest("La imagen debe ser JPEG o PNG.");

            // Modelo y API key (appsettings.json → "ApiKeyGemini")
            var configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables()
                     .Build();
            var apiKey = configuration["ApiKeyGemini"];
            var endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=" + apiKey;
            if (string.IsNullOrWhiteSpace(apiKey))
                return StatusCode(500, "Falta configurar Gemini:ApiKey.");

            var base64 = Convert.ToBase64String(bytes);

            // Prompt mejorado en español + salida JSON estricta (response_schema)
            var prompt = """
Eres un asistente experto en catalogación bibliográfica con acceso a una base de datos mundial de libros. Tu tarea tiene TRES FASES OBLIGATORIAS:

**FASE 1 - ANÁLISIS DE LA PORTADA:**
Analiza meticulosamente el contenido visible de la portada (título, autor/es, editorial, año, páginas).
- Extrae TODO lo que se ve con claridad en la imagen.
- Lee cuidadosamente cada texto visible, incluso si es pequeño.
- Si ves el número de páginas en la portada, extráelo.

**FASE 2 - COMPLETAR INFORMACIÓN FALTANTE (OBLIGATORIO):**
Para CADA campo que NO aparece en la portada, DEBES intentar completarlo:
1. Identifica el libro por su título y autor/es
2. Busca en tu conocimiento la información faltante
3. Completa: editorial, año de publicación, páginas, descripción, sinopsis
4. Para PÁGINAS: número aproximado de páginas si conoces el libro
5. Para la DESCRIPCIÓN: frase corta y atractiva de máximo 20 palabras que capte la esencia del libro
6. Para la SINOPSIS: resumen detallado del libro de máximo 100 palabras
7. Si el libro tiene múltiples ediciones, elige la información más relevante

**FASE 3 - CLASIFICACIÓN TÉCNICA (OBLIGATORIO):**
Clasifica el libro según estándares bibliotecarios:

1. **CDU (Clasificación Decimal Universal) - MÁXIMO 6 DÍGITOS**:
   - Asigna el código CDU específico según la materia del libro
   - LÍMITE: máximo 6 dígitos (ej: 821.134, no 821.134.2-31)
   - Ejemplos de códigos válidos: 
     * 82 = Literatura (general)
     * 821 = Literatura por idiomas
     * 821.1 = Literatura inglesa
     * 821.13 = Literatura románica
     * 34 = Derecho
     * 5 = Ciencias naturales
     * 61 = Medicina
   - Para ficción literaria: usa máximo 6 dígitos del código 82

2. **PALABRAS CLAVE**:
   - Selecciona EXACTAMENTE 3 palabras clave relacionadas con el CDU asignado
   - Las palabras deben ser términos técnicos, temáticos o conceptuales del libro
   - Deben ser relevantes para búsquedas y clasificación
   - Ejemplos para "El Padrino": ["mafia", "familia", "poder"]

3. **LIBRISTICA**:
- Selecciona EXACTAMENTE las 3 primeras letras del APELLIDO del PRIMER autor o autora

**REGLAS DE PRIORIDAD:**
- La información de la portada SIEMPRE tiene prioridad sobre tu conocimiento
- Si hay conflicto entre portada y base de conocimiento: usa la portada
- Si la portada está incompleta: completa con tu conocimiento
- Para libros famosos: completa TODOS los campos incluyendo clasificación técnica
- Para libros desconocidos: indica null en los campos que no puedas inferir
- La clasificación CDU DEBE tener máximo 6 dígitos

**NIVEL DE CONFIANZA (0.0-1.0):**
- 1.0 = Todos los datos extraídos directamente de la portada
- 0.9 = Mayoría de portada + editorial/año inferidos con certeza
- 0.7-0.8 = Mezcla balanceada de portada + datos inferidos conocidos + clasificación técnica
- 0.5-0.6 = Título y autor de portada + resto inferido (incluyendo sinopsis y clasificación)
- 0.3-0.4 = Solo título visible + todo lo demás inferido
- 0.0-0.2 = Datos mayormente inferidos con baja certeza

**EJEMPLOS DE COMPLETADO:**
- Si ves "El Padrino" + "Mario Puzo": 
  * Páginas: 448
  * Descripción: "La saga épica de la familia Corleone y el mundo de la mafia italiana en Nueva York"
  * Sinopsis: "Don Vito Corleone es el respetado y temido jefe de una de las cinco familias de la mafia de Nueva York. Mientras construye su imperio criminal basado en la lealtad familiar y el respeto, debe enfrentar guerras entre familias, traiciones y el desafío de pasar el poder a su hijo Michael, quien inicialmente rechaza el negocio familiar pero eventualmente se convierte en un líder aún más despiadado."
  * CDU: "821.11" (Literatura inglesa y angloamericana - máx. 6 dígitos)
  * Palabras Clave: ["mafia", "familia", "poder"]
  
- Si ves "Cien años de soledad" + "García Márquez": 
  * Páginas: 471
  * Descripción: "La historia mítica de la familia Buendía a través de generaciones en Macondo"
  * Sinopsis: "Narra la historia de la familia Buendía a lo largo de siete generaciones en el pueblo ficticio de Macondo. Desde su fundación por José Arcadio Buendía hasta su trágico final, la novela mezcla realidad y fantasía, explorando temas de soledad, amor, guerra civil, y el paso inexorable del tiempo en un estilo de realismo mágico que revolucionó la literatura latinoamericana."
  * CDU: "821.13" (Literatura románica - máx. 6 dígitos)
  * Palabras Clave: ["realismo mágico", "Colombia", "generaciones"]

IMPORTANTE: Tu objetivo es devolver la mayor cantidad de información posible. NO dejes campos vacíos si conoces el libro. La descripción debe ser concisa (máx. 20 palabras), la sinopsis detallada pero no exceder 100 palabras, y el CDU debe tener máximo 6 dígitos.
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
                    // JSON Schema mejorado para guiar la extracción
                    response_schema = new
                    {
                        type = "object",
                        properties = new Dictionary<string, object>
                        {
                            ["Titulo"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "Título principal del libro extraído de la portada o inferido"
                            },
                            ["Autores"] = new
                            {
                                type = "array",
                                items = new { type = "string" },
                                description = "Lista de autores en el orden que aparecen. DEBE completarse si el libro es conocido"
                            },
                            ["Editorial"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "Editorial visible en portada o COMPLETAR si el libro es conocido (ej: Random House, Penguin, Sudamericana)"
                            },
                            ["Anio"] = new
                            {
                                type = "integer",
                                nullable = true,
                                description = "Año de publicación visible o COMPLETAR si el libro es conocido"
                            },
                            ["Paginas"] = new
                            {
                                type = "integer",
                                nullable = true,
                                description = "Número de páginas visible en portada o COMPLETAR si el libro es conocido"
                            },
                            ["Descripcion"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "Frase corta y atractiva de MÁXIMO 20 palabras que capte la esencia del libro. DEBE completarse si el libro es conocido"
                            },
                            ["Sinopsis"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "Resumen detallado del libro de MÁXIMO 100 palabras. DEBE completarse si el libro es conocido"
                            },
                            ["CDU"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "Código CDU (Clasificación Decimal Universal) de MÁXIMO 6 dígitos. Ej: 821.13 para literatura románica, 34 para derecho"
                            },
                            ["PalabrasClave"] = new
                            {
                                type = "array",
                                items = new { type = "string" },
                                description = "EXACTAMENTE 3 palabras clave relacionadas con el CDU y el contenido del libro"
                            },
                            ["Libristica"] = new
                            {
                                type = "string",
                                nullable = true,
                                description = "EXACTAMENTE las 3 primeras letras del APELLIDO del PRIMER autor o autora"
                            },
                            ["Confianza"] = new
                            {
                                type = "number",
                                nullable = false,
                                description = "Nivel de confianza OBLIGATORIO (0.0-1.0): 1.0=todo de portada, 0.5=mitad inferido, 0.0=todo inferido",
                                minimum = 0.0,
                                maximum = 1.0
                            }
                        },
                        required = new[] { "Titulo", "Autores", "Editorial", "Confianza" }
                    }
                }
            };

            var http = new HttpClient();
            using var msg = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };
            msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var resp2 = await http.SendAsync(msg, ct);
            var json = await resp2.Content.ReadAsStringAsync(ct);

            if (!resp2.IsSuccessStatusCode)
                return StatusCode((int)resp2.StatusCode, $"Gemini error: {json}");

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

            BookMetaDataDTO? bookMetadata;
            try
            {
                bookMetadata = JsonSerializer.Deserialize<BookMetaDataDTO>(jsonPayload, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // Si el modelo devolvió JSON con campos inesperados, devolvemos el raw para diagnóstico
                return StatusCode(502, $"Error parseando JSON del modelo: {ex.Message}. Raw: {jsonPayload}");
            }

            var libro = MapToLibro(bookMetadata);
            return Ok(libro);
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

        // Mapeamos BookMetaDataDTO a Libro
        private static Libro MapToLibro(BookMetaDataDTO dto)
        {
            var libro = new Libro
            {
                Titulo = dto.Titulo ?? string.Empty,
                LibrosAutores = dto.Autores != null
                    ? dto.Autores.Select(a => new LibroAutor { Autor = new Autor { Nombre = a } }).ToList()
                    : new List<LibroAutor>(),
                LibrosGeneros = dto.Generos != null
                    ? dto.Generos.Select(g => new LibroGenero { Genero = new Genero { Nombre = g } }).ToList()
                    : new List<LibroGenero>(),
                Editorial = dto.Editorial != null ? new Editorial { Nombre = dto.Editorial } : null,
                Descripcion = dto.Descripcion ?? string.Empty,
                Sinopsis = dto.Sinopsis ?? string.Empty,
                CDU = dto.CDU ?? string.Empty,
                Libristica = dto.Libristica ?? string.Empty,
                PalabrasClave = dto.PalabrasClave != null ? string.Join(", ", dto.PalabrasClave) : string.Empty,
                Paginas = dto.Paginas ?? 0,
                AnioPublicacion = dto.Anio ?? 0
            };
            return libro;
        }
    }
}