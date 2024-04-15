namespace Leseplan.View;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		// Refreshes the View when reopened
		this.Appearing += async (sender, e) => await viewModel.RefreshMainViewData();
	}

	
}


