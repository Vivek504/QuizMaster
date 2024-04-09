using System;
using QuizPortal.Utility;

namespace QuizPortal.SQSModels
{
	public class MessageBody
	{
		public required MessageTypeEnum MessageType { get; set; }
	}
}
