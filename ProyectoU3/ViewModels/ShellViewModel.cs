using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ShellViewModel:ObservableObject
    {
        [RelayCommand]
         async Task CerrarSesion()
        {
            if (await Shell.Current.DisplayAlert("Cerrar sesión", "Seguro que quieres cerrar sesión?", "Si", "No"))
            {
                Shell.Current.ToolbarItems.First().IsEnabled = false;
                await Shell.Current.GoToAsync("//LoginView");
                SecureStorage.RemoveAll();
            }
        }

    }
}
