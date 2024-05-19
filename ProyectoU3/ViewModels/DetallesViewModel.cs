using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    [QueryProperty("Id", "id")]
    public partial class DetallesViewModel : ObservableObject
    {
        private readonly ActividadesService actividadesService;
        [ObservableProperty]
        int id;
        [ObservableProperty]
        ActividadesDTO actividadesDTO = new();
        public DetallesViewModel( ActividadesService actividadesService)
        {
            this.actividadesService = actividadesService;
            CargarDetalles();
        }
        async Task CargarDetalles()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            ActividadesDTO = await actividadesService.GetActividadOrBorrador(tkn, Id);
        }
    }
}
