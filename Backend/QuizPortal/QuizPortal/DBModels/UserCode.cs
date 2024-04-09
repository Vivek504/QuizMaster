using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuizPortal.DBModels;

namespace QuizPortal.DBModels
{
	public class UserCode
	{
		[Key]
		[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[Required]
		public User User { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		public bool IsActive { get; set; }
    }
}
