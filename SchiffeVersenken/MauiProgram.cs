using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SchiffeVersenken.Data;

namespace SchiffeVersenken
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
                    fonts.AddFont("ka1.ttf", "Karmatic");
                    fonts.AddFont("6809-chargen.ttf", "6809chargen");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
			builder.Services.AddSingleton<GameLogicService>();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite("Data Source=schiffeversenken.db"));

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
