using AutoMapper;
using CommunityToolkit.Maui.Alerts;
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
            var cons = actividadesRepository.GetAll().Where(x => x.estado == (int)Estado.Borrador && x.idDepartamento == id);
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
        [RelayCommand]
        void VerBorradores(int id)
        {

        }

        [RelayCommand]
        async Task EliminarActividadOrBorrador(int id)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                if (await Shell.Current.DisplayAlert("Eliminar", "¿Seguro que quiere eliminarlo?", "Sí", "No"))
                {
                    var act = actividadesRepository.Get(id);
                    var dto = mapper.Map<ActividadesDTO>(act);

                    dto.estado = (int)Estado.Eliminado;

                    var equis = await actividadesService.Update(dto);
                    if (equis)
                    {
                        actividadesRepository.Delete(act);
                        actividadesService.Invoke();
                        ActividadesService_DatosActualizados();
                    }
                    else
                    {
                        await Toast.Make("Hubo un problema al publicar").Show();
                    }
                }
            }
            else
            {
                await Toast.Make("No hay internet").Show();
            }
        }
        [RelayCommand]
        async Task PublicarBorrador(int id)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (await Shell.Current.DisplayAlert("Publicar", "¿Seguro que quiere publicar este borrador?", "Sí", "No"))
                {
                    var act = actividadesRepository.Get(id);
                    var dto = mapper.Map<ActividadesDTO>(act);


                    dto.estado = (int)Estado.Publicado;

                    var equis = await actividadesService.Update(dto);
                    if (equis)
                    {
                        act.estado = (int)Estado.Publicado;
                        actividadesRepository.Update(act);
                        actividadesService.Invoke();
                        ActividadesService_DatosActualizados();
                    }
                    else
                    {
                        await Toast.Make("Hubo un problema al eliminar").Show();
                    }
                }
            }
            else
            {
                await Toast.Make("No hay internet").Show();
            }
        }
        [RelayCommand]
        async Task CambiarImagen(int id)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (await Shell.Current.DisplayAlert("Cambiar imagen", "¿Seguro que quiere cambiar la imagen para esta publicación?", "Sí", "No"))
                {
                    var foto = await PedirFoto();
                    await Toast.Make("Cargando imagen").Show();
                    if (foto != null)
                    {
                        var sino = await actividadesService.UploadImagen(id, foto);
                        if (sino)
                        {
                            await Toast.Make("Imagen cargada").Show();
                            actividadesService.Invoke();
                        }
                        else
                        {
                            await Toast.Make("Error al obtener imagen").Show();
                        }
                    }
                    else
                    {
                        await Toast.Make("Error al obtener imagen").Show();
                    }
                }
            }
            else
            {
                await Toast.Make("No hay internet").Show();
            }
        }
        async Task<string> PedirFoto()
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
                return "";

            var stream = await result.OpenReadAsync();
            var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            memoryStream.Close();
            return Convert.ToBase64String(imageBytes);
        }

        [RelayCommand]
        void GoBack()
        {
            Shell.Current.GoToAsync("//ListaActividadesView");
        }

    }
}
