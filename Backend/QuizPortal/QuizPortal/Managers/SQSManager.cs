using System;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using QuizPortal.IManagers;
using QuizPortal.Utility;

namespace QuizPortal.Managers
{
    public class SQSManager: ISQSManager
    {
        private readonly IConfiguration _configuration;

        private readonly IAmazonSQS _amazonSQS;

        public SQSManager(IConfiguration configuration, IAmazonSQS amazonSQS)
        {
            _configuration = configuration;
            _amazonSQS = amazonSQS;
        }

        public async Task<string> GetQueueUrlAsync(string queueName)
        {
            try
            {
                var response = await _amazonSQS.GetQueueUrlAsync(new GetQueueUrlRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                var response = await _amazonSQS.CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = queueName
                });

                return response.QueueUrl;
            }
        }

        public async Task<bool> PublishToQueueAsync(string queueUrl, string message)
        {
            await _amazonSQS.SendMessageAsync(new SendMessageRequest
            {
                MessageBody = message,
                QueueUrl = queueUrl
            });

            return true;
        }

        public async Task<List<QueueMessage>> ReceiveMessageAsync(string queueUrl, int maxMessages = 1)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxMessages
            };

            var messages = await _amazonSQS.ReceiveMessageAsync(request);

            return messages.Messages.Select(m => new QueueMessage
            {
                MessageId = m.MessageId,
                Body = m.Body,
                Handle = m.ReceiptHandle
            }).ToList();
        }

        public async Task DeleteMessageAsync(string queueUrl, string id)
        {
            await _amazonSQS.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = id
            });
        }
    }
}
