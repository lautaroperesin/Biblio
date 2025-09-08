using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppMovil.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isBusy;
        private string _errorMessage = string.Empty;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); ((Command)LoginCommand).ChangeCanExecute(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); ((Command)LoginCommand).ChangeCanExecute(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); ((Command)LoginCommand).ChangeCanExecute(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin, CanLogin);
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

                // Simulación de carga (opcional, puedes reducir o quitar)
                await Task.Delay(500);

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
