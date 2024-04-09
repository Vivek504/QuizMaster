using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortal.Utility;

namespace QuizPortal.Managers
{
	public class StudentQuizManager: IStudentQuizManager
	{
        private readonly IConfiguration _configuration;
        private readonly AppDBContext _context;
        private readonly ISQSManager _sqsManager;

        public StudentQuizManager(IConfiguration configuration, AppDBContext context, ISQSManager sqsManager)
		{
            _configuration = configuration;
            _context = context;
            _sqsManager = sqsManager;
        }

        public async Task Add(StudentQuiz studentQuiz)
        {
            await _context.StudentQuizzes.AddAsync(studentQuiz);
            await _context.SaveChangesAsync();
        }

        public async Task EndStudentQuiz(StudentQuiz studentQuiz)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<StudentQuiz> GetStudentQuiz(int studentQuizId)
        {
            return await _context.StudentQuizzes
                .Include(sq => sq.StudentCourse)
                .Include(sq => sq.Quiz)
                .Where(sq => sq.Id == studentQuizId)
                .FirstOrDefaultAsync();
        }

        public async Task<StudentQuiz> GetStudentQuizByStudentCourseIdAndQuizId(int studentCourseId, int quizId)
        {
            return await _context.StudentQuizzes
                .Include(sq => sq.StudentCourse)
                .Include(sq => sq.Quiz)
                .Where(sq => sq.StudentCourse.Id == studentCourseId && sq.Quiz.Id == quizId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateStudentQuiz(StudentQuiz studentQuiz)
        {
            if(studentQuiz.AnalyzedVideos && studentQuiz.FoundAccuracy)
            {
                string queueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.ResultQueueName]);

                FindResultMessage findResultMessage = new FindResultMessage()
                {
                    MessageType = MessageTypeEnum.FIND_RESULT,
                    StudentQuizId = studentQuiz.Id
                };
                await _sqsManager.PublishToQueueAsync(queueUrl, JsonConvert.SerializeObject(findResultMessage));
            }
            await _context.SaveChangesAsync();
        }
    }
}
