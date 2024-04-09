using System;
using Newtonsoft.Json;

namespace QuizPortalConsumer.RequestModels
{
	public class OpenAIRequest
	{
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<OpenAIMessageRequest> Messages { get; set; }

        [JsonProperty("temperature")]
        public float Temperature { get; set; }

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }
    }

    public class OpenAIMessageRequest
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}

