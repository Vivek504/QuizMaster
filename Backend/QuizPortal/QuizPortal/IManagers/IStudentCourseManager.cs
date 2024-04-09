using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IStudentCourseManager
	{
		public Task Add(StudentCourse studentCourse);

		public Task<List<StudentCourse>> GetCourseStudents(int courseId);

		public Task<List<StudentCourse>> GetStudentCourses(int studentId);

		public Task<StudentCourse> GetByStudentAndCourseId(int studentId, int courseId);

		public Task<StudentCourse> GetById(int studentCourseId);
	}
}
