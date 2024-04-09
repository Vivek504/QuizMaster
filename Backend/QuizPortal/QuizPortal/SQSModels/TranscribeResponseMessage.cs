using System;
namespace QuizPortal.SQSModels
{
	public class TranscribeResponseMessage: MessageBody
	{
        public required int VideoId { get; set; }
        public required string Text { get; set; }
        public required bool IsLastAnswer { get; set; }
    }
}
