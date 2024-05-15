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
    public partial class LoginViewModel(LoginClient loginClient) : ObservableObject
    {
        private readonly LoginClient loginClient = loginClient;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string error;
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
                    await Shell.Current.GoToAsync("");
                }
            }
            else
            {
                Error = "Llenar campos";
            }

        }
    }
}
