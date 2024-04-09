using System;
using System.Net.Http.Headers;
using System.Text;
using Amazon.Rekognition;
using Newtonsoft.Json;
using QuizPortalConsumer.IManagers;
using QuizPortalConsumer.RequestModels;
using QuizPortalConsumer.ResponseModels;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.Managers
{
	public class OpenAIManager: IOpenAIManager
	{
        public readonly IConfiguration _configuration;

        public OpenAIManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetResponse(string prompt)
        {
            var apiKey = _configuration[AppSettingsConstantsPath.OpenAIAPIKey];
            var baseUrl = _configuration[AppSettingsConstantsPath.OpenAIBaseUrl];

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new OpenAIRequest
            {
                Model = OpenAIConstants.OpenAIModel,
                Messages = new List<OpenAIMessageRequest>{
                    new OpenAIMessageRequest
                    {
                        Role = OpenAIConstants.OpenAIRole,
                        Content = prompt
                    }
                },
                MaxTokens = 200
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<OpenAIErrorResponse>(responseJson);
                Console.WriteLine(errorResponse.Error.Message);
                return null;
            }

            var data = JsonConvert.DeserializeObject<OpenAISuccessResponse>(responseJson);
            var responseText = data.choices[0].message.content;

            return responseText;
        }
    }
}
