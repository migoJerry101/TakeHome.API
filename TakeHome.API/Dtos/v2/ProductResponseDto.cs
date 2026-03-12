namespace TakeHome.API.Dtos.v2
{
    public class ProductResponseDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public List<PackagingLevelDto> PackagingLevels { get; set; } = new();
    }

    public class PackagingLevelDto
    {
        public int PackageLevel { get; set; }
        public string PackagingPath { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public PackagingItemDto Item { get; set; }

        public List<PackagingLevelDto> Packaging { get; set; } = new();
    }

    public class PackagingItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
