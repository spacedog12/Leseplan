using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.DebugRainbows;
using DotNet.Meteor.HotReload.Plugin;


namespace Leseplan;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitMarkup()
#if DEBUG
			.EnableHotReload()
#endif
			.UseMauiCommunityToolkit()
			/*
			 * Rainbows debuger, only enable to Debug
			 */
			/* .UseDebugRainbows(new DebugRainbowsOptions{
				ShowRainbows = true,
				ShowGrid = false,
				HorizontalItemSize = 20,
				VerticalItemSize = 20,
				MajorGridLineInterval = 4,
				MajorGridLines = new GridLineOptions { Color = Color.FromRgb(255, 0, 0), Opacity = 1, Width = 4 },
				MinorGridLines = new GridLineOptions { Color = Color.FromRgb(255, 0, 0), Opacity = 1, Width = 1 },
				GridOrigin = DebugGridOrigin.TopLeft,
			}) */

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Light.ttf", "OpenSansLight");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Montserrat-VariableFont-wght.ttf", "Montserrat");
				fonts.AddFont("NanumGothic-Bold.ttf", "Gothic");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<BibleATPage>();
		builder.Services.AddSingleton<BibleNTPage>();
		builder.Services.AddSingleton<BibleViewModel>();

		builder.Services.AddTransient<BibleDetailPage>();
		builder.Services.AddTransient<BibleDetailViewModel>();

		builder.Services.AddSingleton<CatechismPage>();
		builder.Services.AddSingleton<CatechismViewModel>();

		builder.Services.AddTransient<PopupCatechismPage>();
		builder.Services.AddTransient<PopupCatechismViewModel>(); 

		builder.Services.AddSingleton<SettingsPage>();

		builder.Services.AddSingleton<DatabaseRepository>();




		return builder.Build();
	}
}