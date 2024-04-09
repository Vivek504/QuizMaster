using System;
using QuizPortal.SQSModels;
using QuizPortalConsumer.IManagers;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.Workflows
{
	public class MailWorkflow
	{
		public async Task SendEmail(SendEmailMessage sendEmailMessage, IMailManager _mailManager)
		{
			MailData mailData = new MailData()
			{
				EmailToId = sendEmailMessage.UserEmail,
				EmailToName = sendEmailMessage.UserEmail,
				EmailSubject = sendEmailMessage.EmailSubject,
				EmailBody = sendEmailMessage.EmailContent
			};

			_mailManager.SendMailAsync(mailData);
		}
	}
}

