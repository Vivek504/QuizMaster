using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class StudentResultManager: IStudentResultManager
	{
        private readonly AppDBContext _context;

        public StudentResultManager(AppDBContext context)
		{
			_context = context;
		}

        public async Task Add(StudentResult studentResult)
        {
            await _context.StudentResults.AddAsync(studentResult);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentResult> GetById(int id)
        {
            return await _context.StudentResults.FindAsync(id);
        }

        public async Task<StudentResult> GetByStudentQuizId(int studentQuizId)
        {
            return await _context.StudentResults
                .Include(sq => sq.StudentQuiz)
                .Where(sq => sq.StudentQuiz.Id == studentQuizId)
                .FirstOrDefaultAsync();
        }

        public async Task Update(StudentResult studentResult)
        {
            await _context.SaveChangesAsync();
        }
    }
}
