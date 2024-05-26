using Microsoft.Maui.Controls;

namespace Leseplan.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    DatabaseRepository dbRepo;
    ReadTagSetterHelper readTagSetterHelper;

    public MainViewModel(DatabaseRepository dbRepo)
    {
        this.dbRepo = dbRepo;
        readTagSetterHelper = new ReadTagSetterHelper();
    }

    // Properties for the CatechismCard
    [ObservableProperty]
    CatechismPlan catechismPassage;
    
    [ObservableProperty]
    string catechismStatusIndicator = "Offen";

    [ObservableProperty]
    Color catechismStatusTagColor;

    // Refreshes the Views
    public async Task OnAppearing()
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
            Console.WriteLine($"Start getting next CatechismPassage");
            IsBusy = true;

            // gets the next passage
            (CatechismPlan nextPassage, CatechismPlan previousPassage) = await dbRepo.GetNextUnreadCatechismPassage();

            // Checks which Data to pick
            if (nextPassage is null)
            {
                Debug.WriteLine($"The data is up to date!");
                return;
            }

            bool catRead = false;

            if (CatechismPassage is null && previousPassage is null)
            {
                CatechismPassage = nextPassage;
                catRead = nextPassage.CatechismRead;
            }
            else if (CatechismPassage is null || (previousPassage is not null && previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate())))
            {
                CatechismPassage = previousPassage;
                catRead = previousPassage.CatechismRead;
            }
            else
            {
                CatechismPassage = nextPassage;
                catRead = nextPassage.CatechismRead;
            }

            Debug.WriteLine("Before updating: " + CatechismStatusIndicator + ", " + CatechismStatusTagColor);
            CatechismStatusIndicator = readTagSetterHelper.GetStatusIndicator(catRead);
            CatechismStatusTagColor = readTagSetterHelper.GetStatusTagColor(catRead);
            Debug.WriteLine("After updating: " + CatechismStatusIndicator + ", " + CatechismStatusTagColor);

            Debug.WriteLine($"Read status of {(CatechismPassage == previousPassage ? "previous" : "next")} Passage: {catRead}");
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

    [RelayCommand]
    async Task GoToCatechismPopupAsync(CatechismPlan catechismPlan)
    {

        if (catechismPlan is null)
            return;

        // Opens the Popup for Catechism
        await Shell.Current.GoToAsync($"{nameof(PopupCatechismPage)}", true,
            new Dictionary<string, object>
            {
                    {"CatechismPlan", catechismPlan}
            });

        // TODO: Need to find out, how to Implement the PushModalAsync so that i can pass data
        // await Shell.Current.Navigation.PushModalAsync(new PopupCatechismPage(new PopupCatechismViewModel()), true);

    }
}

