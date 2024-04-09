using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.Utility;
using QuizPortal.IManagers;
using QuizPortal.DBModels;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using System.Net;
using System.Security.Claims;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Authorize]
    [Route(APIRoutes.CONTROLLER)]
    public class StudentCourseController : Controller
    {
        private readonly IStudentCourseManager _studentCourseManager;
        private readonly ICourseManager _courseManager;
        private readonly IStudentManager _studentManager;

        public StudentCourseController(IStudentCourseManager studentCourseManager, ICourseManager courseManager, IStudentManager studentManager)
        {
            _studentCourseManager = studentCourseManager;
            _courseManager = courseManager;
            _studentManager = studentManager;
        }

        [HttpGet(APIRoutes.GET_COURSE_STUDENTS)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> GetCourseStudents(int courseId)
        {
            Course? course = await _courseManager.GetCourse(courseId);
            if(course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            List<StudentCourse> courseStudents = await _studentCourseManager.GetCourseStudents(courseId);

            List<object> students = new List<object>();
            foreach (StudentCourse item in courseStudents)
            {
                students.Add(new
                {
                    item.Student.Id,
                    item.Student.User.Email
                });
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.GetStudentCoursesSuccess,
                Payload = new
                {
                    students
                }
            };
        }

        [HttpGet(APIRoutes.GET_STUDENT_COURSE)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> GetStudentCourse(int studentId, int courseId)
        {
            StudentCourse? studentCourse = await _studentCourseManager.GetByStudentAndCourseId(studentId, courseId);
            if(studentCourse == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentCourseNotFound
                };
            }

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.GetStudentCourseSuccess,
                Payload = new
                {
                    studentCourse
                }
            };
        }

        [HttpGet(APIRoutes.GET_STUDENT_COURSES)]
        [Authorize(Roles = RoleConstants.STUDENT)]
        public async Task<Response> GetStudentCourses()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UserNotFound
                };
            }

            Student student = await _studentManager.GetStudent(userId);

            if(student == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentNotFound
                };
            }

            List<StudentCourse> studentCourses = await _studentCourseManager.GetStudentCourses(student.Id);

            List<Course> courses = new List<Course>();
            foreach (StudentCourse item in studentCourses)
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
