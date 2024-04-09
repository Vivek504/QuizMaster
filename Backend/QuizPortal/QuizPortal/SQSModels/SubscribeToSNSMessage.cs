using System;
namespace QuizPortal.SQSModels
{
	public class SubscribeToSNSMessage: MessageBody
	{
		public required string UserEmail { get; set; }
	}
}

