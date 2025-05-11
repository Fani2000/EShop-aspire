using Catelog.Models;

namespace WebApp.ApiClient;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<List<Product>> GetProducts()
    {
        return (await httpClient.GetFromJsonAsync<List<Product>>($"/api/products"))!;
    }
    public async Task<Product> GetProductById(int id)
    {
        return (await httpClient.GetFromJsonAsync <Product> ($"/api/products/{id}"))!;
    }
}
