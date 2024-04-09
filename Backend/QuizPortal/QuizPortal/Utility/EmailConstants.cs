using System;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

namespace QuizPortal.Utility
{
	public class EmailConstants
	{
		public const string RESET_PASSWORD_EMAIL_SUBJECT = "Your Password Reset Link";

		public static string GET_RESET_PASSWORD_EMAIL_CONTENT(string url)
		{
			return $"Hi,\n\nHere's the link to reset your password: {url}\n\nBest regards,\nQuiz Master";
        }

		public const string ACCOUNT_CREATION_EMAIL_SUBJECT = "Your Account Credentials";

		public static string GET_ACCOUNT_CREATION_EMAIL_CONTENT(string email, string password)
		{
            return $"Hi,\n\nHere's the credentials to login into Quiz Master.\n\nEmail: {email}\n\nPassword: {password}\n\nBest regards,\nQuiz Master";
        }
    }
}

