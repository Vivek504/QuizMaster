using System;

namespace QuizPortalConsumer.Utility
{
	public class OpenAIConstants
	{
		public const string OpenAIModel = "gpt-3.5-turbo";
		public const string OpenAIRole = "user";

		public static string GetFindAccuracyPrompt(string question, string answer)
		{
			string prompt = "I would request you to provide me the accuracy of an answer given by a student.\n";
			prompt += $"Question: \"{question}\"\n";
			prompt += $"Answer: \"{answer}\"\n";
			prompt += "Please provide the response in the given below json format and don't provide anything else.\n";
			prompt += "{\"accuracy\": \"<in percentage(double)>\"}";

			return prompt;
        }
    }
}
