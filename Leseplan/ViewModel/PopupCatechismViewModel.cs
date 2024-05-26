namespace Leseplan.ViewModel;

[QueryProperty(nameof(CatechismPlan), "CatechismPlan")]
public partial class PopupCatechismViewModel : BaseViewModel
{
	DatabaseRepository dbRepo;
	ReadTagSetterHelper readTagSetterHelper;

	public PopupCatechismViewModel(DatabaseRepository dbRepo)
	{
		this.dbRepo = dbRepo;
		readTagSetterHelper = new ReadTagSetterHelper();
	}

	[ObservableProperty]
	CatechismPlan catechismPlan;

	[ObservableProperty]
	string statusIndicator = "";

	[ObservableProperty]
	Color statusTagColor;

	[RelayCommand]
	async Task UpdateCatechismReadStateAsync()
	{
		if (IsBusy)
			return;

		try
		{
			IsBusy = true;

			// Updates the Database Repository
			await dbRepo.SetCatechismRead(CatechismPlan.CatechismId);
			Console.WriteLine($"Updated catechism {CatechismPlan.CatechismRead}");
			SetStatusIndicatorColorAndText();
		}
		catch (Exception ex)
		{

			Console.WriteLine($"Exception: {ex}");
		}
		finally
		{
			IsBusy = false;
		}

	}

	public void SetStatusIndicatorColorAndText()
	{
		if (CatechismPlan is null)
		{
			Console.WriteLine("CatechismPlan is null");
			return;
		}

		try
		{
			IsBusy = true;

			Console.WriteLine($"Passage Status: {CatechismPlan.CatechismRead}");
			// gets the state of the passage
			bool catRead = CatechismPlan.CatechismRead;

			// sets the Color and Text for the Passag read tag
			Console.WriteLine("Before updating: " + StatusIndicator + ", " + StatusTagColor);
			StatusIndicator = readTagSetterHelper.GetStatusIndicator(catRead);
			StatusTagColor = readTagSetterHelper.GetStatusTagColor(catRead);
			Console.WriteLine("After updating: " + StatusIndicator + ", " + StatusTagColor);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception: {ex}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	public async void HandleCheckboxChange()
	{
		if (CatechismPlan is null)
		{
			Console.WriteLine($"CatechismPlan is null");
			return;
		}

		await UpdateCatechismReadStateAsync();
	}
}

