using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using Service.Services;
using Service.DTOs;

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
        public IRelayCommand ForgotPasswordCommand { get; }
        public IRelayCommand RegisterCommand { get; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            _usuarioService = new UsuarioService();
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
            ForgotPasswordCommand = new RelayCommand(OnForgotPassword);
            RegisterCommand = new RelayCommand(OnRegister);
        }

        private void OnRegister()
        {
            if (Application.Current?.MainPage is AppShell shell)
            {
                shell.ViewModel.RegisterVisible = true;
                shell.SetLoginState(false);
            }
        }

        private async void OnForgotPassword()
        {
            if (Application.Current?.MainPage is AppShell shell)
            {
                shell.ViewModel.ResetPasswordVisible = true;
                shell.SetLoginState(false);
            }
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

                if (response != null)
                {
                    // Si la respuesta no es null, significa que hubo un error
                    ErrorMessage = response;
                    return;
                }

                var usuario = await _usuarioService.GetByEmailAsync(username);
                if (usuario == null)
                {
                    ErrorMessage = "No se pudo obtener la información del usuario.";
                    return;
                }
                Preferences.Set("UserLoginId", usuario.Id);

                // PERMITE CUALQUIER USUARIO/CONTRASEÑA durante desarrollo
                // Solo requiere que no estén vacíos
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
                // Mostrar una alerta cuando ErrorMessage cambie a un valor no vacío
                Application.Current?.MainPage?.DisplayAlert("Error de Login", newValue, "OK");
            }
        }
    }
}