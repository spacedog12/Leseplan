namespace Leseplan.ViewModel;

public partial class CatechismViewModel : BaseViewModel
{
    DatabaseRepository dbRepo;
    private bool disposed = false;

    public ObservableCollection<CatechismPlan> CatechismPassages { get; set; } = [];
    public RelayCommand<CatechismPlan> UpdateThisItemCommand { get; }

    public CatechismViewModel(DatabaseRepository dbRepo)
    {
        this.dbRepo = dbRepo;
        UpdateThisItemCommand = new RelayCommand<CatechismPlan>(async (plan) => await SetCatechismPassageReadAsync(plan));
    }

    [ObservableProperty]
    bool isRefreshing;

    public async Task OnAppearing()
    {
        await GetCatechismPassagesAsync();
    }

    [RelayCommand]
    async Task GetCatechismPassagesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            Debug.WriteLine($"Start adding Catechism passages to ObservableCollection");

            IsBusy = true;
            var passages = await dbRepo.GetCatechismPassages();

            if (CatechismPassages.Count != 0)
                CatechismPassages.Clear();

            foreach (var passage in passages)
            {
                CatechismPassages.Add(passage);
            }

            Debug.WriteLine($"Catechism added to ObservableCollection");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception: {ex}");
            await Shell.Current.DisplayAlert("Error!", $"Unable to get Catechism Passages: {ex.Message}", "OK");

        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }

    }

    [RelayCommand]
    async Task SetCatechismPassageReadAsync(CatechismPlan plan)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            await dbRepo.SetCatechismRead(plan.CatechismId);
            Console.WriteLine($"Updated catechism {plan.CatechismRead}");

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                dbRepo?.Dispose();
                CatechismPassages?.Clear();
            }

            disposed = true;
        }
    }

    ~CatechismViewModel()
    {
        Dispose(false);
    }
}

