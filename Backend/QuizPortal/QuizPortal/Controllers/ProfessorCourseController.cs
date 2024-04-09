using Microsoft.AspNetCore.Mvc;
using QuizPortal.Utility;
using QuizPortal.IManagers;
using QuizPortal.DBModels;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize(Roles = RoleConstants.PROFESSOR)]
    [Route(APIRoutes.CONTROLLER)]
    public class ProfessorCourseController : ControllerBase
    {
        private readonly IProfessorCourseManager _professorCourseManager;
        private readonly ICourseManager _courseManager;
        private readonly IProfessorManager _professorManager;

        public ProfessorCourseController(IProfessorCourseManager professorCourseManager, ICourseManager courseManager, IProfessorManager professorManager)
        {
            _professorCourseManager = professorCourseManager;
            _courseManager = courseManager;
            _professorManager = professorManager;
        }

        [HttpGet(APIRoutes.GET_PROFESSOR_COURSES)]
        public async Task<Response> GetProfessorCourses()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UserNotFound
                };
            }

            Professor professor = await _professorManager.GetByUserId(userId);

            if (professor == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.ProfessorNotFound
                };
            }

            List<ProfessorCourse> professorCourses = await _professorCourseManager.GetProfessorCourses(professor.Id);

            List<Course> courses = new List<Course>();
            foreach (var item in professorCourses)
            {
                courses.Add(item.Course);
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.GetProfessorCoursesSuccess,
                Payload = new
                {
                    courses
                }
            };
        }
    }
}

