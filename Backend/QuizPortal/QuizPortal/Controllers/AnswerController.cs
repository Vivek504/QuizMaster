using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.Managers;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize]
    [Route(APIRoutes.CONTROLLER)]
    public class AnswerController : Controller
    {
        private readonly IAnswerManager _answerManager;
        private readonly IStudentQuizManager _studentQuizManager;
        private readonly IQuestionManager _questionManager;

        public AnswerController(IAnswerManager answerManager, IStudentQuizManager studentQuizManager, IQuestionManager questionManager)
        {
            _answerManager = answerManager;
            _studentQuizManager = studentQuizManager;
            _questionManager = questionManager;
        }

        [HttpPost(APIRoutes.ADD_ANSWER)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> AddAnswer(AddAnswerRequest addAnswerRequest)
        {
            StudentQuiz studentQuiz = await _studentQuizManager.GetStudentQuiz(addAnswerRequest.StudentQuizId);
            if (studentQuiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentQuizNotFound
                };
            }

            Question? question = await _questionManager.GetQuestion(addAnswerRequest.QuestionId);
            if (question == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuestionNotFound
                };
            }

            Answer answer = new Answer
            {
                StudentQuiz = studentQuiz,
                Question = question,
                AnswerText = addAnswerRequest.AnswerText,
                IsLastAnswer = addAnswerRequest.IsLastAnswer
            };

            await _answerManager.Add(answer);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.AddedAnswer
            };
        }
    }
}

