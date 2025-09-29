using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Service.Interfaces;
using Service.Services;

namespace AppMovil.ViewModels
{
    public partial class RecuperarPasswordViewModel : ObservableObject
    {
        AuthService _authService = new();

        [ObservableProperty]
        private string mail = string.Empty;

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string successMessage = string.Empty;

        public IRelayCommand EnviarCommand { get; }
        public IRelayCommand VolverCommand { get; }

        public RecuperarPasswordViewModel()
        {
            EnviarCommand = new AsyncRelayCommand(OnEnviar, CanEnviar);
            VolverCommand = new AsyncRelayCommand(OnVolver);
        }

        private bool CanEnviar()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(mail);
        }

        private async Task OnEnviar()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                // Validación básica de email
                if (!Mail.Contains("@") || !Mail.Contains("."))
                {
                    ErrorMessage = "Por favor, ingrese un correo electrónico válido";
                    return;
                }



                await OnVolver();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al enviar las instrucciones: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnVolver()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}