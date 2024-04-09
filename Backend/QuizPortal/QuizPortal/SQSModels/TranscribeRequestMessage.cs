using System;
namespace QuizPortal.SQSModels
{
	public class TranscribeRequestMessage: MessageBody
	{
		public required int VideoId { get; set; }
		public required string BucketName { get; set; }
		public required string ObjectName { get; set; }
		public required bool IsLastAnswer { get; set; }
	}
}
