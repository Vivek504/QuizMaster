using System;
namespace QuizPortal.RequestModels
{
	public class UpdateQuestionRequest
	{
		public required int Id { get; set; }

		public required string QuestionText { get; set; }
	}
}

