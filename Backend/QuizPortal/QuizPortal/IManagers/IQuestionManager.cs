using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IQuestionManager
	{
		public Task Add(Question question);

		public Task<Question> GetQuestion(int questionId);

		public Task<List<Question>> GetQuizQuestions(int quizId);

		public Task<Question> UpdateQuestion(Question updatedQuestion);
	}
}
