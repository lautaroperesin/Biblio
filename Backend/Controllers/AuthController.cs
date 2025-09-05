using Firebase.Auth;
using Firebase.Auth.Providers;
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

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            SetFirebaseConfig();
        }

        private void SetFirebaseConfig()
        {
            // Configuración de Firebase con proveedor de autenticación por correo electrónico y anónimo
            var config = new FirebaseAuthConfig
            {
                ApiKey = _configuration["ApiKeyFirebase"],
                AuthDomain = _configuration["AuthDomainFirebase"],
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()

                },
            };

            firebaseAuthClient = new FirebaseAuthClient(config);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                var credential = await firebaseAuthClient.SignInWithEmailAndPasswordAsync(login.Username, login.Password);
                return Ok(credential.User.GetIdTokenAsync().Result);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized(new { message = $"Credenciales inválidas. Error: {ex.Reason.ToString()}" });
            }
        }
    }
}
