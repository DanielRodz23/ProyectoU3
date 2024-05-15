using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ListActividadesViewModel(ActividadesService actividadesService): ObservableObject
    {
        private readonly ActividadesService actividadesService = actividadesService;

        public ObservableCollection<ActividadesDTO> ListaActividades { get; set; } = new();

    }
}
