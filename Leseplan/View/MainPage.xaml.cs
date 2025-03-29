namespace Leseplan.View;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		// Refreshes the View when reopened
		//this.Appearing += async (sender, e) => await viewModel.OnAppearing();
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (BindingContext is MainViewModel viewModel)
		{
			await viewModel.OnAppearing();
		}
	}
}


