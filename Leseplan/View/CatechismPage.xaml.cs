namespace Leseplan.View;

public partial class CatechismPage : ContentPage
{
	public CatechismPage(CatechismViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is CatechismViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }

    // Checks if the Checkbox was triggert or changed
    void CheckBox_CheckedChanged(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is CatechismPlan catechismPlan)
        {
            (BindingContext as CatechismViewModel)?.UpdateThisItemCommand.Execute(catechismPlan);
        }
    }
}
