using System;
using QuizPortal.Utility;

namespace QuizPortal.RequestModels
{
	public class AddQuizRequest
	{
		public required int CourseId { get; set; }

		public required string QuizTitle { get; set; }

		public required DateTime StartDateTime { get; set; }

		public required DateTime EndDateTime { get; set; }

		public required string QuizType { get; set; }
	}
}

