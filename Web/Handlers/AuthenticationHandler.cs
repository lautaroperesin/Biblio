using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.Services;

namespace Web.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly FirebaseAuthService _firebaseAuthService;
        public AuthenticationHandler(FirebaseAuthService firebaseAuthService)
        {
            _firebaseAuthService = firebaseAuthService;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jwt = await _firebaseAuthService.GetUserToken();
            if (!string.IsNullOrEmpty(jwt))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
