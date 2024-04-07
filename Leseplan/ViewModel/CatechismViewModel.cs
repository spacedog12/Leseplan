namespace Leseplan.ViewModel;

public partial class CatechismViewModel : BaseViewModel
{
    DatabaseRepository dbRepo;

    public ObservableCollection<CatechismPlan> CatechismPassages { get; } = new();

    public CatechismViewModel(DatabaseRepository dbRepo)
    {
        this.dbRepo = dbRepo;
    }

    [ObservableProperty]
    bool isRefreshing;

    [RelayCommand]
    async Task GetCatechismPassagesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            await Task.Delay(2000);

            var passages = await dbRepo.GetCatechismPassages();

            if (CatechismPassages.Count != 0)
                CatechismPassages.Clear();

            foreach (var passage in passages)
            {
                CatechismPassages.Add(passage);
            }
;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get Catechism Passages: {ex.Message}", "OK");

        }
        finally
        {
            IsBusy = false;
            isRefreshing = false;
        }

    }
}

