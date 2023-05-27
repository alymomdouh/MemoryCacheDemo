using API.Dtos;
using API.IService;
using API.Models;
using Newtonsoft.Json;

namespace API.Service
{
    public class ProductService : IProductService
    {
        public string ProductApiURL = "https://dummyjson.com/products";

        public async Task AddAsync(Product value)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Product>> GetProducts()
        {
            using (var client = new HttpClient())
            {
                // Send a GET request to the API
                HttpResponseMessage response = await client.GetAsync(ProductApiURL);

                // Check the status code of the response
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var data = JsonConvert.DeserializeObject<ProductResponse>(responseString);
                        return data.Products;
                    }
                }
            }
            return null;
        }

        public async Task RemoveProductAsync(Product filteredData)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
