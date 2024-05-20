using ProyectoU3.Services;

namespace ProyectoU3
{
    public partial class App : Application
    {

        public App(ActividadesService actividadesService)
        {
            InitializeComponent();

            ActividadesService = actividadesService;

            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();


            MainPage = new AppShell();
            CheckTkn();
        }

        public static ActividadesService ActividadesService { get; set; }

        async void Sincronizador()
        {
            while (true)
            {
                await ActividadesService.GetAllAsync();
                Thread.Sleep(1000);
            }
        }
        async void CheckTkn()
        {
            var tkn = await SecureStorage.GetAsync("tkn");
            if (tkn == null) return;
            await Shell.Current.GoToAsync("//ListaActividadesView");
            Shell.Current.ToolbarItems.First().IsEnabled = true;
            //var Valido = await loginClient.Validar(tkn);
            //if (Valido)
            //{
            //    await Shell.Current.GoToAsync("//ListaActividadesView");
            //    Shell.Current.ToolbarItems.First().IsEnabled = true;
            //}
            //else
            //{
            //    SecureStorage.RemoveAll();
            //}
        }
    }
}
