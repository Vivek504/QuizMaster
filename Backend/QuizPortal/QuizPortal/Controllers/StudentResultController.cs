using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize]
    [Route(APIRoutes.CONTROLLER)]
    public class StudentResultController : Controller
    {
        private readonly IStudentResultManager _studentResultManager;

        public StudentResultController(IStudentResultManager studentResultManager)
        {
            _studentResultManager = studentResultManager;
        }

        [HttpGet(APIRoutes.GET_STUDENT_RESULT)]
        [Authorize(Roles = RoleConstants.STUDENT + "," + RoleConstants.PROFESSOR)]
        public async Task<Response> GetStudentResult(int studentQuizId)
        {
            StudentResult? studentResult = await _studentResultManager.GetByStudentQuizId(studentQuizId);
            if(studentResult == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentResultNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.StudentResultFound,
                Payload = new
                {
                    studentResult
                }
            };
        }
    }
}

