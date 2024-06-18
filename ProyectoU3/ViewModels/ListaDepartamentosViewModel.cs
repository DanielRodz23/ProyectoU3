using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using ProyectoU3.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ListaDepartamentosViewModel : ObservableObject
    {
        public ObservableCollection<DepartamentosDTO> Departamentos { get; set; } = new ObservableCollection<DepartamentosDTO>();
        DepartamentosService DepartamentosService = IPlatformApplication.Current?.Services.GetService<DepartamentosService>() ?? throw new Exception();
        public ListaDepartamentosViewModel()
        {
            CargarDepartamentos();
        }
        void CargarDepartamentos()
        {
            Departamentos.Clear();
            var depas = DepartamentosService.GetDepartments();
            int id = Preferences.Get("Id", 0);
            foreach (var depa in depas)
            {
                if (depa.Id != id)
                {
                    Departamentos.Add(depa);
                }
            }
        }
        [RelayCommand]
        async void EliminarDepartemento(int id)
        {
            if (id != 0)
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    if (await Shell.Current.DisplayAlert("Eliminar", "¿Seguro que quieres eliminar este departamento?", "Si", "No"))
                    {
                        var eliminado = DepartamentosService.DeleteDepartment(id);
                        if (eliminado)
                        {
                            Toast.Make("Departamento eliminado").Show();
                        }
                        else
                        {
                            Toast.Make("Hubo un problema al eliminarlo").Show();
                        }
                        CargarDepartamentos();
                        await Shell.Current.Navigation.PopAsync();
                    }
                }
                else
                {
                    Toast.Make("Sin internet\n Intente mas tarde").Show();
                }
            }
        }
        [RelayCommand]
        void VerAgregarDepartamento()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Toast.Make("Se necesita internet para realizar esta acción").Show();
                return;
            }
            var equis = IPlatformApplication.Current.Services.GetService<AgregarDepartamentoView>() ?? new AgregarDepartamentoView(IPlatformApplication.Current.Services.GetService<DepartamentosService>());
            var view = new AgregarDepartamento(equis);
            Shell.Current.Navigation.PushAsync(view);
        }
    }
}
