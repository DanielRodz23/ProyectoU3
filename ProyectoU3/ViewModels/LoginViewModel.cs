using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.LoginModels;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public LoginViewModel(LoginClient loginClient)
        {
            this.loginClient = loginClient;
            CheckTkn();
        }
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string error;
        private readonly LoginClient loginClient;
        async void CheckTkn()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            if (tkn == null) return;
            var Valido = await loginClient.Validar(tkn);
            if (Valido)
            {
                await Shell.Current.GoToAsync("//ListaActividadesView");
            }
            else
            {
                SecureStorage.RemoveAll();
            }
        }
        [RelayCommand]
        async void Login()
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                var token = await loginClient.GetToken(new LoginModel { username = Username, password = Password });
                if (token == null) Error = "Usuario o contraseña incorrectos";
                else
                {
                    await SecureStorage.SetAsync("tkn", token);
                    await Shell.Current.GoToAsync("//ListaActividadesView");
                }
            }
            else
            {
                Error = "Llenar campos";
            }

        }
    }
}
