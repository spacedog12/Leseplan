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
        if (sender is CheckBox checkBox && checkBox.BindingContext is PopupCatechismViewModel viewModel)
        {
            Debug.WriteLine($"Checkbox Changed to: {e.Value}");
            viewModel.HandleCheckboxChange();
        }
    }
}
