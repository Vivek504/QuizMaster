using System;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.IManagers
{
	public interface IMailManager
	{
        Task SendMailAsync(MailData mailData);
    }
}
