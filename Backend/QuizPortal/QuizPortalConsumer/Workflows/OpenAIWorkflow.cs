using System;
using Newtonsoft.Json;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortalConsumer.IManagers;
using QuizPortalConsumer.ResponseModels;
using QuizPortalConsumer.Utility;

namespace QuizPortalConsumer.Workflows
{
	public class OpenAIWorkflow
	{
		public async Task FindAnswerAccuracy(IAnswerManager _answerManager, IAccuracyManager _accuracyManager, IVideoManager _videoManager,
            IStudentQuizManager _studentQuizManager, IOpenAIManager _openAIManager,
			FindAnswerAccuracyMessage findAnswerAccuracyMessage)
		{
			Answer answer = await _answerManager.GetById(findAnswerAccuracyMessage.AnswerId);
			if(answer == null)
			{
				return;
			}

			string prompt = OpenAIConstants.GetFindAccuracyPrompt(answer.Question.QuestionText, answer.AnswerText);

			string openAIResponse = await _openAIManager.GetResponse(prompt);
			if (openAIResponse == null)
			{
				return;
			}

			AccuracyResponse accuracyResponse = JsonConvert.DeserializeObject<AccuracyResponse>(openAIResponse);

			Accuracy accuracy = new Accuracy()
			{
				Answer = answer,
				AccuracyPercentage = accuracyResponse.Accuracy
			};
			await _accuracyManager.Add(accuracy);

			bool foundAccuracyForAll = false;

            List<Accuracy> accuracies = await _accuracyManager.GetAccuraciesByStudentQuizId(answer.StudentQuiz.Id);
            if (answer.StudentQuiz.Quiz.QuizType == QuizPortal.Utility.QuizTypeEnum.RECORDING)
			{
				List<Video> videos = await _videoManager.GetVideosByStudentQuizId(answer.StudentQuiz.Id);
				if(accuracies != null && videos != null)
				{
					if(accuracies.Count == videos.Count)
					{
						foundAccuracyForAll = true;
					}
				}
			}
			else if(answer.StudentQuiz.Quiz.QuizType == QuizPortal.Utility.QuizTypeEnum.TYPING)
			{
				List<Answer> answers = await _answerManager.GetAnswersByStudentQuizId(answer.StudentQuiz.Id);
				if(accuracies != null)
				{
					Answer? lastAnswer = answers.Where(a => a.IsLastAnswer).FirstOrDefault();
					if(lastAnswer != null)
					{
						if(accuracies.Count == answers.Count)
						{
							foundAccuracyForAll = true;
                        }
					}
				}
			}

			if (foundAccuracyForAll)
			{
                answer.StudentQuiz.FoundAccuracy = true;
                await _studentQuizManager.UpdateStudentQuiz(answer.StudentQuiz);
            }
		}
	}
}
