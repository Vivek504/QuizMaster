using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class CourseManager: ICourseManager
	{
		private readonly AppDBContext _context;

		public CourseManager(AppDBContext context)
		{
			_context = context;
		}

        public async Task Add(Course course)
        {
			await _context.Courses.AddAsync(course);
			await _context.SaveChangesAsync();
        }

        public async Task<Course> GetByCourseName(string courseName)
        {
			Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseName == courseName);
			return course;
        }

        public async Task<Course> GetCourse(int courseId)
        {
            Course? course = await _context.Courses.FindAsync(courseId);
            return course;
        }

        public async Task<Course> UpdateCourse(int courseId, Course updatedCourse)
        {
            Course? course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return null;
            }

            course.CourseName = updatedCourse.CourseName;
            await _context.SaveChangesAsync();

            return course;
        }
    }
}
