using System;
using QuizPortal.DBModels;

namespace QuizPortal.IManagers
{
	public interface IVideoManager
	{
		Task AddVideo(Video video);

		Task<Video> GetVideo(int id);

		Task UpdateVideo(Video video);

		Task<List<Video>> GetVideosByStudentQuizId(int studentQuizId);
	}
}
