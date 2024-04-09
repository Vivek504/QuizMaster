using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class ProfessorManager: IProfessorManager
	{
		private readonly AppDBContext _context;

		public ProfessorManager(AppDBContext context)
		{
			_context = context;
		}

        public async Task Add(Professor professor)
        {
            await _context.Professors.AddAsync(professor);
            await _context.SaveChangesAsync();
        }

        public async Task<Professor> GetByUserId(string userId)
        {
            Professor? professor = await _context.Professors.FirstOrDefaultAsync(p => p.User.Id == userId);
            return professor;
        }
    }
}

