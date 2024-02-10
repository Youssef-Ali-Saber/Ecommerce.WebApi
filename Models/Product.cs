using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Sku { get; set; } 

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }
    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
