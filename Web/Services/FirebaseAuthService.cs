using Firebase.Auth;
using Microsoft.JSInterop;

namespace WebBlazor.Services
{
    public class FirebaseAuthService
    {
        private readonly IJSRuntime _jsRuntime;
        public event Action OnChangeLogin;
        public UserInfo CurrentUser { get; set; }

        public FirebaseAuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<UserInfo?> SignInWithEmailPassword(string email, string password, bool rememberPassword)
        {
            // Ingresar el usuario y contraseña en Firebase
            var user = await _jsRuntime.InvokeAsync<UserInfo?>("firebaseAuth.signInWithEmailPassword", email, password, rememberPassword);
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

        public async Task<UserInfo?> GetUserFirebase()
        {
            var userFirebase = await _jsRuntime.InvokeAsync<UserInfo>("firebaseAuth.getUserFirebase");
            CurrentUser = userFirebase;
            return userFirebase;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var user = await GetUserFirebase();
            return user != null;
        }

        public async Task<UserInfo?> LoginWithGoogle()
        {
            var userFirebase = await _jsRuntime.InvokeAsync<UserInfo>("firebaseAuth.loginWithGoogle");
            CurrentUser = userFirebase;
            OnChangeLogin?.Invoke();
            return userFirebase;
        }
    }
}