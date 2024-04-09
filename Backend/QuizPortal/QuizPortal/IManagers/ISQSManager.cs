using System;
using QuizPortal.Utility;

namespace QuizPortal.IManagers
{
	public interface ISQSManager
	{
        Task<string> GetQueueUrlAsync(string queueName);

        Task<bool> PublishToQueueAsync(string queueUrl, string message);

        Task<List<QueueMessage>> ReceiveMessageAsync(string queueUrl, int maxMessages = 1);

        Task DeleteMessageAsync(string queueUrl, string id);
    }
}
