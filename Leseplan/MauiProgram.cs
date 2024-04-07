using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Leseplan;

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
				fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				// fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Montserrat-VariableFont-wght.ttf", "Montserrat");
                fonts.AddFont("NanumGothic-Bold.ttf", "Gothic");
			});

#if DEBUG
        builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<BiblePage>();

		builder.Services.AddSingleton<CatechismPage>();
		builder.Services.AddSingleton<CatechismViewModel>();


		return builder.Build();
	}
}

