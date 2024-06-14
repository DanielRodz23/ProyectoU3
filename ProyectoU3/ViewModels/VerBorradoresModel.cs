using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Helpers;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Repositories;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class VerBorradoresModel : ObservableObject
    {
        private readonly ActividadesRepository actividadesRepository;
        private readonly IMapper mapper;
        private readonly ActividadesService actividadesService;

        public ObservableCollection<ActividadesDTO> MisActividades { get; set; } = new();
        public ObservableCollection<ActividadesDTO> MisBorradores { get; set; } = new();
        //no se que sigue aqui, me daba errores 
        public VerBorradoresModel(ActividadesRepository actividadesRepository, IMapper mapper, ActividadesService actividadesService)
        {
            this.actividadesRepository = actividadesRepository;
            this.mapper = mapper;
            this.actividadesService = actividadesService;

            actividadesService.DatosActualizados += ActividadesService_DatosActualizados;

            LlenarMisActividades();
            LlenarMisborradores();
        }

        private void ActividadesService_DatosActualizados()
        {
            LlenarMisActividades();
            LlenarMisborradores();
        }

        private void LlenarMisborradores()
        {
            int id = Preferences.Get("Id", 0);
            MisBorradores.Clear();
            var cons = actividadesRepository.GetAll().Where(x=>x.estado==(int)Estado.Borrador && x.idDepartamento == id);
            int can = cons.Count();
            foreach (var item in cons)
            {
                MisBorradores.Add(mapper.Map<ActividadesDTO>(item));
            }
        }

        private void LlenarMisActividades()
        {
            int id = Preferences.Get("Id", 0);
            MisActividades.Clear();
            var cons = actividadesRepository.GetAll().Where(x => x.estado == (int)Estado.Publicado && x.idDepartamento == id);
            int can = cons.Count();
            foreach (var item in cons)
            {
                var maped = mapper.Map<ActividadesDTO>(item);
                MisActividades.Add(maped);
            }
        }

        [RelayCommand]
        void VerActividadOrBorrador(int id)
        {

        }

        //si
        [RelayCommand]
        void VerBorradores(int id)
        {

        }
        [RelayCommand]
        void GoBack()
        {
            Shell.Current.GoToAsync("//ListaActividadesView");
        }

    }
}
