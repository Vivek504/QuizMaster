using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IAccuracyManager
	{
		Task Add(Accuracy accuracy);

		Task<List<Accuracy>> GetAccuraciesByStudentQuizId(int studentQuizId);
	}
}

