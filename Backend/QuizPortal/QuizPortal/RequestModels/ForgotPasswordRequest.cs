using System;
namespace QuizPortal.RequestModels
{
	public class ForgotPasswordRequest
	{
		public required string Email { get; set; }

		public required string ResetPasswordUrl { get; set; }
	}
}
