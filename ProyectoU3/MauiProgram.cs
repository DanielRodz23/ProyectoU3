using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ProyectoU3.Repositories;
using ProyectoU3.Services;
using ProyectoU3.ViewModels;
using ProyectoU3.Views;

namespace ProyectoU3
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //Servicios
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://doubledapi.labsystec.net/") };
            builder.Services.AddSingleton(client);
            builder.Services.AddTransient<LoginClient>();

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<ActividadesService>();
            builder.Services.AddTransient<ListActividadesViewModel>();
            builder.Services.AddTransient<ListActividadesView>();

            builder.Services.AddSingleton<DetallesViewModel>();
            builder.Services.AddTransient<VerDetallesActividadView>();

            builder.Services.AddSingleton<ActividadesRepository>();
            builder.Services.AddTransient<AgregarActividadViewModel>();
            builder.Services.AddTransient<AgregarActividadView>();
            builder.Services.AddTransient<VerBorradoresModel>();
            builder.Services.AddTransient<VerBorradorView>();

            builder.Services.AddAutoMapper(typeof(MauiProgram));
#if DEBUG
            builder.Logging.AddDebug();
#endif

#if __ANDROID__
            ImageHandler.Mapper.PrependToMapping(nameof(Microsoft.Maui.IImage.Source), (handler, view) => PrependToMappingImageSource(handler, view));
#endif

            return builder.Build();
        }

#if __ANDROID__
        public static void PrependToMappingImageSource(IImageHandler handler, Microsoft.Maui.IImage image)
        {
            handler.PlatformView?.Clear();
        }
#endif


    }
}
