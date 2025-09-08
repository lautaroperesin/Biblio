using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace AppMovil.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        private bool _isLoggedIn;
        private bool _loginVisible = true;
        private bool _menuVisible = false;

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set 
            { 
                _isLoggedIn = value;
                LoginVisible = !value;
                MenuVisible = value;
                OnPropertyChanged();
            }
        }

        public bool LoginVisible
        {
            get => _loginVisible;
            set { _loginVisible = value; OnPropertyChanged(); }
        }

        public bool MenuVisible
        {
            get => _menuVisible;
            set { _menuVisible = value; OnPropertyChanged(); }
        }

        public ICommand LogoutCommand { get; }

        public AppShellViewModel()
        {
            LogoutCommand = new Command(OnLogout);
        }

        public void SetLoginState(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
            if (isLoggedIn)
                Shell.Current.GoToAsync("//MainPage");  // Cambio a MainPage (pantalla de inicio)
            else
                Shell.Current.GoToAsync("//LoginPage");
        }

        private void OnLogout()
        {
            SetLoginState(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}