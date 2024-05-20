using AutoMapper;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<ActividadesDTO, Actividades>()
                .ForMember(dest=>dest.fechaRealizacion, opt=>opt.MapFrom(src=>new DateTime(src.fechaRealizacion.Value.Year, src.fechaRealizacion.Value.Month, src.fechaRealizacion.Value.Day)));
            CreateMap<Actividades, ActividadesDTO>();

            CreateMap<DateTime, DateOnly>()
            .ConvertUsing(src => DateOnly.FromDateTime(src));

            // Mapeo de DateOnly a DateTime
            CreateMap<DateOnly, DateTime>()
                .ConvertUsing(src => src.ToDateTime(TimeOnly.MinValue));

            // Otros mapeos aquí

            //.ForMember(dest => dest.Departamento,
            //    opt => opt
            //        .MapFrom(src => src.IdDepartamentoNavigation.Nombre));

        }
    }
}
