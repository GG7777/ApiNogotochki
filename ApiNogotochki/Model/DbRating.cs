using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("ratings")]
	public class DbRating
	{
		[Key]
		[Column("user_id")]
		public string UserId { get; set; }
		
		[Key]
		[Column("target_id")]
		public string TargetId { get; set; }
		
		[Key]
		[Column("target_type")]
		public string TargetType { get; set; }
		
		[Column("value")]
		public int Value { get; set; }
	}
}