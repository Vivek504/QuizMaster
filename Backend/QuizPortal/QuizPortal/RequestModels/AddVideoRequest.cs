using System;
namespace QuizPortal.RequestModels
{
	public class AddVideoRequest
	{
		public required int StudentQuizId { get; set; }
		public required int QuestionId { get; set; }
		public required string S3ObjectName { get; set; }
		public required bool IsLastVideo { get; set; }
	}
}

