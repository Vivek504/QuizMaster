using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.Utility;
using QuizPortal.SQSModels;
using Newtonsoft.Json;

namespace QuizPortal.Managers
{
	public class AnswerManager: IAnswerManager
	{
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _context;
        private readonly ISQSManager _sqsManager;

        public AnswerManager(IConfiguration configuration, AppDBContext context, ISQSManager sqsManager)
        {
            _configuration = configuration;
            _context = context;
            _sqsManager = sqsManager;
        }

        public async Task Add(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            string accuracyQueueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.FindAnswerAccuracyQueueName]);

            FindAnswerAccuracyMessage findAnswerAccuracyMessage = new FindAnswerAccuracyMessage()
            {
                MessageType = MessageTypeEnum.FIND_ANSWER_ACCURACY,
                AnswerId = answer.Id
            };

            await _sqsManager.PublishToQueueAsync(accuracyQueueUrl, JsonConvert.SerializeObject(findAnswerAccuracyMessage));
        }

        public async Task<List<Answer>> GetAnswersByStudentQuizId(int studentQuizId)
        {
            return await _context.Answers
                .Include(a => a.StudentQuiz)
                .Include(a => a.Question)
                .Where(a => a.StudentQuiz.Id == studentQuizId)
                .ToListAsync();
        }

        public async Task<Answer> GetById(int answerId)
        {
            return await _context.Answers
                .Include(a => a.StudentQuiz)
                .Include(a => a.StudentQuiz.Quiz)
                .Include(a => a.Question)
                .Where(a => a.Id == answerId)
                .FirstOrDefaultAsync();
        }
    }
}
