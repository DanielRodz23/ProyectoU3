using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Helpers;
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
        //hola espero no regarla
        //aguas con esto jaja


        private readonly ActividadesService actividadesService;

        public AgregarActividadViewModel(ActividadesService actividadesService)
        {
            this.actividadesService = actividadesService;
            IsBorrador = false;
            Fecha = DateTime.Now;
        }
        bool IsBorrador;
        [ObservableProperty] string errorTitulo;
        [ObservableProperty] string errorDescripcion;
        [ObservableProperty] string errorImagen;
        [ObservableProperty] string errorFechaRealizacion;
        [ObservableProperty] string errorGeneral = "";

        [ObservableProperty]
        string base64Imagen;

        [ObservableProperty]
        DateTime fecha;

        [ObservableProperty]
        ActividadesDTO actividad = new();

        [RelayCommand]
        async Task PedirFoto()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "image/png" } }, // MIME type
            });

            var pickOptions = new PickOptions
            {
                PickerTitle = "Agregar Imagen",
                FileTypes = customFileType
            };

            var result = await FilePicker.PickAsync(pickOptions);

            if (result == null)
                return ;

            var stream = await result.OpenReadAsync();
            var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            memoryStream.Close();
            Base64Imagen = Convert.ToBase64String(imageBytes);
        }



        [RelayCommand]
        void AgregarBorrador()
        {
            IsBorrador = true;
            AgregarActividad();
        }

        [RelayCommand]
        async void AgregarActividad()
        {
            Actividad.fechaRealizacion = new DateOnly(Fecha.Year, Fecha.Month, Fecha.Day);
            //Validar actividad
            //[ObservableProperty] string errorTitulo;
            if (string.IsNullOrWhiteSpace(Actividad.titulo)) ErrorTitulo = "El titulo no es valido"; else { ErrorTitulo = ""; }
            //[ObservableProperty] string errorDescripcion;
            if (string.IsNullOrWhiteSpace(Actividad.descripcion)) ErrorDescripcion = "La descripcion no es valida"; else { ErrorDescripcion = ""; }
            //[ObservableProperty] string errorFechaRealizacion;
            //var fecha = Actividad.fechaRealizacion??new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (Fecha >= DateTime.Today || Fecha < new DateTime(1990, 1, 1)) ErrorFechaRealizacion = "La fecha no es valida"; else { ErrorFechaRealizacion = ""; }

            //TODO validar imagen
            ErrorImagen = "";
            ErrorGeneral = "";
            var tkn = await SecureStorage.GetAsync("tkn");
            var actividadValidada = (ErrorTitulo == "" && ErrorDescripcion == "" && ErrorImagen == "" && ErrorFechaRealizacion == "" && ErrorGeneral == "");
            if (actividadValidada && tkn != null)
            {
                if (IsBorrador)
                {
                    Actividad.estado = (int)Estado.Borrador;
                }
                else
                {
                    Actividad.estado = (int)Estado.Publicado;
                }
                
                var Insertado = await actividadesService.InsertarActividad(tkn, new InsertAct 
                {
                    Titulo = Actividad.titulo, 
                    Descripcion=Actividad.descripcion,
                    Anio=Actividad.fechaRealizacion.Value.Year,
                    Mes = Actividad.fechaRealizacion.Value.Month,
                    Dia = Actividad.fechaRealizacion.Value.Day
                });
                if (Insertado!=0)
                {
                    await PedirFoto();
                    await actividadesService.UploadImagen(Insertado, Base64Imagen);

                    await Shell.Current.GoToAsync("//ListaActividadesView");
                }
                else
                {
                    ErrorGeneral = "Hubo un error en el envio de datos";
                }

            }

        }
        [RelayCommand]
        void GoBack()
        {
            Shell.Current.GoToAsync("//ListaActividadesView");
        }
    }
}
