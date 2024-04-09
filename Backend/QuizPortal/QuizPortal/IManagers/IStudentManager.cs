using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IStudentManager
	{
		public Task Add(Student student);

		public Task<Student> GetStudentByEmail(string Email);

		public Task<Student> GetStudent(string userId);
	}
}

