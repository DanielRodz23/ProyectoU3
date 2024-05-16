using CommunityToolkit.Mvvm.ComponentModel;
using ProyectoU3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.ViewModels
{
    public partial class ListBorradoresViewModel: ObservableObject
    {
        private readonly ActividadesService actividadesService;

        public ListBorradoresViewModel(ActividadesService actividadesService)
        {
            this.actividadesService = actividadesService;
        }

    }
}
