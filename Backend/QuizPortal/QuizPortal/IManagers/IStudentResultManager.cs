using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IStudentResultManager
	{
		Task Add(StudentResult studentResult);

		Task<StudentResult> GetById(int id);

		Task<StudentResult> GetByStudentQuizId(int studentQuizId);

		Task Update(StudentResult studentResult);
	}
}
