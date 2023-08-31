using DevExpress.Blazor;
using LoginForMauiBlazorHybrid.Data;
using LoginForMauiBlazorHybrid.Services;
using Microsoft.Extensions.Logging;

namespace LoginForMauiBlazorHybrid
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IAppService, AppService>();
            builder.Services.AddSingleton<WeatherForecastService>();
			builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);
			return builder.Build();
        }
    }
}