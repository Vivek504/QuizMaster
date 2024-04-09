using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.DBModels
{
	public class Answer
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public StudentQuiz StudentQuiz { get; set; }

        [Required]
        public Question Question { get; set; }

        [Required]
        public string AnswerText { get; set; }

        [Required]
        public bool IsLastAnswer { get; set; }
    }
}
