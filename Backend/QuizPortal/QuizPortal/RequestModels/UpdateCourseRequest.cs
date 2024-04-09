using System;
namespace QuizPortal.RequestModels
{
	public class UpdateCourseRequest
	{
		public required int Id { get; set; }

		public required string courseName { get; set; }
	}
}

