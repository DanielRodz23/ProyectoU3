using ProyectoU3.Services;
using ProyectoU3.ViewModels;
using ProyectoU3.Views;

namespace ProyectoU3
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            var nombre = SecureStorage.GetAsync("name").Result;
            if (nombre == null)
            {
                var deprepo = IPlatformApplication.Current?.Services.GetService<DepartamentosService>();
                nombre = deprepo?.GetDepartamento();
            }
            this.ToolbarItems.Insert(0, new ToolbarItem { IsEnabled = false, Text = (nombre?.Length<=16?nombre:(nombre.Substring(0,16)+"...")).ToUpper() });
            //Routing.RegisterRoute(nameof(ListActividadesView), typeof(ListActividadesView));
            try
            {
                BindingContext = IPlatformApplication.Current?.Services.GetService<ShellViewModel>();
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }


    }
}
