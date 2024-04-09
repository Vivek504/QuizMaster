using System;
namespace QuizPortal.RequestModels
{
	public class AddAnswerRequest
	{
		public required int StudentQuizId { get; set; }
		public required int QuestionId { get; set; }
		public required string AnswerText { get; set; }
		public required bool IsLastAnswer { get; set; }
	}
}

