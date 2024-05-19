using ProyectoU3.ViewModels;
using ProyectoU3.Views;

namespace ProyectoU3
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(ListActividadesView), typeof(ListActividadesView));
            BindingContext = new ShellViewModel();
            
        }

        
    }
}
