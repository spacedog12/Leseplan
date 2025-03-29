namespace Leseplan.ViewModel;

public partial class BibleViewModel : BaseViewModel, IDisposable
{
	DatabaseRepository dbRepo;
	private bool disposed = false;

	public ObservableCollection<BiblePlan> TestamentBooks { get; private set; }

	// Constructor
	public BibleViewModel(DatabaseRepository dbRepo)
	{
		this.dbRepo = dbRepo;
		TestamentBooks = [];
	}

	public async Task OnAppearingPage(string pageTestament)
	{
		await GetTestamentDataAsync(pageTestament);
	}

	[RelayCommand]
	public async Task GetTestamentDataAsync(string selectedTestament)
	{
		if (IsBusy)
			return;

		try
		{
			Debug.WriteLine($"Start of GetTestamentDataAsyn in the BibleViewModel");
			
			IsBusy = true;

			var books = await dbRepo.GetBibleBooksByTestament(selectedTestament);
			TestamentBooks.Clear();
			foreach (var book in books)
			{
				TestamentBooks.Add(book);
			}

			Debug.WriteLine($"End of GetTestamentDataAsyn in the BibleViewModel");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Exception thrown in GetBibleBooksByTestament in BibleViewModel: {ex.Message}");
			Debug.WriteLine($"Exception thrown in GetBibleBooksByTestament in BibleViewModel: {ex.StackTrace}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	async Task GoToBibleDetailPageAsync(BiblePlan biblePlan)
    {
        if (biblePlan is null)
            return;

		Debug.WriteLine($"Navigating to details page with BibleBook: {biblePlan.BibleBooks}");
		// Opens the BibleDetailPage
		await Shell.Current.GoToAsync($"{nameof(BibleDetailPage)}", true,
			new Dictionary<string, object>
			{
				{"BiblePlan", biblePlan}
			});
        
    }

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				dbRepo?.Dispose();
				TestamentBooks?.Clear();
			}

			disposed = true;
		}
	}

	~BibleViewModel()
	{
		Dispose(false);
	}
}

