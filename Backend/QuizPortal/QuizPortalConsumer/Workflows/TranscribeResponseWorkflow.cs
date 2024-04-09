using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.Workflows
{
	public class TranscribeResponseWorkflow
	{
        public async Task HandleTranscribeResponse(IVideoManager _videoManager, IAnswerManager _answerManager, TranscribeResponseMessage transcribeResponseMessage)
		{
			Video video = await _videoManager.GetVideo(transcribeResponseMessage.VideoId);
			if(video == null)
			{
				return;
			}

			Answer answer = new Answer()
			{
				StudentQuiz = video.StudentQuiz,
				Question = video.Question,
				AnswerText = transcribeResponseMessage.Text,
				IsLastAnswer = transcribeResponseMessage.IsLastAnswer
			};

			await _answerManager.Add(answer);
        }

    }
}
