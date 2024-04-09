using System;
namespace QuizPortal.RequestModels
{
	public class AddQuestionRequest
	{
		public required int QuizId { get; set; }

		public required string QuestionText { get; set; }
	}
}

