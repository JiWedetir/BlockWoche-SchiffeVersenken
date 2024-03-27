using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiffeVersenken.DatabaseEF.Models
{
	[Table("HighScores")]
	public class HighScoreEF
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int Score { get; set; }

		[Required]
		public string? Opponent { get; set; }

		[Required]
		public bool Won { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }

		public UserEF? User { get; set; }
	}
}
