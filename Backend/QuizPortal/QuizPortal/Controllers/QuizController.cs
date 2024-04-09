using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.IManagers;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;
using QuizPortal.RequestModels;
using QuizPortal.DBModels;
using System.Net;

namespace QuizPortal.Controllers
{
    [Authorize]
    [ApiController]
    [Route(APIRoutes.CONTROLLER)]
    public class QuizController : ControllerBase
    {
        private readonly IQuizManager _quizManager;
        private readonly ICourseManager _courseManager;

        public QuizController(IQuizManager quizManager, ICourseManager courseManager)
        {
            _quizManager = quizManager;
            _courseManager = courseManager;
        }

        [HttpGet(APIRoutes.GET_QUIZ)]
        [Authorize(Roles = RoleConstants.PROFESSOR + "," + RoleConstants.STUDENT)]
        public async Task<Response> GetQuiz(int quizId)
        {
            Quiz? quizResponse = await _quizManager.GetQuiz(quizId);

            if (quizResponse == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            object? quiz = null;

            if (Enum.IsDefined(typeof(QuizTypeEnum), quizResponse.QuizType))
            {
                QuizTypeEnum parsedQuizType = quizResponse.QuizType;
                string quizType = Enum.GetName(typeof(QuizTypeEnum), parsedQuizType);

                if (quizType != null)
                {
                    quizType = quizType.ToLower();
                    quizType = char.ToUpper(quizType[0]) + quizType.Substring(1);

                    quiz = new
                    {
                        quizResponse.Id,
                        quizResponse.QuizTitle,
                        quizResponse.StartDateTime,
                        quizResponse.EndDateTime,
                        quizType
                    };
                }
            }
            else
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.InvalidQuizType
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.QuizFound,
                Payload = new
                {
                    quiz
                }
            };
        }

        [HttpGet(APIRoutes.GET_COURSE_QUIZZES)]
        [Authorize(Roles = RoleConstants.PROFESSOR + "," + RoleConstants.STUDENT)]
        public async Task<Response> GetCourseQuizzes(int courseId)
        {
            Course? course = await _courseManager.GetCourse(courseId);

            if (course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            List<Quiz> quizzesResponse = await _quizManager.GetQuizzesByCourseId(course.Id);

            List<object> quizzes = new List<object>();
            foreach (Quiz item in quizzesResponse)
            {
                if (Enum.IsDefined(typeof(QuizTypeEnum), item.QuizType))
                {
                    QuizTypeEnum parsedQuizType = item.QuizType;
                    string quizType = Enum.GetName(typeof(QuizTypeEnum), parsedQuizType);

                    if (quizType != null)
                    {
                        quizType = quizType.ToLower();
                        quizType = char.ToUpper(quizType[0]) + quizType.Substring(1);

                        quizzes.Add(new
                        {
                            item.Id,
                            item.QuizTitle,
                            item.StartDateTime,
                            item.EndDateTime,
                            quizType
                        });
                    }

                }
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.CourseQuizzesFound,
                Payload = new
                {
                    quizzes
                }
            };
        }

        [HttpPost(APIRoutes.ADD_QUIZ)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> AddQuiz(AddQuizRequest addQuizRequest)
        {
            Course? course = await _courseManager.GetCourse(addQuizRequest.CourseId);

            if (course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            QuizTypeEnum parsedQuizType;
            if (Enum.TryParse(addQuizRequest.QuizType, out parsedQuizType))
            {
                Quiz quiz = new Quiz()
                {
                    Course = course,
                    QuizTitle = addQuizRequest.QuizTitle,
                    StartDateTime = addQuizRequest.StartDateTime,
                    EndDateTime = addQuizRequest.EndDateTime,
                    QuizType = parsedQuizType
                };

                await _quizManager.Add(quiz);

                return new Response()
                {
                    ResponseCode = HttpStatusCode.OK,
                    Message = ResponseMessage.QuizAdded
                };
            }
            else
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.InvalidQuizType
                };
            }
        }

        [HttpPut(APIRoutes.UPDATE_QUIZ)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> UpdateQuiz(UpdateQuizRequest updateQuizRequest)
        {
            Quiz? quiz = await _quizManager.GetQuiz(updateQuizRequest.Id);

            if (quiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            QuizTypeEnum parsedQuizType;
            if (Enum.TryParse(updateQuizRequest.QuizType, out parsedQuizType))
            {
                quiz.QuizTitle = updateQuizRequest.QuizTitle;
                quiz.StartDateTime = updateQuizRequest.StartDateTime;
                quiz.EndDateTime = updateQuizRequest.EndDateTime;
                quiz.QuizType = parsedQuizType;

                await _quizManager.UpdateQuiz(quiz);

                return new Response()
                {
                    ResponseCode = HttpStatusCode.OK,
                    Message = ResponseMessage.QuizUpdated
                };
            }
            else
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.InvalidQuizType
                };
            }
        }
    }
}
