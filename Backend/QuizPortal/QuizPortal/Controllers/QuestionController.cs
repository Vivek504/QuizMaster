using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    public class QuestionController : ControllerBase
    {
        private readonly IQuizManager _quizManager;
        private readonly IQuestionManager _questionManager;

        public QuestionController(IQuestionManager questionManager, IQuizManager quizManager)
        {
            _quizManager = quizManager;
            _questionManager = questionManager;
        }

        [HttpGet(APIRoutes.GET_QUESTION)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> GetQuestion(int questionId)
        {
            Question? question = await _questionManager.GetQuestion(questionId);

            if (question == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.QuestionFound,
                Payload = new
                {
                    question
                }
            };
        }

        [HttpGet(APIRoutes.GET_QUIZ_QUESTIONS)]
        [Authorize(Roles = RoleConstants.PROFESSOR + "," + RoleConstants.STUDENT)]
        public async Task<Response> GetQuizQuestions(int quizId)
        {
            Quiz? quiz = await _quizManager.GetQuiz(quizId);

            if (quiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            List<Question> questions = await _questionManager.GetQuizQuestions(quizId);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.QuizQuestionsFound,
                Payload = new
                {
                    questions
                }
            };
        }

        [HttpPost(APIRoutes.ADD_QUESTION)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> AddQuestion(AddQuestionRequest addQuestionRequest)
        {
            Quiz? quiz = await _quizManager.GetQuiz(addQuestionRequest.QuizId);

            if(quiz == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuizNotFound
                };
            }

            Question question = new Question()
            {
                Quiz = quiz,
                QuestionText = addQuestionRequest.QuestionText
            };

            await _questionManager.Add(question);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.QuestionAdded
            };
        }

        [HttpPut(APIRoutes.UPDATE_QUESTION)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> UpdateQuestion(UpdateQuestionRequest updateQuestionRequest)
        {
            Question? question = await _questionManager.GetQuestion(updateQuestionRequest.Id);
            if(question == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.QuestionNotFound
                };
            }

            question.QuestionText = updateQuestionRequest.QuestionText;

            await _questionManager.UpdateQuestion(question);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.QuestionUpdated
            };
        }
    }
}
