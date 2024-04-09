using System;
namespace QuizPortal.RequestModels
{
	public class ResetPasswordRequest
	{
		public required string Code { get; set; }

		public required string Password { get; set; }
	}
}

