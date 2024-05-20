using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoU3.Helpers;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Repositories;
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

        public ObservableCollection<ActividadesDTO> MisActividades { get; set; } = new();
        public ObservableCollection<ActividadesDTO> MisBorradores { get; set; } = new();
        //no se que sigue aqui, me daba errores 
        public VerBorradoresModel(ActividadesRepository actividadesRepository, IMapper mapper)
        {
            this.actividadesRepository = actividadesRepository;
            this.mapper = mapper;
            LlenarMisActividades();
            LlenarMisborradores();
        }

        private void LlenarMisborradores()
        {
            MisActividades.Clear();
            var cons = actividadesRepository.GetAll().Where(x=>x.estado==(int)Estado.Publicado);
            foreach (var item in cons)
            {
                MisActividades.Add(mapper.Map<ActividadesDTO>(item));
            }
        }

        private void LlenarMisActividades()
        {
            MisBorradores.Clear();
            var cons = actividadesRepository.GetAll().Where(x => x.estado == (int)Estado.Borrador);
            foreach (var item in cons)
            {
                MisBorradores.Add(mapper.Map<ActividadesDTO>(item));
            }
        }
    }
}
