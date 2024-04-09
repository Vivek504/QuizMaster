using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.DBModels;
using QuizPortal.IManagers;
using QuizPortal.RequestModels;
using QuizPortal.ResponseModels;
using QuizPortal.Utility;
using QuizPortal.SQSModels;
using Newtonsoft.Json;

namespace QuizPortal.Controllers
{
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IProfessorManager _professorManager;
        private readonly IStudentManager _studentManager;
        private readonly IUserCodeManager _userCodeManager;

        private readonly IMailManager _mailManager;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            IProfessorManager professorManager, IStudentManager studentManager, IUserCodeManager userCodeManager, IMailManager mailManager)
        {
            _configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;

            _professorManager = professorManager;
            _studentManager = studentManager;
            _userCodeManager = userCodeManager;

            _mailManager = mailManager;
        }

        [HttpPost(APIRoutes.REGISTER)]
        public async Task<Response> Registration(RegistrationRequest model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UserExists
                };
            }

            User user = new User()
            {
                UserName = model.Email,
                Email = model.Email
            };

            var createUserResult = await userManager.CreateAsync(user, model.Password);

            if (!createUserResult.Succeeded)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.RegistrationFailed
                };
            }

            bool doesRoleExist = await roleManager.RoleExistsAsync(model.Role);
            if (!doesRoleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            }

            await userManager.AddToRoleAsync(user, model.Role);

            if(model.Role == RoleConstants.PROFESSOR)
            {
                Professor professor = new Professor()
                {
                    User = user
                };
                await _professorManager.Add(professor);
            }
            else if(model.Role == RoleConstants.STUDENT)
            {
                Student student = new Student()
                {
                    User = user
                };
                await _studentManager.Add(student);
            }

            string token = await ValidateUserRoleAndGetToken(user);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.LoginSuccess,
                Payload = new
                {
                    Token = token
                }
            };
        }

        [HttpPost(APIRoutes.LOGIN)]
        public async Task<Response> Login(LoginRequest model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UserNotFound
                };
            }

            bool isUserValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!isUserValid)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.WrongCredentials
                };
            }

            string token = await ValidateUserRoleAndGetToken(user);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.LoginSuccess,
                Payload = new
                {
                    Token = token
                }
            };
        }

        private async Task<string> ValidateUserRoleAndGetToken(User user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = AuthUtility.GenerateToken(authClaims, _configuration[AppSettingsConstantsPath.jwtSecret], _configuration[AppSettingsConstantsPath.jwtTokenExpiryTimeInHour], _configuration[AppSettingsConstantsPath.jwtValidIssuer], _configuration[AppSettingsConstantsPath.jwtValidAudience]);
            return token;
        }

        [HttpPost(APIRoutes.FORGOT_PASSWORD)]
        public async Task<Response> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordRequest.Email);
            if (user == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UserNotFound
                };
            }

            UserCode userCode = new UserCode()
            {
                User = user,
                Code = Guid.NewGuid().ToString(),
                IsActive = true
            };
            await _userCodeManager.Add(userCode);

            string url = forgotPasswordRequest.ResetPasswordUrl + "/" + userCode.Code;
            await _mailManager.SendEmailToResetPassword(forgotPasswordRequest.Email, url);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.RequestForResetPassword
            };
        }

        [HttpPost(APIRoutes.RESET_PASSWORD)]
        public async Task<Response> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            UserCode userCode = await _userCodeManager.GetUserCodeByCode(resetPasswordRequest.Code);
            if(userCode == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.InvalidUserCode
                };
            }

            string resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(userCode.User);
            if(resetPasswordToken == null)
            {
                return new Response()
                {
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = ResponseMessage.UnableToResetPassword
                };
            }

            await userManager.ResetPasswordAsync(userCode.User, resetPasswordToken, resetPasswordRequest.Password);

            userCode.IsActive = false;
            await _userCodeManager.UpdateIsActive(userCode);

            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.ResetPasswordSuccess
            };
        }

        [HttpGet(APIRoutes.STATUS)]
        public async Task<Response> CheckStatus()
        {
            return new Response()
            {
                ResponseCode = HttpStatusCode.OK,
                Message = ResponseMessage.StatusSuccess
            };
        }
    }
}
