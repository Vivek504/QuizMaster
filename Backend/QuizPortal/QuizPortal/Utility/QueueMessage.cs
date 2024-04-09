using System;
using QuizPortal.SQSModels;

namespace QuizPortal.Utility
{
	public class QueueMessage
	{
        public required string MessageId { get; set; }
        public required string Body { get; set; }
        public required string Handle { get; set; }
    }
}
