using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("social_networks")]
	public class DbSocialNetwork
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }

		[Column("type")]
		public string Type { get; set; }
		
		[Column("mention")]
		public string Mention { get; set; }
	}
}