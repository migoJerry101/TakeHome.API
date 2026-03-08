using System;
using System.Collections.Generic;

namespace TakeHome.API.Models;

public partial class PackagingItem
{
    public int PackagingItemId { get; set; }

    public int PackagingId { get; set; }

    public int ItemId { get; set; }

    public decimal Quantity { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Packaging Packaging { get; set; } = null!;
}
