using System;
namespace QuizPortalConsumer.Utility
{
	public class AppSettingsConstantsPath
	{
		public const string SNS_ARN = "AWS:SNS:ARN";
        public const string mailQueueName = "AWS:SQS:MailQueueName";
        public const string faceAnalysisQueueName = "AWS:SQS:FaceAnalysisQueueName";
        public const string S3BucketName = "AWS:S3:BucketName";
        public const string TranscribeRequestQueueName = "AWS:SQS:TranscribeRequestQueue";
        public const string TranscribeResponseQueueName = "AWS:SQS:TranscribeResponseQueue";
        public const string FindAnswerAccuracyQueueName = "AWS:SQS:FindAnswerAccuracyQueue";
        public const string ResultQueueName = "AWS:SQS:ResultQueue";

        public const string OpenAIAPIKey = "OpenAISetting:APIKey";
        public const string OpenAIBaseUrl = "OpenAISetting:BaseUrl";

        public const string VIDEO_STORAGE_PATH = "VideoStoragePath";
    }
}

