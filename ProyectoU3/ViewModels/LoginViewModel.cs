using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Platform;
using ProyectoU3.Models.LoginModels;
using ProyectoU3.Repositories;
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

        public LoginViewModel(LoginClient loginClient, ActividadesService actividadesService, ActividadesRepository actividadesRepository)
        {
            this.loginClient = loginClient;
            this.actividadesService = actividadesService;
            this.actividadesRepository = actividadesRepository;
            //CheckTkn();
            IsEnabled = true;

            var dato = actividadesRepository.GetAll().Count();
        }
        [ObservableProperty]
        string username = "director@realmail.com";
        [ObservableProperty]
        string password = "Admina1";
        [ObservableProperty]
        string error;
        [ObservableProperty]
        bool isEnabled;
        private readonly LoginClient loginClient;
        private readonly ActividadesService actividadesService;
        private readonly ActividadesRepository actividadesRepository;

        async void CheckTkn()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            if (tkn == null) return;
            await Shell.Current.GoToAsync("//ListaActividadesView");
            Shell.Current.ToolbarItems.First().IsEnabled = true;
            //var Valido = await loginClient.Validar(tkn);
            //if (Valido)
            //{
            //    await Shell.Current.GoToAsync("//ListaActividadesView");
            //    Shell.Current.ToolbarItems.First().IsEnabled = true;
            //}
            //else
            //{
            //    SecureStorage.RemoveAll();
            //}
        }
        [RelayCommand]
        async void Login()
        {
            IsEnabled = false;
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                var token = await loginClient.GetToken(new LoginModel { username = Username.ToLower(), password = Password });
                if (token == null) Error = "Usuario o contraseña incorrectos";
                else
                {
                    await SecureStorage.SetAsync("tkn", token);
                    App.Current.MainPage = new AppShell();
                    //actividadesService.GetAllAsync().Wait();
                    await Shell.Current.GoToAsync("//ListaActividadesView");
                    //Shell.Current.ToolbarItems.Add(new ToolbarItem() { Text = "Cerrar sesion", Order = ToolbarItemOrder.Secondary, Command = (Shell.Current.BindingContext as ShellViewModel).CerrarSesionCommand });
                    Shell.Current.ToolbarItems.First().IsEnabled = true;

                    Username = "";
                    Password = "";
                    Error = "";
                }
            }
            else
            {
                Error = "Llenar campos";
            }
            IsEnabled = true;
        }
    }
}
