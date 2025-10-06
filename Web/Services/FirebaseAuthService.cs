using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Service.Models.Login;
using Service.Services;

namespace WebBlazor.Services
{
    public class FirebaseAuthService
    {
        private readonly IJSRuntime _jsRuntime;
        public event Action OnChangeLogin;
        public FirebaseUser CurrentUser { get; set; }

        public FirebaseAuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<FirebaseUser?> SignInWithEmailPassword(string email, string password, bool rememberPassword)
        {
            var user = await _jsRuntime.InvokeAsync<FirebaseUser?>("firebaseAuth.signInWithEmailPassword", email, password, rememberPassword);
            if (user != null)
            {
                CurrentUser = user;
                OnChangeLogin?.Invoke();
            }
            return user;
        }

        public async Task<string> createUserWithEmailAndPassword(string email, string password, string displayName)
        {
            var userId = await _jsRuntime.InvokeAsync<string>("firebaseAuth.createUserWithEmailAndPassword", email, password, displayName);
            if (userId != null)
            {
                OnChangeLogin?.Invoke();
            }
            return userId;
        }

        public async Task SignOut()
        {
            await _jsRuntime.InvokeVoidAsync("firebaseAuth.signOut");
            CurrentUser = null;
            OnChangeLogin?.Invoke();
        }

        public async Task<FirebaseUser?> GetUserFirebase()
        {
            var userFirebase = await _jsRuntime.InvokeAsync<FirebaseUser>("firebaseAuth.getUserFirebase");
            if (userFirebase != null && userFirebase.EmailVerified)
            {
                CurrentUser = userFirebase;
                return userFirebase;
            }
            else
            {
                CurrentUser = null;
                return null;
            }
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var user = await GetUserFirebase();
            if(user != null)
            {
                await SetUserToken();
                OnChangeLogin?.Invoke();
            }
            return user != null;
        }

        public async Task SetUserToken()
        {
            var token = await _jsRuntime.InvokeAsync<string>("firebaseAuth.getUserToken");
            if (token != null)
            {
                GenericService<object>.token = token;
            }
        }

        public async Task<FirebaseUser?> LoginWithGoogle()
        {
            var userFirebase = await _jsRuntime.InvokeAsync<FirebaseUser>("firebaseAuth.loginWithGoogle");
            CurrentUser = userFirebase;
            OnChangeLogin?.Invoke();
            return userFirebase;
        }

        public async Task<bool> RecoveryPassword(string email)
        {
            return await _jsRuntime.InvokeAsync<bool>("firebaseAuth.recoveryPassword", email);
        }
    }
}