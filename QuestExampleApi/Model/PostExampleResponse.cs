using System.Text.Json.Serialization;

namespace QuestExampleApi.Model
{
    public class PostExampleResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
