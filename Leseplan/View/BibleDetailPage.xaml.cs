namespace Leseplan.View;

public partial class BibleDetailPage : ContentPage
{

	private bool isUserInteraction = false;

	public BibleDetailPage(BibleDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (BindingContext is BibleDetailViewModel viewModel)
		{
			await viewModel.OnAppearingPage();
		}
	}

	/* protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (BindingContext is BibleDetailViewModel viewModel)
		{
			await viewModel.OnAppearingPage();

			foreach (var item in viewModel.BookPassages)
			{
				var checkBox = FindCheckBoxForItem(item);
				if (checkBox != null)
				{
					checkBox.CheckedChanged += CheckBox_CheckedChanged;
					checkBox.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(() => CheckBox_Tapped(checkBox, null))
					});
				}
			}
		}
	} */

	void CheckBox_CheckedChanged(System.Object sender, CheckedChangedEventArgs e)
	{ 
		Debug.WriteLine($"Start CheckBox_CheckedChanged in BibleDetailPage");
		if (!isUserInteraction)
			return;

		if (sender is CheckBox checkBox && checkBox.BindingContext is BiblePlan biblePlan)
		{
			Debug.WriteLine($"Changed CheckBox_State in BibleDetailPage");
			(BindingContext as BibleDetailViewModel)?.UpdateThisItemCommand.Execute(biblePlan);
		}

		isUserInteraction = false;
		Debug.WriteLine($"End CheckBox_CheckedChanged in BibleDetailPage");
	}

	void CheckBox_Tapped(System.Object sender, TappedEventArgs e)
	{
		Debug.WriteLine($"CheckBox_Tapped fired");
		isUserInteraction = true;
		
		Debug.WriteLine($"State of isUserInteraction: {isUserInteraction}");
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

		if (BindingContext is BibleDetailViewModel viewModel)
		{
			viewModel.Dispose();
		}
    }

	/* protected override void OnDisappearing()
    {
        base.OnDisappearing();

		if (BindingContext is BibleDetailViewModel viewModel)
		{
			foreach (var item in viewModel.BookPassages)
			{
				var checkBox = FindCheckBoxForItem(item);
				if(checkBox != null)
				{
					checkBox.CheckedChanged -= CheckBox_CheckedChanged;
				}
			}
			viewModel.Dispose();
		}
    } */

	private CheckBox FindCheckBoxForItem(BiblePlan item)
	{ 
		return (CheckBox)this.FindByName("CheckBox" + item.BibleId);
	}

}