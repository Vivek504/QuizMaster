﻿using System;
using Newtonsoft.Json;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortal.Utility;

namespace QuizPortal.Managers
{
	public class MailManager: IMailManager
	{
        private readonly ISQSManager _sqsManager;
        private readonly IConfiguration _configuration;

        public MailManager(ISQSManager sqsManager, IConfiguration configuration)
		{
            _sqsManager = sqsManager;
            _configuration = configuration;
        }

        public async Task SendEmailForAccountCreation(string email, string password)
        {
            string queueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.mailQueueName]);

            SendEmailMessage sendEmailMessage = new SendEmailMessage()
            {
                MessageType = MessageTypeEnum.SEND_MAIL,
                UserEmail = email,
                EmailSubject = EmailConstants.ACCOUNT_CREATION_EMAIL_SUBJECT,
                EmailContent = EmailConstants.GET_ACCOUNT_CREATION_EMAIL_CONTENT(email, password)
            };

            await _sqsManager.PublishToQueueAsync(queueUrl, JsonConvert.SerializeObject(sendEmailMessage));
        }

        public async Task SendEmailToResetPassword(string email, string url)
        {
            string queueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.mailQueueName]);

            SendEmailMessage sendEmailMessage = new SendEmailMessage()
            {
                MessageType = MessageTypeEnum.SEND_MAIL,
                UserEmail = email,
                EmailSubject = EmailConstants.RESET_PASSWORD_EMAIL_SUBJECT,
                EmailContent = EmailConstants.GET_RESET_PASSWORD_EMAIL_CONTENT(url)
            };

            await _sqsManager.PublishToQueueAsync(queueUrl, JsonConvert.SerializeObject(sendEmailMessage));
        }
    }
}
