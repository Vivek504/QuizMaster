using System;
namespace QuizPortal.RequestModels
{
	public class UpdateQuizRequest
	{
        public required int Id { get; set; }

        public required string QuizTitle { get; set; }

        public required DateTime StartDateTime { get; set; }

        public required DateTime EndDateTime { get; set; }

        public required string QuizType { get; set; }
    }
}
