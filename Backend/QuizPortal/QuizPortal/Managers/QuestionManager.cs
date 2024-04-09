using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class QuestionManager: IQuestionManager
	{
        private readonly AppDBContext _context;

		public QuestionManager(AppDBContext context)
		{
            _context = context;
		}

        public async Task Add(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            Question? question = await _context.Questions.FindAsync(questionId);
            return question;
        }

        public async Task<List<Question>> GetQuizQuestions(int quizId)
        {
            List<Question> questions = await _context.Questions
                .Where(q => q.Quiz.Id == quizId)
                .Select(q => new Question
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText
                })
                .ToListAsync();

            return questions;
        }

        public async Task<Question> UpdateQuestion(Question updatedQuestion)
        {
            Question? question = await _context.Questions.FindAsync(updatedQuestion.Id);

            if(question == null)
            {
                return null;
            }

            question.QuestionText = updatedQuestion.QuestionText;
            await _context.SaveChangesAsync();

            return question;
        }
    }
}
