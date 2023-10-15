namespace StudiBloc3_Mercadona.Model;

public class CompleteProduct
{
    public Product Product { get; set; }
    public IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
}