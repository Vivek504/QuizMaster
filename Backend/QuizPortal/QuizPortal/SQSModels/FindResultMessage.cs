using System;
namespace QuizPortal.SQSModels
{
	public class FindResultMessage: MessageBody
	{
		public required int StudentQuizId { get; set; }
	}
}
