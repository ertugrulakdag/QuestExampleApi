using QuestExampleApi.Model;
using System.Text.Json;

namespace QuestExampleApi.Services
{
    /// <summary>
    /// url:dummyjson.com
    /// </summary>
    public class DummyJsonService//TypedClient yöntemidir. Bu yöntem de client nesnelerimizi kendimiz isimlendirip oluşturabiliyoruz. Ayrıca DI yardımıyla istediğimiz an nesneler çağrılıp sonrasın GC tarafından silinmesiyle büyük performans kazançları sağlıyor.
    {
        public HttpClient Client { get; }

        public DummyJsonService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://dummyjson.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            Client = client;
        }

        public async Task<ProductDTO?> GetProduct(int Id)
        {
            var response = await Client.GetAsync($"products/{Id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ProductDTO>(responseStream);
        }
    }
}
