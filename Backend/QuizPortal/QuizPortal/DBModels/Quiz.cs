using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizPortal.Utility;

namespace QuizPortal.DBModels
{
	public class Quiz
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Course Course { get; set; }

        [Required]
        public string QuizTitle { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public QuizTypeEnum QuizType { get; set; }
    }
}
