using API.IService;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IProductService _productService;
        private ILogger<ProductController> _logger;
        public ProductController(ICacheService cacheService, IProductService productService, ILogger<ProductController> logger)
        {
            _cacheService = cacheService;
            _productService = productService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet("products")]
        public async Task<List<Product>> Get()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of products from cache.");
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                return cacheData.ToList();
            }
            _logger.Log(LogLevel.Information, "products list not found in cache. Fetching from database.");

            // Set cache options. We will cache the data for 1 day.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                      // .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                      // .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                      .SetAbsoluteExpiration(TimeSpan.FromDays(1))
                      //This sets the priority of the cached object. By default, the priority will be Normal, but we can set it to Low, High, Never Remove
                      .SetPriority(CacheItemPriority.Normal)
                      .SetSize(1024);//Setting a Size Limit on Memory Cache

            cacheData = await _productService.GetProducts();
            _cacheService.SetData<IEnumerable<Product>>("product", cacheData, cacheEntryOptions);
            return cacheData.ToList();
        }
        [HttpGet("product")]
        public async Task<Product> GetAsync(int id)
        {
            Product filteredData;
            var cacheData = _cacheService.GetData<IEnumerable<Product>>("product");
            if (cacheData != null)
            {
                filteredData = cacheData.First(x => x.Id == id);
                return filteredData;
            }
            filteredData = (await _productService.GetProducts()).First(x => x.Id == id);
            return filteredData;
        }
        [HttpPost("addproduct")]
        public async Task Post(Product value)
        {
            await _productService.AddAsync(value);
            _cacheService.RemoveData("product");
        }
        [HttpPut("updateproduct")]
        public async Task Put(Product product)
        {
            await _productService.UpdateProductAsync(product);
            _cacheService.RemoveData("product");
        }
        [HttpDelete("deleteproduct")]
        public async Task Delete(int Id)
        {
            var filteredData = (await _productService.GetProducts()).First(x => x.Id == Id);
            await _productService.RemoveProductAsync(filteredData);
            _cacheService.RemoveData("product");
        }
    }
}
