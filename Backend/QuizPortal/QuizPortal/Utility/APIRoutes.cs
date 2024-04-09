using System;
namespace QuizPortal.Utility
{
	public class APIRoutes
	{
		public const string CONTROLLER = "api/[controller]";

		public const string STATUS = "status";
		public const string REGISTER = "register";
		public const string LOGIN = "login";
		public const string FORGOT_PASSWORD = "forgotPassword";
		public const string RESET_PASSWORD = "resetPassword";

		public const string ADDCOURSE = "addCourse";
		public const string GETCOURSE = "getCourse";
		public const string UPDATECOURSE = "updateCourse";

		public const string GET_PROFESSOR_COURSES = "getProfessorCourses";

		public const string ADD_STUDENT = "addStudent";

		public const string GET_COURSE_STUDENTS = "getCourseStudents";
		public const string GET_STUDENT_COURSES = "getStudentCourses";
		public const string GET_STUDENT_COURSE = "getStudentCourse";

		public const string ADD_QUIZ = "addQuiz";
		public const string GET_COURSE_QUIZZES = "getCourseQuizzes";
		public const string GET_QUIZ = "getQuiz";
		public const string UPDATE_QUIZ = "updateQuiz";

        public const string ADD_QUESTION = "addQuestion";
        public const string GET_QUIZ_QUESTIONS = "getQuizQuestions";
		public const string GET_QUESTION = "getQuestion";
        public const string UPDATE_QUESTION = "updateQuestion";

		public const string ADD_STUDENT_QUIZ = "addStudentQuiz";
		public const string END_STUDENT_QUIZ = "endStudentQuiz";
		public const string GET_STUDENT_QUIZ = "getStudentQuiz";
		public const string GET_STUDENT_QUIZ_BY_STUDENT_COURSE_AND_QUIZ = "getStudentQuizByStudentCourseAndQuiz";

		public const string ADD_VIDEO = "addVideo";
		public const string GET_VIDEOS_BY_STUDENT_QUIZ_ID = "getVideosByStudentQuizId";

		public const string ADD_ANSWER = "addAnswer";

		public const string GET_STUDENT_RESULT = "getStudentResult";
    }
}
