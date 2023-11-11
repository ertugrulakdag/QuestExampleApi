using System.Text.Json.Serialization;

namespace QuestExampleApi.Model
{
    public class PostExampleRequest
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
