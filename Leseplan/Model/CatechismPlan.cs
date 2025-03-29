
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Leseplan.Model;

[Table("Catechism")]
public class CatechismPlan
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int CatechismId { get; set; }

    [Column("catechism")]
    public string? CatechismPassage { get; set; }

    [Column("catechismRead")]
    public bool CatechismRead { get; set; }

    [Column("dateRead")]
    public string? CatechismDateRead { get; set; }

    // Is called when the checkbox in the Catechism CollectionView is changed
    /*
    private Command? updateThisItemCommand;
    public ICommand UpdateThisItemCommand => updateThisItemCommand ??= new Command(UpdateThisItem);

    private void UpdateThisItem()
    {

    }
    */
}

