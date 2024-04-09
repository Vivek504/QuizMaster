using System;
namespace QuizPortal.RequestModels
{
	public class AddStudentQuizRequest
	{
		public required int StudentCourseId { get; set; }
		public required int QuizId { get; set; }
		public required DateTime StartDateTime { get; set; }
	}
}
