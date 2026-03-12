using Microsoft.EntityFrameworkCore;
using TakeHome.API.Dtos.v2;
using TakeHome.API.Interface.v2;
using TakeHome.API.Models;

namespace TakeHome.API.Services.v2
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _appDbContext.Products
                .Include(p => p.Packagings)
                    .ThenInclude(pg => pg.PackagingType)
                .Include(p => p.Packagings)
                    .ThenInclude(pg => pg.PackagingItems)
                        .ThenInclude(pi => pi.Item)
                .ToListAsync();

            var results = new List<ProductResponseDto>();

            foreach (var product in products)
            {
                var productDto = new ProductResponseDto
                {
                    ProductName = product.ProductName,
                    SKU = product.Sku,
                    PackagingLevels = new List<PackagingLevelDto>()
                };

                var allPackagings = product.Packagings.ToList();

                var dictionary = allPackagings
                    .Where(p => p.ParentPackagingId != null)
                    .GroupBy(p => p.ParentPackagingId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var rootPackagings = allPackagings
                    .Where(pg => pg.ParentPackagingId == null)
                    .ToList();

                foreach (var root in rootPackagings)
                {
                    productDto.PackagingLevels.Add(
                        MapPackagingRecursive(root, dictionary, 1, "")
                    );
                }

                results.Add(productDto);
            }

            return results;
        }

        private PackagingLevelDto MapPackagingRecursive(Packaging current,Dictionary<int?, List<Packaging>> dictionary,int level, string parentPath)
        {
            var typeName = current.PackagingType?.TypeName ?? "Unknown";
            var currentPath = string.IsNullOrEmpty(parentPath)
                ? typeName
                : $"{parentPath} -> {typeName}";

            var dto = new PackagingLevelDto
            {
                PackageLevel = level,
                PackagingPath = currentPath,
                TypeName = typeName,
                Item = new PackagingItemDto(),
                Packaging = new List<PackagingLevelDto>()
            };

            var packagingItem = current.PackagingItems.FirstOrDefault();

            if (packagingItem?.Item != null)
            {
                dto.Item.ItemName = packagingItem.Item.ItemName;
                dto.Item.Quantity = packagingItem.Quantity;
            }

            if (dictionary.TryGetValue(current.PackagingId, out var children))
            {
                foreach (var child in children)
                {
                    dto.Packaging.Add(
                        MapPackagingRecursive(child, dictionary, level + 1, currentPath)
                    );
                }
            }

            return dto;
        }
    }
}
