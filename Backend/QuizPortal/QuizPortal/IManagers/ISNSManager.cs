using System;
namespace QuizPortal.IManagers
{
	public interface ISNSManager
	{
		Task SendEmailForAccountCreation(string email, string password);

        Task SendEmailToResetPassword(string email, string url);

		Task SendSubscribeRequest(string email);
	}
}

