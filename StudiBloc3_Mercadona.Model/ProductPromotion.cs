using System.ComponentModel.DataAnnotations.Schema;

namespace StudiBloc3_Mercadona.Model;

[Table("ProductPromotion", Schema = "data")]
public class ProductPromotion
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int PromotionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}