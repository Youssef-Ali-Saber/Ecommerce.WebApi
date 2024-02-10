using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Shipment
{
    public int Id { get; set; }
    [Required]
    public int OrderId { get; set; }

    public DateTime? Date { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public string Country { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
