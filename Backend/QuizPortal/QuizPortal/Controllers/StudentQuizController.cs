using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;
using System.Net;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize]
    [Route(APIRoutes.CONTROLLER)]
    public class StudentQuizController : Controller
    {
        private readonly IQuizManager _quizManager;
        private readonly IStudentCourseManager _studentCourseManager;
        private readonly IStudentQuizManager _studentQuizManager;

        public StudentQuizController(IStudentQuizManager studentQuizManager, IQuizManager quizManager, IStudentCourseManager studentCourseManager)
        {
            _quizManager = quizManager;
            _studentCourseManager = studentCourseManager;
            _studentQuizManager = studentQuizManager;
        }

        [HttpPost(APIRoutes.ADD_STUDENT_QUIZ)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> AddStudentQuiz(AddStudentQuizRequest addStudentQuizRequest)
        {
            Quiz quiz = await _quizManager.GetQuiz(addStudentQuizRequest.QuizId);
            if (quiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            StudentCourse studentCourse = await _studentCourseManager.GetById(addStudentQuizRequest.StudentCourseId);
            if (studentCourse == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentCourseNotFound
                };
            }

            StudentQuiz studentQuiz = new StudentQuiz()
            {
                Quiz = quiz,
                StudentCourse = studentCourse,
                StartDateTime = addStudentQuizRequest.StartDateTime,
                EndDateTime = null,
                AnalyzedVideos = false,
                FoundAccuracy = false
            };
            await _studentQuizManager.Add(studentQuiz);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.AddedStudentQuiz,
                Payload = new
                {
                    studentQuiz
                }
            };
        }

        [HttpGet(APIRoutes.GET_STUDENT_QUIZ_BY_STUDENT_COURSE_AND_QUIZ)]
        [Authorize(Roles = RoleConstants.STUDENT + "," + RoleConstants.PROFESSOR)]
        public async Task<Response> GetStudentQuizByStudentCourseAndQuiz(int studentCourseId, int quizId)
        {
            StudentQuiz? studentQuiz = await _studentQuizManager.GetStudentQuizByStudentCourseIdAndQuizId(studentCourseId, quizId);
            if (studentQuiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentQuizNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.StudentQuizFound,
                Payload = new
                {
                    studentQuiz
                }
            };
        }

        [HttpGet(APIRoutes.GET_STUDENT_QUIZ)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> GetStudentQuiz(int studentQuizId)
        {
            StudentQuiz? studentQuiz = await _studentQuizManager.GetStudentQuiz(studentQuizId);
            if(studentQuiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentQuizNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.StudentQuizFound,
                Payload = new
                {
                    studentQuiz
                }
            };
        }

        [HttpPut(APIRoutes.END_STUDENT_QUIZ)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> EndStudentQuiz(EndStudentQuizRequest endStudentQuizRequest)
        {
            StudentQuiz? studentQuiz = await _studentQuizManager.GetStudentQuiz(endStudentQuizRequest.StudentQuizId);
            if (studentQuiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentQuizNotFound
                };
            }

            studentQuiz.EndDateTime = endStudentQuizRequest.EndDateTime;
            await _studentQuizManager.EndStudentQuiz(studentQuiz);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.UpdatedStudentQuiz
            };
        }
    }
}
