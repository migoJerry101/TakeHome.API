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

                var rootPackagings = product.Packagings
                    .Where(pg => pg.ParentPackagingId == null)
                    .ToList();

                foreach (var root in rootPackagings)
                {
                    MapPackagingRecursive(root, product.Packagings.ToList(), productDto.PackagingLevels, 1, "");
                }

                results.Add(productDto);
            }

            return results;
        }

        private void MapPackagingRecursive(Packaging current, List<Packaging> all, List<PackagingLevelDto> results, int level, string parentPath)
        {
            var typeName = current.PackagingType?.TypeName ?? "Unknown";
            var currentPath = string.IsNullOrEmpty(parentPath) ? typeName : $"{parentPath} -> {typeName}";

            results.Add(new PackagingLevelDto
            {
                PackageLevel = level,
                PackagingPath = currentPath,
                TypeName = typeName,
                Items = current.PackagingItems.Select(pi => new PackagingItemDto
                {
                    ItemName = pi.Item?.ItemName ?? "Unknown",
                    Quantity = pi.Quantity
                }).ToList()
            });

            var children = all.Where(pg => pg.ParentPackagingId == current.PackagingId);
            foreach (var child in children)
            {
                MapPackagingRecursive(child, all, results, level + 1, currentPath);
            }
        }
    }
}
