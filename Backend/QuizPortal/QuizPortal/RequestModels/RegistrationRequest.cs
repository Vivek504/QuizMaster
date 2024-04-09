using System;
namespace QuizPortal.RequestModels
{
	public class RegistrationRequest
	{
		public required string Email { get; set; }

		public required string Password { get; set; }

		public required string Role { get; set; }
	}
}

