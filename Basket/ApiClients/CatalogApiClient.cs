using Catelog.Models;

namespace Basket.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<Product> GetProductById(int id)
    {
        return (await httpClient.GetFromJsonAsync<Product>($"/api/products/{id}"))!;
    }
}
