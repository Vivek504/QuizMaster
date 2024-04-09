using System;
namespace QuizPortal.SQSModels
{
	public class FindAnswerAccuracyMessage: MessageBody
	{
		public int AnswerId { get; set; }
	}
}

