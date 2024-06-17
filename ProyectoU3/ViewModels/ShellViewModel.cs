using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;
using ProyectoU3.Services;
using ProyectoU3.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        private readonly DepartamentosService adminService;

        public ShellViewModel(DepartamentosService adminService)
        {
            this.adminService = adminService;

            //admincheck = new Thread(CheckAdmin) { IsBackground = true };
            //admincheck.Start();
        }
        Thread admincheck;

        [ObservableProperty]
        bool isAdmin;

        async void CheckAdmin()
        {
            await Task.Run(async () =>
            {
                var tkn = SecureStorage.GetAsync("tkn").Result;
                while (tkn == null)
                {
                    tkn = SecureStorage.GetAsync("tkn").Result;
                    Thread.Sleep(500);
                }
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsAdmin = adminService.AdminCheckAsync(tkn).Result;
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Snackbar.Make("No hay internet, no se pudo comprobar rol de administrador").Show();
                    });
                }
            });
        }

        [RelayCommand]
        async Task CerrarSesion()
        {
            if (await Shell.Current.DisplayAlert("Cerrar sesión", "Seguro que quieres cerrar sesión?", "Si", "No"))
            {
                //Shell.Current.ToolbarItems.First().IsEnabled = false;
                //await Shell.Current.GoToAsync("//LoginView");

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    App.ActividadesService.ActividadesRepository.DeleteAll();
                    Preferences.Clear();
                    SecureStorage.RemoveAll();
                    App.Current.MainPage= App.LoginView;
                });
            }
        }
        public void Emrg()
        {
            App.ActividadesService.ActividadesRepository.DeleteAll();
            Preferences.Clear();
            SecureStorage.RemoveAll();
            App.Current.MainPage = App.LoginView;
        }

    }
}
