using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("photos")]
	public class DbPhoto
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }

		[Key]
		[Column("size")]
		public string Size { get; set; }
		
		[Column("path")]
		public string Path { get; set; }
	}
}