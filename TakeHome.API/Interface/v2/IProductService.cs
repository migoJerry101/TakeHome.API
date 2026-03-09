using TakeHome.API.Dtos.v2;

namespace TakeHome.API.Interface.v2
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync();
    }
}
