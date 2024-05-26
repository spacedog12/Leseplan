namespace Leseplan.ViewModel;

[QueryProperty(nameof(BiblePlan), "BiblePlan")]
public partial class BibleDetailViewModel : BaseViewModel, IDisposable
{
	DatabaseRepository dbRepo;
	private bool disposed = false;

	public ObservableCollection<BiblePlan> BookPassages { get; set; }
	public RelayCommand<BiblePlan> UpdateThisItemCommand { get; }


	public BibleDetailViewModel(DatabaseRepository dbRepo)
	{
		this.dbRepo = dbRepo;
		BookPassages = [];
		UpdateThisItemCommand = new RelayCommand<BiblePlan>(async (plan) => await SetBiblePassageReadAsync(plan));
	}

	[ObservableProperty]
	BiblePlan biblePlan;

	public async Task OnAppearingPage()
	{
		await GetPassagesDataForBibleBookAsync();
	}

	[RelayCommand]
	async Task GetPassagesDataForBibleBookAsync()
	{
		if (IsBusy)
			return;

		try
		{
			Debug.WriteLine($"Start of GettingPassagesDataForBibleBookAsync in BibleDetailBiewModel");


			IsBusy = true;

			var passages = await dbRepo.GetBiblePassagesByBook(BiblePlan.BibleBooks);
			BookPassages.Clear();
			foreach (var passage in passages)
			{
				BookPassages.Add(passage);
			}

			Debug.WriteLine($"End of GettingPassagesDataForBibleBookAsync in BibleDetailBiewModel");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Exception thrown in GetPassagesDataForBibleBookAsync in BibleDetailViewModel: {ex.Message}");
			Debug.WriteLine($"Exception thrown in GetPassagesDataForBibleBookAsync in BibleDetailViewModel: {ex.StackTrace}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	async Task SetBiblePassageReadAsync(BiblePlan plan)
	{
		if (IsBusy)
			return;

		try
		{
			IsBusy = true;

			await dbRepo.SetBiblePassageRead(plan.BibleId);
			Console.WriteLine($"Updated bible {plan.BibleRead}");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Exception in SetBiblePassageReadAsync in BibleDetailViewModel: {ex.Message}");
			Debug.WriteLine($"Exception in SetBiblePassageReadAsync in BibleDetailViewModel: {ex.StackTrace}");
		}
		finally
		{
			IsBusy = false;
		}
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
				BookPassages?.Clear();
			}

			disposed = true;
		}
	}

	~BibleDetailViewModel()
	{
		Dispose(false);
	}
}