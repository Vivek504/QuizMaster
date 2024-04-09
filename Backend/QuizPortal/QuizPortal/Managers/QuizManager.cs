using System;
using QuizPortal.IManagers;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.Utility;

namespace QuizPortal.Managers
{
	public class QuizManager: IQuizManager
	{
		private readonly AppDBContext _context;

		public QuizManager(AppDBContext context)
		{
			_context = context;
		}

		public async Task Add(Quiz quiz)
		{
			await _context.Quizzes.AddAsync(quiz);
			await _context.SaveChangesAsync();
		}

        public async Task<Quiz> GetQuiz(int quizId)
        {
			Quiz? quiz = await _context.Quizzes
				.Include(q => q.Course)
				.Where(q => q.Id == quizId)
				.FirstOrDefaultAsync();
            return quiz;
        }

        public async Task<List<Quiz>> GetQuizzesByCourseId(int courseId)
        {
			List<Quiz> quizzes = await _context.Quizzes
				.Where(q => q.Course.Id == courseId)
				.ToListAsync();

			return quizzes;
        }

        public async Task<Quiz> UpdateQuiz(Quiz updatedQuiz)
        {
			Quiz? quiz = await _context.Quizzes.FindAsync(updatedQuiz.Id);
			if(quiz == null)
			{
				return null;
			}

			quiz.QuizTitle = updatedQuiz.QuizTitle;
			quiz.StartDateTime = updatedQuiz.StartDateTime;
			quiz.EndDateTime = updatedQuiz.EndDateTime;
			quiz.QuizType = updatedQuiz.QuizType;

			await _context.SaveChangesAsync();

			return quiz;
        }
    }
}
