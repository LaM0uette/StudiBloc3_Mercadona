using System.ComponentModel.DataAnnotations.Schema;

namespace StudiBloc3_Mercadona.Model;

[Table("Promotion", Schema = "data")]
public class Promotion
{
    public int Id { get; set; }
    public int DiscountPercentage { get; set; }
}
