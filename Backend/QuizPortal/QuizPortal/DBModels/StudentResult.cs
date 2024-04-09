using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.DBModels
{
	public class StudentResult
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public StudentQuiz StudentQuiz { get; set; }

        [Required]
        public bool IsCheatingFound { get; set; }

        public double? Score { get; set; }
    }
}
