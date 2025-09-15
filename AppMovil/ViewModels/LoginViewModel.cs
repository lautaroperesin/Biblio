using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Service.DTOs;
using Service.Services;

namespace AppMovil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        AuthService _authService;
        UsuarioService _usuarioService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string password = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            _usuarioService = new UsuarioService();
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
        }

        private bool CanLogin()
        {
            return !IsBusy && 
                   !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Password);
        }

        private async void OnLogin()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                var response = await _authService.Login(new LoginDTO
                {
                    Username = Username,
                    Password = Password
                });

                if (!response)
                {
                    ErrorMessage = "Credenciales inválidas. Intente nuevamente.";
                    return;
                }

                var usuario = await _usuarioService.GetByEmailAsync(username);

                if (usuario == null)
                {
                    ErrorMessage = "No se pudo obtener la información del usuario.";
                    return;
                }

                Preferences.Set("UserLoginId", usuario.Id);

                if (Application.Current?.MainPage is AppShell shell)
                {
                    shell.SetLoginState(true);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al iniciar sesión: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnErrorMessageChanged(string? oldValue, string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                // Aquí puedes implementar lógica adicional si es necesario
                // Por ejemplo, mostrar una alerta o notificación
                Application.Current?.MainPage?.DisplayAlert("Error de inicio de sesión", newValue, "OK");
            }
        }
    }
}
