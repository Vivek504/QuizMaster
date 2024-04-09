using System;
namespace QuizPortal.RequestModels
{
	public class StudentRegistrationRequest
	{
        public required string Email { get; set; }

        public required int CourseId { get; set; }
    }
}
