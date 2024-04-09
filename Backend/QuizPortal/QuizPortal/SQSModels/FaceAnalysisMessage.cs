using System;
namespace QuizPortal.SQSModels
{
	public class FaceAnalysisMessage: MessageBody
	{
        public required int VideoId { get; set; }
    }
}
