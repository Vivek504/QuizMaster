using System;
namespace QuizPortal.SQSModels
{
	public class SendEmailMessage: MessageBody
	{
		public required string UserEmail { get; set; }
		public required string EmailSubject { get; set; }
		public required string EmailContent { get; set; }
	}
}
