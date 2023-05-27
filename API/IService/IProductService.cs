using API.Models;

namespace API.IService
{
    public interface IProductService
    {
        Task AddAsync(Product value);
        Task<IList<Product>> GetProducts();
        Task RemoveProductAsync(Product filteredData);
        Task UpdateProductAsync(Product product);
    }
}
