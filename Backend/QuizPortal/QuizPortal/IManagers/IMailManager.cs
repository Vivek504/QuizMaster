using System;
namespace QuizPortal.IManagers
{
	public interface IMailManager
	{
        public Task SendEmailForAccountCreation(string email, string password);

        public Task SendEmailToResetPassword(string email, string url);
    }
}

