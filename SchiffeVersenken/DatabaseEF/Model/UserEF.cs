using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchiffeVersenken.DatabaseEF.Models
{
	[Table("User")]
	public class UserEF
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; }

		[Required]
		public string? PasswordHash { get; set; }

		[Required]
		public string? Salt { get; set; }
	}
}
