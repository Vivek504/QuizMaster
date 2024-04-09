using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IProfessorManager
	{
		public Task Add(Professor professor);

		public Task<Professor> GetByUserId(string userId);
	}
}
