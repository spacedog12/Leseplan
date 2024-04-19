namespace Leseplan.Services;

public class DatePickerHelper
{
	public DatePickerHelper()
	{
	}

	public static string GetTodaysDate()
	{
        // Get's today's date
        var dateOnly = DateOnly.FromDateTime(DateTime.Today);
        var dateString = dateOnly.ToString("yyyy-MM-dd");
        Console.WriteLine($"Todays Date: {dateString}");

        return dateString;
    }
}

