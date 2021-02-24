using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
	[Table("messages")]
	public class DbMessage
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }
		
		[Column("dialog_id")]
		public string DialogId { get; set; }
		
		[Column("sender_type")]
		public string SenderType { get; set; }
		
		[Column("sender_id")]
		public string SenderId { get; set; }

		[Column("timestamp")]
		public long Timestamp { get; set; }
		
		[Column("message")]
		public string Message { get; set; }
	}
}