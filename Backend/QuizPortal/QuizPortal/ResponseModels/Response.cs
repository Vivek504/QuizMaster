using System;
using System.Net;

namespace QuizPortal.ResponseModels
{
	public class Response
	{
		public required HttpStatusCode ResponseCode { get; set; }

		public required string Message { get; set; }

		public object? Payload { get; set; }
	}
}
