using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirebaseAdmin.Auth;
using Service.Enums;
using Service.Models;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppMovil.ViewModels
{
    public partial class RegistrarseViewModel : ObservableObject
    {
        AuthService _authService = new();
        UsuarioService _usuarioService = new();

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string mail;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string verifyPassword;

        [ObservableProperty]
        private bool isBusy;
        public IRelayCommand VolverCommand { get; }
        public IRelayCommand RegistrarseCommand { get; }

        public RegistrarseViewModel()
        {
            RegistrarseCommand = new RelayCommand(Registrarse);
            VolverCommand = new AsyncRelayCommand(OnVolver);
        }

        private async void Registrarse()
        {
            if (IsBusy) return;
            IsBusy = true;

            if (Password != VerifyPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Registrarse", "Las contraseñas ingresadas no coinciden", "Ok");
                return;
            }

            try
            {
                var user = await _authService.CreateUserWithEmailAndPassword(Mail, Password, Nombre);
                if (user == false)
                {
                    await Application.Current.MainPage.DisplayAlert("Registrarse", "No se pudo crear el usuario", "OK");
                    return;
                }
                else
                {
                    var newUser = new Usuario
                    {
                        Nombre = Nombre,
                        Email = Mail,
                        Password = Password,
                        TipoRol = TipoRolEnum.Alumno,
                        Dni = "12345678",
                    };
                    await _usuarioService.AddAsync(newUser);
                    await Application.Current.MainPage.DisplayAlert("Registrarse", "Cuenta creada!", "Ok");
                    await Shell.Current.GoToAsync("//LoginPage");
                }

            }
            catch (FirebaseAuthException error) // Use alias here 
            {
                await Application.Current.MainPage.DisplayAlert("Registrarse", "Ocurrió un problema:" + error.Message, "Ok");
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
