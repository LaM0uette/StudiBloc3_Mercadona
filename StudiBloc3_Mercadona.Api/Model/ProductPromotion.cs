using System.ComponentModel.DataAnnotations.Schema;

namespace StudiBloc3_Mercadona.Api.Model;

[Table("ProductPromotion", Schema = "data")]
public class ProductPromotion
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int PromotionId { get; set; }
}