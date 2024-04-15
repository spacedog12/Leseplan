namespace Leseplan.ViewModel;

public partial class MainViewModel : BaseViewModel
{
	DatabaseRepository dbRepo;

	public MainViewModel(DatabaseRepository dbRepo)
	{
		this.dbRepo = dbRepo;
	}

	[ObservableProperty]
	CatechismPlan passage;

	// Refreshes the Views
	public async Task RefreshMainViewData()
	{
		await GetNextUnreadCatechismPassageAsync();
	}

	[RelayCommand]
	async Task GetNextUnreadCatechismPassageAsync()
	{
		if (IsBusy)
			return;

		try
		{
            Debug.WriteLine($"Start getting next CatechismPassage");
            IsBusy = true;


			Passage = await dbRepo.GetNextUnreadCatechismPassage();
			Console.WriteLine($"Next Catechism Passage: {Passage}");

		}
		catch (Exception ex)
		{
            Debug.WriteLine($"Exception: {ex}");
        }
		finally
		{
			IsBusy = false;
		}
	}
}

