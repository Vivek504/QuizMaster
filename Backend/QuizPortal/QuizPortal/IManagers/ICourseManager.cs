using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface ICourseManager
	{
		public Task Add(Course course);

		public Task<Course> GetByCourseName(string courseName);

		public Task<Course> GetCourse(int courseId);

		public Task<Course> UpdateCourse(int courseId, Course updatedCourse);
	}
}
