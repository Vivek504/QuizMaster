using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.Utility;
using QuizPortal.IManagers;
using QuizPortal.DBModels;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace QuizPortal.Controllers
{
    [ApiController]
    [Route(APIRoutes.CONTROLLER)]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentManager _studentManager;
        private readonly UserManager<User> _userManager;
        private readonly ICourseManager _courseManager;
        private readonly IStudentCourseManager _studentCourseManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailManager _mailManager;

        public StudentController(IStudentManager studentManager, UserManager<User> userManager, ICourseManager courseManager,
            IStudentCourseManager studentCourseManager, RoleManager<IdentityRole> roleManager, IMailManager mailManager)
        {
            _studentManager = studentManager;
            _userManager = userManager;
            _courseManager = courseManager;
            _studentCourseManager = studentCourseManager;
            _roleManager = roleManager;
            _mailManager = mailManager;
        }

        [HttpPost]
        [Route(APIRoutes.ADD_STUDENT)]
        [Authorize(Roles = RoleConstants.PROFESSOR)]
        public async Task<Response> AddStudents(StudentRegistrationRequest studentRegistrationRequest)
        {
            Course course = await _courseManager.GetCourse(studentRegistrationRequest.CourseId);

            if (course == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CourseNotFound
                };
            }

            string Email = studentRegistrationRequest.Email;

            User? user = await _userManager.FindByEmailAsync(Email);
            Student student;

            if (user == null)
            {
                user = new User()
                {
                    UserName = Email,
                    Email = Email
                };

                string password = AuthUtility.GenerateRandomPassword();

                var userResponse = await _userManager.CreateAsync(user, password);

                if (userResponse.Succeeded)
                {
                    bool doesRoleExist = await _roleManager.RoleExistsAsync(RoleConstants.STUDENT);
                    if (!doesRoleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(RoleConstants.STUDENT));
                    }

                    await _userManager.AddToRoleAsync(user, RoleConstants.STUDENT);
                }

                student = new Student()
                {
                    User = user
                };

                await _studentManager.Add(student);

                _mailManager.SendEmailForAccountCreation(user.Email, password);
            }

            student = await _studentManager.GetStudentByEmail(user.Email);

            StudentCourse? studentCourse = await _studentCourseManager.GetByStudentAndCourseId(student.Id, course.Id);

            if(studentCourse != null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.StudentCourseAlreadyExists
                };
            }

            studentCourse = new StudentCourse()
            {
                Student = student,
                Course = course
            };

            await _studentCourseManager.Add(studentCourse);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.StudentAdded
            };
        }
    }
}
