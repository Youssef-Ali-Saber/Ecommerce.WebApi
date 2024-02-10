using System;
using System.Collections.Generic;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public DateTime Date { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public decimal Amount { get; set; }

    public virtual Order? Order { get; set; }
}
