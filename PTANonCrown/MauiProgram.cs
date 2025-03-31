using CommunityToolkit.Maui;
using PTANonCrown.Services;
using PTANonCrown.ViewModel;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace PTANonCrown
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

                builder.Services.AddSingleton<MainService>();

                builder.Services.AddSingleton<StandPage>();
                builder.Services.AddSingleton<PlotPage>();
                builder.Services.AddSingleton<LiveTreePage>();
                builder.Services.AddSingleton<DeadTreePage>();
                builder.Services.AddSingleton<CoarseWoodyMaterialPage>();
                builder.Services.AddSingleton<SummaryPage>();

                builder.Services.AddSingleton<MainViewModel>();

            builder.Logging.AddDebug();

                return builder.Build();

        }
    }
}
