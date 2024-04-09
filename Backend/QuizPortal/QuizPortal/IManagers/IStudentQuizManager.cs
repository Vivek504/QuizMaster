using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IStudentQuizManager
	{
		Task Add(StudentQuiz studentQuiz);

		Task<StudentQuiz> GetStudentQuiz(int studentQuizId);

		Task<StudentQuiz> GetStudentQuizByStudentCourseIdAndQuizId(int studentCourseId, int quizId);

		Task UpdateStudentQuiz(StudentQuiz studentQuiz);

		Task EndStudentQuiz(StudentQuiz studentQuiz);
	}
}
