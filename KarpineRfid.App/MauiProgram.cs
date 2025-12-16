using Microsoft.Extensions.Logging;
using KarpineRfid.App.Services;
using KarpineRfid.App.ViewModels;
using KarpineRfid.App.Views;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Maui;
using System;

namespace KarpineRfid.App
{
    public static class ServiceContainer
    {
        public static IServiceProvider? Services { get; set; }
    }

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif


            
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
            
            ;

            builder.Services.AddSingleton<ISessionStore, SessionService>();

            builder.Services.AddSingleton<IExportService, ExportService>();

            builder.Services.AddSingleton<ISessionStore, InMemorySessionStore>();

            builder.Services.AddTransient<SessionListViewModel>();
            builder.Services.AddTransient<SessionDetailViewModel>();
            builder.Services.AddTransient<ExportViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<AboutViewModel>();


            builder.Services.AddTransient<Homepage>();
            builder.Services.AddTransient<SessionListPage>();
            builder.Services.AddTransient<SessionDetailPage>();
            builder.Services.AddTransient<ExportPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<AboutModalPage>();
            builder.Services.AddSingleton<AppShell>();



            var app = builder.Build();

            ServiceContainer.Services = app.Services;

            return app;
        }
    }
}

