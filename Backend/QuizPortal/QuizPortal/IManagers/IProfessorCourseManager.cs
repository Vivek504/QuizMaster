using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IProfessorCourseManager
	{
		public Task Add(ProfessorCourse professorCourse);

		public Task<ProfessorCourse> GetByProfessorIdAndCourseId(int professorId, int courseId);

		public Task<List<ProfessorCourse>> GetProfessorCourses(int professorId);
	}
}

