using API.Models;

namespace API.IService
{
    public interface IProductService
    {
        Task<IList<Product>> GetProducts();
    }
}
