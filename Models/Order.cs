using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Order
{
    public int Id { get; set; }

    public string userId { get; set; } = null!;

    public DateTime Date { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } 

    public virtual ICollection<OrderItem> OrderItems { get; set; }
    [JsonIgnore]
    public virtual ICollection<Payment> Payments { get; set; }
    [JsonIgnore]
    public virtual ICollection<Shipment> Shipments { get; set; }
    [JsonIgnore]
    public virtual User user { get; set; }
}
