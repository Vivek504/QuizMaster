import './App.css';
import { Routes, Route } from 'react-router-dom';
import ProfessorLogin from './components/Authentication/Professor/ProfessorLogin';
import StudentLogin from './components/Authentication/Student/StudentLogin';
import ProfessorRegistration from './components/Authentication/Professor/ProfessorRegistration';
import CourseIndex from './components/Courses/CourseIndex';
import AddCourse from './components/Courses/AddCourse/AddCourse';
import Authentication from './authUtility/Authentication';
import Authorization from './authUtility/Authorization';
import ROLES from './constants/roles';
import UpdateCourse from './components/Courses/UpdateCourse/UpdateCourse';
import StudentIndex from './components/Students/StudentIndex';
import AddStudent from './components/Students/AddStudent/AddStudent';
import StudentCourseIndex from './components/StudentCourses/StudentCoursesIndex';
import CourseQuizzesIndex from './components/CourseQuizzes/CourseQuizzesIndex';
import AddCourseQuiz from './components/CourseQuizzes/AddCourseQuiz/AddCourseQuiz';
import UpdateCourseQuiz from './components/CourseQuizzes/UpdateCourseQuiz/UpdateCourseQuiz';
import QuizQuestionsIndex from './components/QuizQuestions/QuizQuestionsIndex';
import AddQuizQuestion from './components/QuizQuestions/AddQuizQuestion/AddQuizQuestion';
import UpdateQuizQuestion from './components/QuizQuestions/UpdateQuizQuestion/UpdateQuizQuestion';
import ForgotPassword from './components/Authentication/ForgotPassword/ForgotPassword';
import ResetPassword from './components/Authentication/ResetPassword/ResetPassword';
import StudentQuizIndex from './components/StudentQuiz/StudentQuizIndex';


function App() {
  return (
    <Routes>
      <Route path="/" element={<StudentLogin />} />
      <Route path="/student/login" element={<StudentLogin />} />
      <Route path="/professor/login" element={<ProfessorLogin />} />
      <Route path="/professor/register" element={<ProfessorRegistration />} />
      <Route path="/forgot-password" element={<ForgotPassword />} />
      <Route path="/reset-password/:code" element={<ResetPassword />} />
      <Route path="/courses" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <CourseIndex /> </Authorization> </Authentication>} />
      <Route path="/student-courses" element={<Authentication> <Authorization allowedRoles={[ROLES.STUDENT]}> <StudentCourseIndex /> </Authorization> </Authentication>} />
      <Route path="/add-course" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <AddCourse /> </Authorization> </Authentication>} />
      <Route path="/update-course/:id" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <UpdateCourse /> </Authorization> </Authentication>} />
      <Route path="/course/:id/students" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <StudentIndex /> </Authorization> </Authentication>} />
      <Route path="/course/:id/add-student" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <AddStudent /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/quizzes" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR, ROLES.STUDENT]}> <CourseQuizzesIndex /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/add-quiz" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <AddCourseQuiz /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/update-quiz/:quizId" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <UpdateCourseQuiz /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/quiz/:quizId/questions" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <QuizQuestionsIndex /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/quiz/:quizId/add-question" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <AddQuizQuestion /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/quiz/:quizId/update-question/:questionId" element={<Authentication> <Authorization allowedRoles={[ROLES.PROFESSOR]}> <UpdateQuizQuestion /> </Authorization> </Authentication>} />
      <Route path="/course/:courseId/student-quiz/:studentQuizId" element={<Authentication> <Authorization allowedRoles={[ROLES.STUDENT]}> <StudentQuizIndex /> </Authorization> </Authentication>} />
    </Routes>
  );
}

export default App;
