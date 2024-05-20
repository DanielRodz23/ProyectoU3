using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Repositories;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class DetallesViewModel : ObservableObject
    {
        private readonly ActividadesService actividadesService;
        private readonly IMapper mapper;
        private readonly ActividadesRepository actividadesRepository;
      
        [ObservableProperty]
        ActividadesDTO actividadesDTO = new();
        public DetallesViewModel( ActividadesService actividadesService, IMapper mapper, ActividadesRepository actividadesRepository)
        {
            this.actividadesService = actividadesService;
            this.mapper = mapper;
            this.actividadesRepository = actividadesRepository;
        }
        public void CargarDetalles(int id)
        {
            var activ = actividadesRepository.Get(id);
            ActividadesDTO = mapper.Map<ActividadesDTO>(activ) ;
        }
        [RelayCommand]
        async Task Regresar()
        {
            await Shell.Current.GoToAsync("//ListaActividadesView");
        }
    }
}
