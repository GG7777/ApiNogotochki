using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("services")]
	public class DbService
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }

		[Column("type")]
		public string Type { get; set; }
		
		[Column("is_removed")]
		public bool IsRemoved { get; set; }
		
		[Column("content")]
		public string Content { get; set; }
	}
}