using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class AccuracyManager: IAccuracyManager
	{
        private readonly AppDBContext _context;

		public AccuracyManager(AppDBContext context)
		{
            _context = context;
		}

        public async Task Add(Accuracy accuracy)
        {
            await _context.Accuracies.AddAsync(accuracy);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Accuracy>> GetAccuraciesByStudentQuizId(int studentQuizId)
        {
            return await _context.Accuracies
                .Include(a => a.Answer)
                .Where(a => a.Answer.StudentQuiz.Id == studentQuizId)
                .ToListAsync();
        }
    }
}

