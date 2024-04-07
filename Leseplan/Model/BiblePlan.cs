
namespace Leseplan.Model;


[Table("Bible")]
public class BiblePlan
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("day")]
    public int Day { get; set; }

    [Column("books")]
    public string? BibleBooks { get; set; }

    [Column("section")]
    public string? BibleSection { get; set; }

    [Column("bibleRead")]
    public bool BibleRead { get; set; }

    [Column("dateRead")]
    public DateOnly? DateRead { get; set; }
}


