using System;
using System.Collections.Generic;

namespace TakeHome.API.Models;

public partial class PackagingType
{
    public int PackagingTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Packaging> Packagings { get; set; } = new List<Packaging>();
}
