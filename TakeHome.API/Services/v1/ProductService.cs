using Microsoft.EntityFrameworkCore;
using TakeHome.API.Dtos.v1;
using TakeHome.API.Interface.v1;
using TakeHome.API.Models;

namespace TakeHome.API.Services.v1
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
             var products =  await _appDbContext.Products
                .Select(x => new ProductResponseDto 
                {
                    ProductName = x.ProductName,
                    Sku = x.Sku
                })
                .ToListAsync();

            return products;
        }
    }
}
