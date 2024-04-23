namespace Leseplan.View;

public partial class PopupCatechismPage : ContentPage
{

    CheckBox popupCatechismCheckbox;

    public PopupCatechismPage(PopupCatechismViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        popupCatechismCheckbox = this.FindByName<CheckBox>("PopupCatechismCheckBox");
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (BindingContext is PopupCatechismViewModel viewModel)
        {
            viewModel.SetStatusIndicatorColorAndText();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Subscribe to the CheckedChanged event after the view appears
        popupCatechismCheckbox.CheckedChanged += CheckBox_CheckedChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Unsubscribe from the CheckedChanged event to clean up
        popupCatechismCheckbox.CheckedChanged -= CheckBox_CheckedChanged;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is PopupCatechismViewModel viewModel)
        {
            Debug.WriteLine($"Checkbox Changed to: {e.Value}");
            viewModel.HandleCheckboxChange();
        }
    }

    /*
    void CheckBox_CheckedChanged_PopupCatechism(System.Object sender, Microsoft.Maui.Controls.CheckedChangedEventArgs e)
    {
        /*
        if (sender is CheckBox checkBox && checkBox.BindingContext is PopupCatechismViewModel viewModel)
        {
            viewModel.HandleCheckboxChange();
            // (BindingContext as PopupCatechismViewModel)?.UpdateThisItemCommand.Execute(catechismPlan);
        }
        */

        /*
        if (sender is CheckBox checkBox)
        {
            if (checkBox.BindingContext is PopupCatechismViewModel viewModel)
            {
                Debug.WriteLine($"Checkbox changed: {viewModel}");
                viewModel.HandleCheckboxChange();
            }
            else
            {
                Debug.WriteLine("Checkbox changed, but binding context is not a viewModel.");
            }
        }*/
        /*
        if (sender is CheckBox checkBox)
        {
            Debug.WriteLine($"Checkbox changed. BindingContext type: {checkBox.BindingContext.GetType()}");

            if (checkBox.BindingContext is PopupCatechismViewModel viewModel)
            {
                Debug.WriteLine($"Checkbox changed: {viewModel.CatechismPlan.CatechismRead}");
                viewModel.HandleCheckboxChange();
            }
            else
            {
                Debug.WriteLine("Checkbox changed, but binding context is not a PopupCatechismViewModel.");
            }
        }

    }
    */

}
