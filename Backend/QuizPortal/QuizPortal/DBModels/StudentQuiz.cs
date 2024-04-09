using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.DBModels
{
	public class StudentQuiz
	{
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        [Required]
        public StudentCourse StudentCourse { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public bool AnalyzedVideos { get; set; }

        public bool FoundAccuracy { get; set; }
    }
}

