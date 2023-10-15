using System.ComponentModel.DataAnnotations.Schema;

namespace StudiBloc3_Mercadona.Api.Model;

[Table("Category", Schema = "data")]
public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
}