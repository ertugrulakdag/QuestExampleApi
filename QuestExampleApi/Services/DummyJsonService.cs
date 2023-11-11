using QuestExampleApi.Controllers;
using QuestExampleApi.Model;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuestExampleApi.Services
{
    /// <summary>
    /// url:dummyjson.com
    /// </summary>
    public class DummyJsonService//TypedClient yöntemidir. Bu yöntem de _client nesnelerimizi kendimiz isimlendirip oluşturabiliyoruz. Ayrıca DI yardımıyla istediğimiz an nesneler çağrılıp sonrasın GC tarafından silinmesiyle büyük performans kazançları sağlıyor.
    {
        public HttpClient _client { get; }
        public DummyJsonService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://dummyjson.com/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Ornek");
            _client = client;
        }
        public async Task<ProductDTO?> GetProduct(int Id)
        {
            var response = await _client.GetAsync($"products/{Id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ProductDTO>(responseStream);
        }
        public async Task<T?> GetRequest<T>(string requestUri)
        {
            try
            {
                var response = await _client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    if (responseStream != null)
                    {
                        return await JsonSerializer.DeserializeAsync<T>(responseStream);
                    }
                    else
                    {
                        throw new Exception("Cevap Dönmedi");
                    }
                }
                else
                {
                    throw new Exception(response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}, {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} Url: {requestUri}");
            }
        }
        public async Task<TResponse?> PostRequest<TRequest, TResponse>(string requestUri, TRequest content)
        {
            try
            {
                var serialized = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(requestUri, serialized);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody != null)
                    {
                        return JsonSerializer.Deserialize<TResponse>(responseBody);
                    }
                    else
                    {
                        throw new Exception("Cevap Dönmedi");
                    }
                }
                else
                {
                    throw new Exception(response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} {(ex.InnerException != null ? " Inner Exception: " + ex.InnerException.Message : string.Empty)} Url: {requestUri}");
            }

        }
    }
}
