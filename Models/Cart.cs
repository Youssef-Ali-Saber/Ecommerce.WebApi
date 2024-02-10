using System;
using System.Collections.Generic;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Cart
{
    public int Id { get; set; }

    public string userId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
