using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
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
        public ListActividadesViewModel(ActividadesService actividadesService, ActividadesRepository actividadesRepository, DetallesViewModel detallesViewModel, DepartamentosService adminService)
        {
            this.actividadesService = actividadesService;
            this.actividadesRepository = actividadesRepository;
            this.detallesViewModel = detallesViewModel;
            this.adminService = adminService;
            App.ActividadesService.DatosActualizados += ActividadesService_DatosActualizados;

            FillList();

            Thread admincheck = new Thread(CheckAdmin) { IsBackground = true };
            admincheck.Start();

        }

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


        private void ActividadesService_DatosActualizados()
        {
            FillList();
        }

        [ObservableProperty]
        string error;
        private readonly ActividadesService actividadesService;
        private readonly ActividadesRepository actividadesRepository;
        private readonly DetallesViewModel detallesViewModel;
        private readonly DepartamentosService adminService;

        public ObservableCollection<Actividades> ListaActividades { get; set; } = new();
        void FillList()
        {
            ListaActividades.Clear();
            var acts = actividadesRepository.GetAll().OrderByDescending(x=>x.fechaRealizacion);
            int cant = acts.Count();
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
        [ObservableProperty]
        bool departamentoButtonEnabled = true;
        [RelayCommand]
        async Task Agregar()
        {
            await Shell.Current.GoToAsync("//AgregarActividadView");
        }
        [RelayCommand]
        async Task AgregarDepartamento()
        {
            DepartamentoButtonEnabled = false;
            //En realidad este metodo nos lleva a ver los departamentos
            var vista = new ListaDepartamentosView();
            await Shell.Current.Navigation.PushAsync(vista);
            DepartamentoButtonEnabled = true;

        }
        [RelayCommand]
        async Task VerActividad(int id)
        {
            detallesViewModel.CargarDetalles(id);
            await Shell.Current.GoToAsync("//" + nameof(VerDetallesActividadView));
        }
        [RelayCommand]
        async Task VerBorradores()
        {
            await Shell.Current.GoToAsync("//VerBorrador");
        }

    }
}
