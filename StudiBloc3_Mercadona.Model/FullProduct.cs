namespace StudiBloc3_Mercadona.Model;

public class FullProduct
{
    public Product Product { get; set; }
    public Promotion Promotion { get; set; }
    public ProductPromotion ProductPromotion { get; set; }
    public Category Category { get; set; }
    
    public string DisplayPriceWithPromotion()
    {
        if (ProductPromotion is null) 
            return Product.Price.ToString("C");

        var priceWithPromotion = Product.Price - (Product.Price * ((float)Promotion.DiscountPercentage / 100));
        Console.WriteLine($"{priceWithPromotion} -- {Product.Price} -- {Promotion.DiscountPercentage}");
        return priceWithPromotion.ToString("C");
    }
}