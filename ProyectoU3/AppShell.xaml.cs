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
            try
            {
                BindingContext = IPlatformApplication.Current.Services.GetService<ShellViewModel>();
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }


    }
}
