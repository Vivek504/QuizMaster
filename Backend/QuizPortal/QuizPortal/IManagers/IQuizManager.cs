using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IQuizManager
	{
		public Task Add(Quiz quiz);

		public Task<Quiz> GetQuiz(int quizId);

		public Task<List<Quiz>> GetQuizzesByCourseId(int courseId);

		public Task<Quiz> UpdateQuiz(Quiz updatedQuiz);
	}
}
