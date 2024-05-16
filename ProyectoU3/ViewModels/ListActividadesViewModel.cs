using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ListActividadesViewModel: ObservableObject
    {
        public ListActividadesViewModel(ActividadesService actividadesService)
        {
            this.actividadesService = actividadesService;
            FillList();
        }
        

        [ObservableProperty]
        string error;
        private readonly ActividadesService actividadesService;

        public ObservableCollection<ActividadesDTO> ListaActividades { get; set; } = new();
        async void FillList()
        {
            var tkn =  SecureStorage.GetAsync("tkn").Result;
            var list = await actividadesService.GetActividades(tkn);
            if (list == null) { Error = "Error al obtener la lista"; return; }
            ListaActividades.Clear();
            foreach (var item in list)
            {
                ListaActividades.Add(item);
            }
        }
        [RelayCommand]
        void Agregar()
        {
            Shell.Current.GoToAsync("//AgregarActividadView");
        }

    }
}
