namespace Leseplan;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// sets the rout to the Popup of Catechism
		Routing.RegisterRoute(nameof(PopupCatechismPage), typeof(PopupCatechismPage));
	}
}

