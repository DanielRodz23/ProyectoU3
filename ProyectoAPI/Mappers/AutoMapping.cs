using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProyectoAPI.Models.DTOs;
using ProyectoAPI.Models.Entities;

namespace ProyectoAPI.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Actividades, ActividadesDTO>();
            CreateMap<ActividadesDTO, Actividades>();

            CreateMap<Departamentos, DepartamentosDTO>();
            CreateMap<DepartamentosDTO, Departamentos>();

    //         CreateMap<Pago, DashPago>()
    // .ForMember(dest => dest.JugadorNavigation, opt => opt.MapFrom(src => src.IdJugadorNavigation.Nombre))
    // .ForMember(dest => dest.ResponsableNavigation, opt => opt.MapFrom(src => src.IdResponsableNavigation.Nombre));

        }
    }
}