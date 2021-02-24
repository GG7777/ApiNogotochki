using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("dialogs")]
	public class DbDialog
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		
		[Column("user_id")]
		public string UserId { get; set; }
		
		[Column("service_id")]
		public string ServiceId { get; set; }
	}
}