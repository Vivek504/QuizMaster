using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IAnswerManager
	{
		Task Add(Answer answer);

		Task<Answer> GetById(int answerId);

		Task<List<Answer>> GetAnswersByStudentQuizId(int studentQuizId);
	}
}

