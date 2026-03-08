using System;
using System.Collections.Generic;

namespace TakeHome.API.Models;

public partial class Packaging
{
    public int PackagingId { get; set; }

    public int ProductId { get; set; }

    public int PackagingTypeId { get; set; }

    public int? ParentPackagingId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Packaging> InverseParentPackaging { get; set; } = new List<Packaging>();

    public virtual ICollection<PackagingItem> PackagingItems { get; set; } = new List<PackagingItem>();

    public virtual PackagingType PackagingType { get; set; } = null!;

    public virtual Packaging? ParentPackaging { get; set; }

    public virtual Product Product { get; set; } = null!;
}
