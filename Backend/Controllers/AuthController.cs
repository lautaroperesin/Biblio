using System.Net.Http.Headers;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        FirebaseAuthClient firebaseAuthClient;
        IConfiguration _configuration;
        FirebaseAuthConfig _config;
        //private const string RequestUri = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + FirebaseApiKey;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            SetFirebaseConfig();
        }

        private void SetFirebaseConfig()
        {
            // Configuración de Firebase con proveedor de autenticación por correo electrónico y anónimo
            _config = new FirebaseAuthConfig
            {
                ApiKey = _configuration["ApiKeyFirebase"],
                AuthDomain = _configuration["AuthDomainFirebase"],
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()

                },
            };

            firebaseAuthClient = new FirebaseAuthClient(_config);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                var credentials = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(login.Username, login.Password);
                if(credentials.User.Info.IsEmailVerified == false)
                {
                    return BadRequest("Email no verificado. Verifica tu correo electrónico antes de iniciar sesión.");
                }
                return Ok(credentials.User.GetIdTokenAsync().Result);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { message = $"Credenciales inválidas. Error: {ex.Reason.ToString()}" });
            }
        }

        // register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            try
            {
                var user = await firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(register.Email, register.Password, register.Nombre);
                // enviar email de verificación
                await SendVerificationEmailAsync(user.User.GetIdTokenAsync().Result);
                return Ok(user.User.GetIdTokenAsync().Result);
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(new { message = ex.Reason.ToString() });
            }
        }

        [Authorize]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] LoginDTO login)
        {
            try
            {
                await firebaseAuthClient.ResetEmailPasswordAsync(login.Username);
                return Ok();
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest(ex.Reason.ToString());
            }
        }

        private async Task SendVerificationEmailAsync(string idToken)
        {
            var RequestUri = "https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + _config.ApiKey;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent("{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + idToken + "\"}");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(RequestUri, content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
