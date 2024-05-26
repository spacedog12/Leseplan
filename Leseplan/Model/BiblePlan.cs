
namespace Leseplan.Model;


[Table("Bible")]
public class BiblePlan
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int BibleId { get; set; }

    [Column("day")]
    public int Day { get; set; }

    [Column("testament")]
    public string? Testament { get; set; }

    [Column("bookSerie")]
    public string? BookSerie { get; set; }

    [Column("books")]
    public string? BibleBooks { get; set; }

    [Column("passage")]
    public string? BiblePassage { get; set; }

    [Column("chronology")]
    public string? Chronology { get; set; }

    [Column("bibleRead")]
    public bool BibleRead { get; set; }

    [Column("dateRead")]
    public string? BibleDateRead { get; set; }


}


