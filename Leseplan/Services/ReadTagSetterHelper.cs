using System;
namespace Leseplan.Services;
public class ReadTagSetterHelper
{
	string statusIndicator = "Offen";
	Color statusTagColor;

	public ReadTagSetterHelper()
	{
	}

	string StatusIndicator { get; set; }
	Color StatusTagColor { get; set;}

	public string GetStatusIndicator(bool catRead)
	{
		switch (catRead)
		{
			case true:
				StatusIndicator = "Gelesen";
				Debug.WriteLine($"Open status of View: {StatusIndicator}");
				break;
			default:
				StatusIndicator	= "Offen";
				Debug.WriteLine($"Open status of View: {StatusIndicator}");
				break;
		}
		
		return StatusIndicator;
	}

	public Color GetStatusTagColor(bool catRead)
	{ 
		if (catRead)
		{
			// Gets the Color for the read tag out of the Colors.xaml file
			if (App.Current.Resources.TryGetValue("ReadTag", out var colorvalue))
                StatusTagColor = (Color)colorvalue;

			Debug.WriteLine($"Color for StatusTagColor: {StatusTagColor}");
		}
		else
		{
			// Gets the Color for the open tag out of the Colors.xaml file
            if (App.Current.Resources.TryGetValue("OpenTag", out var colorvalue))
                StatusTagColor = (Color)colorvalue;
			
			Debug.WriteLine($"Color for StatusTagColor: {StatusTagColor}");
		}

		return StatusTagColor;
	}

}

