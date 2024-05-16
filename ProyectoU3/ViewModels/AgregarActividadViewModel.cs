using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using ProyectoU3.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class AgregarActividadViewModel : ObservableObject
    {
        private readonly ActividadesService actividadesService;

        public AgregarActividadViewModel(ActividadesService actividadesService)
        {
            this.actividadesService = actividadesService;
            IsBorrador = false;
        }
        bool IsBorrador;
        [ObservableProperty] string errorTitulo;
        [ObservableProperty] string errorDescripcion;
        [ObservableProperty] string errorImagen;
        [ObservableProperty] string errorFechaRealizacion;
        [ObservableProperty] string errorGeneral;
        
        [ObservableProperty]
        ActividadesDTO actividad = new();

        [RelayCommand]
        void AgregarBorrador()
        {
            IsBorrador = true;
            AgregarActividad();
        }

        [RelayCommand]
        async void AgregarActividad()
        {
            //Validar actividad
            //[ObservableProperty] string errorTitulo;
            if (string.IsNullOrWhiteSpace(Actividad.titulo)) ErrorTitulo = "El titulo no es valido"; else { ErrorTitulo = "";  }
            //[ObservableProperty] string errorDescripcion;
            if (string.IsNullOrWhiteSpace(Actividad.descripcion)) ErrorDescripcion = "La descripcion no es valida"; else { ErrorDescripcion = ""; }
            //[ObservableProperty] string errorFechaRealizacion;
            var fecha = Actividad.fechaRealizacion??new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (fecha.ToDateTime(TimeOnly.MinValue) > DateTime.Now || fecha.ToDateTime(TimeOnly.MinValue)<new DateTime(1990,1,1)) ErrorFechaRealizacion = "La fecha no es valida"; else { ErrorFechaRealizacion = ""; }

            //TODO validar imagen
            ErrorImagen = "";

            var tkn = await SecureStorage.GetAsync("tkn");
            var actividadValidada = ErrorTitulo == "" && ErrorDescripcion == "" && ErrorImagen == "" && ErrorFechaRealizacion == "" && ErrorGeneral == "";
            if (actividadValidada && tkn!=null)
            {
                if (IsBorrador)
                {
                    Actividad.estado = 1;
                }
                else
                {
                    Actividad.estado = 2;
                }
                var Insertado = await actividadesService.InsertarActividad(tkn, Actividad);
                if (Insertado)
                {
                    await Shell.Current.GoToAsync("//" + nameof(ListActividadesView));
                }
                else
                {
                    ErrorGeneral = "Hubo un error en el envio de datos";
                }
            }

        }
    }
}
