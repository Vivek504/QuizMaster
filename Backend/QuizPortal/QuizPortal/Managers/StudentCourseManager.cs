using System;
using QuizPortal.IManagers;
using QuizPortal.DBModels;
using Microsoft.EntityFrameworkCore;

namespace QuizPortal.Managers
{
	public class StudentCourseManager: IStudentCourseManager
	{
		private readonly AppDBContext _context;

		public StudentCourseManager(AppDBContext context)
		{
			_context = context;
		}

		public async Task Add(StudentCourse studentCourse)
		{
			await _context.StudentCourses.AddAsync(studentCourse);
			await _context.SaveChangesAsync();
		}

        public async Task<StudentCourse> GetById(int studentCourseId)
        {
			return await _context.StudentCourses
				.Include(sc => sc.Student)
				.Include(sc => sc.Course)
				.Where(sc => sc.Id == studentCourseId)
				.FirstOrDefaultAsync();
        }

        public async Task<StudentCourse> GetByStudentAndCourseId(int studentId, int courseId)
        {
			StudentCourse? studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(sc => sc.Student.Id == studentId && sc.Course.Id == courseId);
			return studentCourse;
        }

        public async Task<List<StudentCourse>> GetCourseStudents(int courseId)
        {
			List<StudentCourse> courseStudents = await _context.StudentCourses
				.Include(sc => sc.Student)
				.Include(sc => sc.Student.User)
				.Where(sc => sc.Course.Id == courseId)
				.ToListAsync();

			return courseStudents;
        }

        public async Task<List<StudentCourse>> GetStudentCourses(int studentId)
        {
			List<StudentCourse> studentCourses = await _context.StudentCourses
				.Include(sc => sc.Course)
				.Where(sc => sc.Student.Id == studentId)
				.ToListAsync();

			return studentCourses;
        }
    }
}

