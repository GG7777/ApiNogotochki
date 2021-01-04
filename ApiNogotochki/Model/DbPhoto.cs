using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("photos")]
	public class DbPhoto
	{
		[Column("id")]
		public string Id { get; set; }
		
		[Column("size")]
		public string Size { get; set; }
		
		[Column("path")]
		public string Path { get; set; }
	}
}