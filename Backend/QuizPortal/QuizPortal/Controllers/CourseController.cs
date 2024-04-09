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
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseManager _courseManager;
        private readonly IProfessorManager _professorManager;
        private readonly IProfessorCourseManager _professorCourseManager;

        public CourseController(ICourseManager courseManager, IProfessorManager professorManager, IProfessorCourseManager professorCourseManager)
        {
            _courseManager = courseManager;
            _professorManager = professorManager;
            _professorCourseManager = professorCourseManager;
        }

        [HttpGet(APIRoutes.GETCOURSE)]
        public async Task<Response> GetCourse(int Id)
        {
            Course course = await _courseManager.GetCourse(Id);

            if(course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.CourseFound,
                Payload = new
                {
                    course
                }
            };
        }


        [Authorize(Roles = RoleConstants.PROFESSOR)]
        [HttpPost(APIRoutes.ADDCOURSE)]
        public async Task<Response> AddCourse(AddCourseRequest courseRequest)
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

            Course course = await _courseManager.GetByCourseName(courseRequest.CourseName);
            if (course == null)
            {
                course = new Course()
                {
                    CourseName = courseRequest.CourseName
                };
            }
            else
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseAlreadyExists
                };
            }

            await _courseManager.Add(course);

            ProfessorCourse professorCourse = await _professorCourseManager.GetByProfessorIdAndCourseId(professor.Id, course.Id);
            if(professorCourse != null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.ProfessorCourseExists
                };
            }

            professorCourse = new ProfessorCourse()
            {
                Professor = professor,
                Course = course
            };

            await _professorCourseManager.Add(professorCourse);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.CourseAdded
            };
        }

        [Authorize(Roles = RoleConstants.PROFESSOR)]
        [HttpPut(APIRoutes.UPDATECOURSE)]
        public async Task<Response> UpdateCourse(UpdateCourseRequest updateCourseRequest)
        {
            Course? course = await _courseManager.GetByCourseName(updateCourseRequest.courseName);
            if(course != null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseAlreadyExists
                };
            }

            course = await _courseManager.GetCourse(updateCourseRequest.Id);
            if(course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            course.CourseName = updateCourseRequest.courseName;

            Course? updatedCourse = await _courseManager.UpdateCourse(course.Id, course);

            if (updatedCourse == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.CourseUpdated,
                Payload = new
                {
                    course = updatedCourse
                }
            };
        }
    }
}
