using CommunityToolkit.Maui.Alerts;
using ProyectoU3.Services;
using ProyectoU3.Views;

namespace ProyectoU3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        private readonly LoginClient loginClient;
        private readonly DepartamentosService adminService;

        public App(ActividadesService actividadesService, LoginView loginView, LoginClient loginClient, DepartamentosService adminService)
        {
            InitializeComponent();

            ActividadesService = actividadesService;
            this.loginClient = loginClient;
            this.adminService = adminService;
            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();

            //Thread admincheck = new Thread(CheckAdmin) { IsBackground = true };
            //admincheck.Start();

            LoginView = loginView;
            var tk = SecureStorage.GetAsync("tkn").Result;
            if (tk != null)
            {
                //var state = loginClient.Validar(tk);
                //if (state)
                //{
                //    MainPage = new AppShell();
                //}
                //else
                //{
                //    MainPage = LoginView;
                //}
                MainPage = new AppShell();
            }
            else
            {
                MainPage = LoginView;
            }

            //MainPage = new AppShell();
            //CheckTkn();
        }
        public static bool IsAdmin { get; set; }

        public static ActividadesService ActividadesService { get; set; }
        public static LoginView LoginView { get; set; }

        public static async Task CargarDatos()
        {
            await ActividadesService.GetAllAsync();
        }
        async void CheckAdmin()
        {
            await Task.Run(async () =>
            {
                var tkn = SecureStorage.GetAsync("tkn").Result;
                while (tkn == null)
                {
                    tkn = SecureStorage.GetAsync("tkn").Result;
                    Thread.Sleep(500);
                }
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsAdmin = adminService.AdminCheckAsync(tkn).Result;
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Snackbar.Make("No hay internet, no se pudo comprobar rol de administrador").Show();
                    });
                }
            });
        }

        async void Sincronizador()
        {
            while (true)
            {
                await ActividadesService.GetAllAsync();
                Thread.Sleep(5000);
            }
        }
        async void GoListas()
        {
            await Shell.Current.GoToAsync("//ListaActividadesView");
            Shell.Current.ToolbarItems.First().IsEnabled = true;
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
