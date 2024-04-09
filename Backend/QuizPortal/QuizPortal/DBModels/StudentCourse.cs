using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.DBModels
{
	public class StudentCourse
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Student Student { get; set; }

        [Required]
        public Course Course { get; set; }
    }
}
