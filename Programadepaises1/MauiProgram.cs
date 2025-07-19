using Microsoft.Extensions.Logging;
using Programadepaises1.Services;
using Programadepaises1.ViewModels;
using Programadepaises1.Views;

namespace Programadepaises1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IPaisService, PaisService>();
        builder.Services.AddSingleton<IDbService, DbService>();

        // ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<FavoritosViewModel>();

        // Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<FavoritosPage>();

        builder.Logging.AddDebug();

        return builder.Build();
    }
}
