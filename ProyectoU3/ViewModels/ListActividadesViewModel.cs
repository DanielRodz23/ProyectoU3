using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Helpers;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Models.Entities;
using ProyectoU3.Repositories;
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
    public partial class ListActividadesViewModel: ObservableObject
    {
        public ListActividadesViewModel(ActividadesService actividadesService, ActividadesRepository actividadesRepository, DetallesViewModel detallesViewModel)
        {
            this.actividadesService = actividadesService;
            this.actividadesRepository = actividadesRepository;
            this.detallesViewModel = detallesViewModel;
            App.ActividadesService.DatosActualizados += ActividadesService_DatosActualizados;

            FillList();
        }

        private void ActividadesService_DatosActualizados()
        {
            FillList();
        }

        [ObservableProperty]
        string error;
        private readonly ActividadesService actividadesService;
        private readonly ActividadesRepository actividadesRepository;
        private readonly DetallesViewModel detallesViewModel;

        public ObservableCollection<Actividades> ListaActividades { get; set; } = new();
        void FillList()
        {
            ListaActividades.Clear();
            var acts = actividadesRepository.GetAll().OrderByDescending(x=>x.fechaRealizacion);
            foreach (var item in acts)
            {
                if (item.estado==(int)Estado.Publicado)
                    ListaActividades.Add(item);
            }
            //var tkn =  SecureStorage.GetAsync("tkn").Result;

            //var list = await actividadesService.GetActividades(tkn);
            //if (list == null) { Error = "Error al obtener la lista"; return; }
            //ListaActividades.Clear();
            //foreach (var item in list)
            //{
            //    ListaActividades.Add(item);
            //}
        }
        [RelayCommand]
        void Agregar()
        {
            Shell.Current.GoToAsync("//AgregarActividadView");
        }
        [RelayCommand]
        void AgregarDepartamento()
        {
            Shell.Current.GoToAsync("//AgregarDepartamentoView");
        }
        [RelayCommand]
        async void VerActividad(int id)
        {
            detallesViewModel.CargarDetalles(id);
            await Shell.Current.GoToAsync("//" + nameof(VerDetallesActividadView));
        }
        [RelayCommand]
        void VerBorradores()
        {
            Shell.Current.GoToAsync("//VerBorrador");
        }

    }
}
