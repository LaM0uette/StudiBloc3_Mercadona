namespace StudiBloc3_Mercadona.Model;

public class FullProduct
{
    public Product Product { get; set; }
    public Promotion Promotion { get; set; }
    public ProductPromotion ProductPromotion { get; set; }
    public Category Category { get; set; }
}