using System;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortal.DBModels;
using Amazon.Rekognition.Model;

namespace QuizPortalConsumer.Workflows
{
	public class ResultWorkflow
	{
		public async Task FindResult(IStudentQuizManager _studentQuizManager, IStudentResultManager _studentResultManager,
			IAccuracyManager _accuracyManager, IQuestionManager _questionManager,
            FindResultMessage findResultMessage)
		{
            StudentQuiz? studentQuiz = await _studentQuizManager.GetStudentQuiz(findResultMessage.StudentQuizId);
            if (studentQuiz == null)
            {
                return;
            }

            StudentResult studentResult = await _studentResultManager.GetByStudentQuizId(findResultMessage.StudentQuizId);
			if(studentResult == null)
			{
                studentResult = new StudentResult()
                {
                    StudentQuiz = studentQuiz,
                    IsCheatingFound = false
                };
                await _studentResultManager.Add(studentResult);
            }

			List<Accuracy> accuracies = await _accuracyManager.GetAccuraciesByStudentQuizId(findResultMessage.StudentQuizId);
			List<Question> questions = await _questionManager.GetQuizQuestions(studentQuiz.Quiz.Id);

			if(accuracies != null && questions != null)
			{
				double score = 0;
				foreach(var accuracy in accuracies)
				{
					score += accuracy.AccuracyPercentage;
				}

                score /= questions.Count;

				studentResult.Score = score;
				await _studentResultManager.Update(studentResult);
			}
		}
	}
}
