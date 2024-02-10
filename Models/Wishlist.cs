using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstWebApiProject_E_Commerce_.Models;

public partial class Wishlist
{
    public int Id { get; set; }

    public string userId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User user { get; set; } = null!;

}
