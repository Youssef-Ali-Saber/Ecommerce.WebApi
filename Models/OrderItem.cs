using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; } 

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
    [JsonIgnore]
    public virtual Order Order { get; set; } 

    public virtual Product Product { get; set; } 
}
