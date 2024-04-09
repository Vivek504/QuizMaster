using System;
namespace QuizPortal.RequestModels
{
	public class EndStudentQuizRequest
	{
		public required int StudentQuizId { get; set; }
		public required DateTime EndDateTime { get; set; }
	}
}

