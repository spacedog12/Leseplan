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

	[ObservableProperty]
	string statusIndicator = "Offen";

	[ObservableProperty]
	Color statusTagColor;

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
            if(nextPassage is null)
            {
                Debug.WriteLine($"The data is up to date!");
                return;
            }

            bool catRead = false;

            if (Passage is null && previousPassage is null)
            {
                Passage = nextPassage;
                catRead = nextPassage.CatechismRead;
            }
            else if (Passage is null || (previousPassage is not null && previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate())))
            {
                Passage = previousPassage;
                catRead = previousPassage.CatechismRead;
            }
            else
            {
                Passage = nextPassage;
                catRead = nextPassage.CatechismRead;
            }

            SetReadTag(catRead);
            Debug.WriteLine($"Read status of {(Passage == previousPassage ? "previous" : "next")} Passage: {catRead}");

            /*

            // checks if it is the first passage, or if the passage is empty
            if (Passage is null && previousPassage is null && nextPassage?.CatechismDateRead is null)
            {
                Passage = nextPassage;
                catRead = nextPassage.CatechismRead;
                SetReadTag(catRead);
                Debug.WriteLine($"Read status of next Passage: {catRead}");
            }
            else if (Passage is null && previousPassage is not null && previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate()))
            {
                Passage = previousPassage;
                catRead = previousPassage.CatechismRead;
                SetReadTag(catRead);
                Debug.WriteLine($"Read status of previous Passage: {catRead}");
            }
            else if (Passage is not null && previousPassage is not null && previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate()))
            {
                Passage = previousPassage;
                catRead = previousPassage.CatechismRead;
                SetReadTag(catRead);
                Debug.WriteLine($"Read status of previous Passage: {catRead}");
            }
            else if (Passage is not null && previousPassage is not null && !previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate()))
            {
                Passage = nextPassage;
                catRead = nextPassage.CatechismRead;
                SetReadTag(catRead);
                Debug.WriteLine($"Read status of next Passage: {catRead}");
            }
            else if (Passage is null && previousPassage is not null && !previousPassage.CatechismDateRead.Equals(DatePickerHelper.GetTodaysDate()))
            {
                Passage = nextPassage;
                catRead = nextPassage.CatechismRead;
                SetReadTag(catRead);
                Debug.WriteLine($"Read status of next Passage: {catRead}");
            }
            else
            {
                Debug.WriteLine($"The data is up to date!");
                return;
            }
            */
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
    async Task GoToCatechismPopupAsync()
    {

    }

	// Sets the Read Tag
	private void SetReadTag(bool catRead)
	{
        // Sets the State for the View
        if (catRead)
        {
            StatusIndicator = "Gelesen";

            // Gets the Color out of the Colors.xaml file
            if (App.Current.Resources.TryGetValue("ReadTag", out var colorvalue))
                StatusTagColor = (Color)colorvalue;

            Debug.WriteLine($"Open status of View: {StatusIndicator}");
            Debug.WriteLine($"Color for StatusTagColor: {StatusTagColor}");
        }
        else
        {
            StatusIndicator = "Offen";

            // Gets the Color out of the Colors.xaml file
            if (App.Current.Resources.TryGetValue("OpenTag", out var colorvalue))
                StatusTagColor = (Color)colorvalue;

            Debug.WriteLine($"Read status of View: {StatusIndicator}");
            Debug.WriteLine($"Color for StatusTagColor: {StatusTagColor}");
        }

        Debug.WriteLine($"Next Catechism Passage: {Passage.CatechismPassage}");
    }
}

