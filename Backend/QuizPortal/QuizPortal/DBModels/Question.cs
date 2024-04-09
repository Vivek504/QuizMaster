using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.DBModels
{
	public class Question
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        [Required]
        public string QuestionText { get; set; }
    }
}

