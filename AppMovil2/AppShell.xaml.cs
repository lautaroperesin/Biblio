using AppMovil.ViewModels;

namespace AppMovil
{
    public partial class AppShell : Shell
    {
        public AppShellViewModel ViewModel => (AppShellViewModel)BindingContext;

        public AppShell()
        {
            InitializeComponent();
        }

        // Método público para cambiar el estado de login desde otras páginas
        public void SetLoginState(bool isLoggedIn)
        {
            ViewModel.SetLoginState(isLoggedIn);
        }
    }
}
