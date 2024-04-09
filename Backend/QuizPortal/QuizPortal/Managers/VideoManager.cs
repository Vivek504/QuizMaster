using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.SQSModels;
using QuizPortal.Utility;

namespace QuizPortal.Managers
{
	public class VideoManager: IVideoManager
	{
		private readonly IConfiguration _configuration;
		private readonly AppDBContext _context;
		private readonly ISQSManager _sqsManager;

		public VideoManager(IConfiguration configuration, AppDBContext context, ISQSManager sqsManager)
		{
			_configuration = configuration;
			_context = context;
			_sqsManager = sqsManager;
		}

        public async Task AddVideo(Video video)
        {
			await _context.Videos.AddAsync(video);
			await _context.SaveChangesAsync();

			string queueUrl = await _sqsManager.GetQueueUrlAsync(_configuration[AppSettingsConstantsPath.faceAnalysisQueueName]);

			FaceAnalysisMessage faceAnalysisMessage = new FaceAnalysisMessage()
			{
				MessageType = MessageTypeEnum.FACE_ANALYSIS,
				VideoId = video.Id
			};

			await _sqsManager.PublishToQueueAsync(queueUrl, JsonConvert.SerializeObject(faceAnalysisMessage));
        }

        public async Task<Video> GetVideo(int id)
        {
			return await _context.Videos
				.Include(v => v.StudentQuiz)
				.Include(v => v.StudentQuiz.Quiz)
				.Include(v => v.Question)
				.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Video>> GetVideosByStudentQuizId(int studentQuizId)
        {
			return await _context.Videos
				.Include(v => v.StudentQuiz)
				.Include(v => v.Question)
				.Where(v => v.StudentQuiz.Id == studentQuizId)
				.ToListAsync();
        }

        public async Task UpdateVideo(Video video)
        {
			await _context.SaveChangesAsync();
        }
    }
}
