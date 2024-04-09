using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class ProfessorCourseManager: IProfessorCourseManager
	{
		private readonly AppDBContext _context;

		public ProfessorCourseManager(AppDBContext context)
		{
			_context = context;
		}

        public async Task Add(ProfessorCourse professorCourse)
        {
			await _context.ProfessorCourses.AddAsync(professorCourse);
			await _context.SaveChangesAsync();
        }

        public async Task<ProfessorCourse> GetByProfessorIdAndCourseId(int professorId, int courseId)
        {
			ProfessorCourse? professorCourse = await _context.ProfessorCourses.FirstOrDefaultAsync(pc => pc.Professor.Id == professorId && pc.Course.Id == courseId);
			return professorCourse;
        }

        public async Task<List<ProfessorCourse>> GetProfessorCourses(int professorId)
        {
            List<ProfessorCourse> professorCourses = await _context.ProfessorCourses
                .Include(pc => pc.Course)
                .Where(pc => pc.Professor.Id == professorId)
                .ToListAsync();
            return professorCourses;
        }
    }
}
