namespace Leseplan.View;

public partial class BibleATPage : ContentPage
{
	protected string tabTitle = "Altes Testament";

	public BibleATPage(BibleViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		Debug.WriteLine($"Testament: {tabTitle}");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is BibleViewModel viewModel)
		{
			await viewModel.OnAppearingPage(tabTitle);
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		
		if (BindingContext is BibleViewModel viewModel)
		{
			viewModel.Dispose();
		}
	}
}
