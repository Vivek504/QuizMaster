using System;
using Microsoft.EntityFrameworkCore;
using QuizPortal.DBModels;
using QuizPortal.IManagers;

namespace QuizPortal.Managers
{
	public class StudentManager: IStudentManager
	{
        private readonly AppDBContext _context;

        public StudentManager(AppDBContext context)
        {
            _context = context;
        }

        public async Task Add(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<Student> GetStudent(string userId)
        {
            Student? student = await _context.Students.FirstOrDefaultAsync(s => s.User.Id == userId);
            return student;
        }

        public async Task<Student> GetStudentByEmail(string Email)
        {
            Student? student = await _context.Students.FirstOrDefaultAsync(s => s.User.Email == Email);
            return student;
        }
    }
}
