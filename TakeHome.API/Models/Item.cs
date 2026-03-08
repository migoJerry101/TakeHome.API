using System;
using System.Collections.Generic;

namespace TakeHome.API.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<PackagingItem> PackagingItems { get; set; } = new List<PackagingItem>();
}
