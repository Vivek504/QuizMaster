using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize]
    [Route(APIRoutes.CONTROLLER)]
    public class VideoController : Controller
    {
        private readonly IVideoManager _videoManager;
        private readonly IStudentQuizManager _studentQuizManager;
        private readonly IQuestionManager _questionManager;

        public VideoController(IVideoManager videoManager, IStudentQuizManager studentQuizManager, IQuestionManager questionManager)
        {
            _videoManager = videoManager;
            _studentQuizManager = studentQuizManager;
            _questionManager = questionManager;
        }

        [HttpPost(APIRoutes.ADD_VIDEO)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> AddVideo(AddVideoRequest addVideoRequest)
        {
            StudentQuiz studentQuiz = await _studentQuizManager.GetStudentQuiz(addVideoRequest.StudentQuizId);
            if (studentQuiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentQuizNotFound
                };
            }

            Question? question = await _questionManager.GetQuestion(addVideoRequest.QuestionId);
            if (question == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuestionNotFound
                };
            }

            Video video = new Video()
            {
                StudentQuiz = studentQuiz,
                Question = question,
                S3ObjectName = addVideoRequest.S3ObjectName,
                IsAnalysisCompleted = false,
                IsLastVideo = addVideoRequest.IsLastVideo
            };
            await _videoManager.AddVideo(video);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.AddedVideo
            };
        }

        [HttpGet(APIRoutes.GET_VIDEOS_BY_STUDENT_QUIZ_ID)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> GetVidesByStudentQuizId(int studentQuizId)
        {
            List<Video> videos = await _videoManager.GetVideosByStudentQuizId(studentQuizId);
            if(videos == null)
            {
                videos = new List<Video>();
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.VideosFound,
                Payload = new
                {
                    videos
                }
            };
        }
    }
}
