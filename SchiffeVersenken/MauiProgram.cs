using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using SchiffeVersenken.Data;
using SchiffeVersenken.DatabaseEF.Database;

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
				});

			builder.Services.AddMauiBlazorWebView();
			builder.Services.AddMudServices();
			builder.Services.AddSingleton<GameLogicService>();
			builder.Services.AddDbContext<DatabaseContext>();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif
			var app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				if (dbContext.Database.EnsureCreated())
				{
					DatabaseAccess.CreateDefaultUsers().Wait();
				}
			}

			return app;
		}
	}
}
