using TakeHome.API.Dtos.v1;

namespace TakeHome.API.Interface.v1
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    }
}
