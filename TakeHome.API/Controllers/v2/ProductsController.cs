using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TakeHome.API.Dtos.v2;
using TakeHome.API.Interface.v2;

namespace TakeHome.API.Controllers.v2
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v2/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> Get()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
    }
}
