using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuizPortal.DBModels
{
	public class AppDBContext: IdentityDbContext<User>
	{
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Professor> Professors { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<ProfessorCourse> ProfessorCourses { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<UserCode> UserCodes { get; set; }

        public DbSet<StudentQuiz> StudentQuizzes { get; set; }

        public DbSet<StudentResult> StudentResults { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Accuracy> Accuracies { get; set; }
    }
}
