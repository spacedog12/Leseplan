
namespace Leseplan.Model;

[Table("Catechism")]
public class CatechismPlan
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("catechism")]
    public string? CatechismPassage { get; set; }

    [Column("catechismRead")]
    public bool CatechismRead { get; set; }

    [Column("dateRead")]
    public DateOnly DateRead { get; set; }
}

