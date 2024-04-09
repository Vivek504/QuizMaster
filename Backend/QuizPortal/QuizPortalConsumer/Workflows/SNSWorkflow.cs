using System;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using QuizPortal.SQSModels;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.Workflows
{
	public class SNSWorkflow
	{
        private readonly IConfiguration _configuration;

		private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

		public SNSWorkflow(IConfiguration configuration, IAmazonSimpleNotificationService amazonSimpleNotificationService)
		{
            _configuration = configuration;

			_amazonSimpleNotificationService = amazonSimpleNotificationService;
		}

        public void SubscribeEmailToTopic(SubscribeToSNSMessage subscribeToSNSMessage)
        {
            //var request = new SubscribeRequest
            //{
            //    TopicArn = _configuration[AppSettingsConstantsPath.SNS_ARN],
            //    Protocol = "email",
            //    Endpoint = subscribeToSNSMessage.UserEmail
            //};

            //_amazonSimpleNotificationService.SubscribeAsync(request);
        }

		public void SendEmail(SendEmailMessage sendEmailMessage)
		{
            //try
            //{
            //    var messageAttributes = new Dictionary<string, MessageAttributeValue>
            //    {
            //        {
            //            "email",
            //            new MessageAttributeValue
            //            {
            //                DataType = "String",
            //                StringValue = sendEmailMessage.UserEmail
            //            }
            //        }
            //    };

            //    var request = new PublishRequest
            //    {
            //        TopicArn = _configuration[AppSettingsConstantsPath.SNS_ARN],
            //        Subject = sendEmailMessage.EmailSubject,
            //        Message = sendEmailMessage.EmailContent,
            //        MessageAttributes = messageAttributes
            //    };

            //    _amazonSimpleNotificationService.PublishAsync(request);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }
    }
}
