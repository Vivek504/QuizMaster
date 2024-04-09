using System;
using Newtonsoft.Json;

namespace QuizPortalConsumer.ResponseModels
{
	public class AccuracyResponse
	{
        [JsonProperty("accuracy")]
        public double Accuracy { get; set; }
	}
}

