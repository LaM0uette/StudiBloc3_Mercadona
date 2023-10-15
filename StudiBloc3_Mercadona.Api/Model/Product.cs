﻿using System.ComponentModel.DataAnnotations.Schema;

namespace StudiBloc3_Mercadona.Api.Model;

[Table("Product", Schema = "data")]
public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? Image { get; set; }
    
    //[ForeignKey("CategoryId")]
    //public Category Category { get; set; }
}