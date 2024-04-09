using System;
namespace QuizPortal.Utility
{
	public class ResponseMessage
	{
		public const string StatusSuccess = "Success.";

		public const string UserExists = "User already exists.";
		public const string UserNotFound = "User doesn't exist.";
		public const string RegistrationFailed = "Registration is failed.";
		public const string RegistrationSuccess = "Registration is successful.";
		public const string LoginSuccess = "Login is successful.";
		public const string WrongCredentials = "User has entered wrong credentials";
		public const string RequestForResetPassword = "Please check your email to reset password.";
		public const string InvalidUserCode = "User code is invalid.";
		public const string UnableToResetPassword = "Unable to reset the password.";
		public const string ResetPasswordSuccess = "Reset password is successful.";

        public const string ProfessorNotFound = "Professor doesn't exist.";

        public const string CourseAdded = "Course is added successfully.";
		public const string CourseNotFound = "Course not found.";
        public const string CourseFound = "Course is found successfully.";
		public const string CourseAlreadyExists = "Course already exists.";
		public const string CourseUpdated = "Course is updated successfully.";

        public const string ProfessorCourseExists = "Course assigned to professor already exists.";
		public const string GetProfessorCoursesSuccess = "Professor courses are returned successfully.";

		public const string StudentAdded = "Student is added successfully.";
		public const string StudentNotFound = "Student not found.";

		public const string GetStudentCoursesSuccess = "Student courses are returned successfully.";
		public const string GetStudentCourseSuccess = "Student course is returned successfully.";
        public const string StudentCourseAlreadyExists = "Student assigned to course already exists.";
		public const string StudentCourseNotFound = "Student course is not found.";

		public const string QuizAdded = "Quiz is added successfully.";
        public const string InvalidQuizType = "Quiz type is invalid.";
		public const string QuizFound = "Quiz is found successfully.";
        public const string QuizNotFound = "Quiz not found.";
		public const string QuizUpdated = "Quiz is updated successfully.";
		public const string CourseQuizzesFound = "Course quizzes are found successfully.";

        public const string QuestionAdded = "Question is added successfully.";
		public const string QuestionFound = "Question is found successfully.";
		public const string QuestionNotFound = "Question not found.";
		public const string QuestionUpdated = "Question is updated successfully.";
		public const string QuizQuestionsFound = "Quiz questions are found successfully.";

		public const string AddedStudentQuiz = "Student quiz is added successfully.";
		public const string UpdatedStudentQuiz = "Student quiz is updated successfully.";
        public const string StudentQuizNotFound = "Student quiz not found.";
		public const string StudentQuizFound = "Student quiz is found successfully.";

        public const string AddedStudentResult = "Student result is added successfully.";
		public const string StudentResultNotFound = "Student result is not found.";
        public const string StudentResultFound = "Student result is found successfully.";

        public const string AddedVideo = "Video is added successfully.";
		public const string VideosNotFound = "Videos not found.";
		public const string VideosFound = "Videos are found successfully.";

		public const string AddedAnswer = "Answer is added successfully.";
    }
}
