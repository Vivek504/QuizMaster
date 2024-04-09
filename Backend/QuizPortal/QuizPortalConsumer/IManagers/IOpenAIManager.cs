using System;
namespace QuizPortalConsumer.IManagers
{
	public interface IOpenAIManager
	{
        Task<string> GetResponse(string prompt);
    }
}

