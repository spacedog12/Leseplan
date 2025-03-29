namespace Leseplan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		/* 
			sets the routs
		*/

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(BibleATPage), typeof(BibleATPage));
		Routing.RegisterRoute(nameof(BibleNTPage), typeof(BibleNTPage));
		Routing.RegisterRoute(nameof(BibleDetailPage), typeof(BibleDetailPage));
		Routing.RegisterRoute(nameof(CatechismPage),typeof(CatechismPage));
		Routing.RegisterRoute(nameof(PopupCatechismPage), typeof(PopupCatechismPage));
		Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
	}
}

