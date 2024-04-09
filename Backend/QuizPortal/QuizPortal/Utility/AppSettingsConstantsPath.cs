using System;
namespace QuizPortal.Utility
{
	public class AppSettingsConstantsPath
	{
		public const string jwtValidAudience = "JWTKey:ValidAudience";
		public const string jwtValidIssuer = "JWTKey:ValidIssuer";
		public const string jwtTokenExpiryTimeInHour = "JWTKey:TokenExpiryTimeInHour";
		public const string jwtSecret = "JWTKey:Secret";

		public const string mailQueueName = "AWS:SQS:MailQueueName";
		public const string faceAnalysisQueueName = "AWS:SQS:FaceAnalysisQueueName";
		public const string TranscribeRequestQueueName = "AWS:SQS:TranscribeRequestQueue";
        public const string TranscribeResponseQueueName = "AWS:SQS:TranscribeResponseQueue";
        public const string FindAnswerAccuracyQueueName = "AWS:SQS:FindAnswerAccuracyQueue";
		public const string ResultQueueName = "AWS:SQS:ResultQueue";

        public const string resetPasswordFrontendURL = "ResetPasswordURL";
    }
}
