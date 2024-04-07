namespace Leseplan.View;

public partial class CatechismPage : ContentPage
{
	public CatechismPage(CatechismViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}
