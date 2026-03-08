using System;
using System.Collections.Generic;

namespace TakeHome.API.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Sku { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Packaging> Packagings { get; set; } = new List<Packaging>();
}
