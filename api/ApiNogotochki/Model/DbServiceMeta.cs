using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiNogotochki.Items;

namespace ApiNogotochki.Model
{
	[Table("services_meta")]
	public class DbServiceMeta
	{
		[Key]
		[Column("id")]
		public string Id { get; set; }

		[Column("type")]
		public string Type { get; set; }
		
		[Column("user_id")]
		public string UserId { get; set; }
		
		[Column("is_removed")]
		public bool IsRemoved { get; set; }
		
		[Column("search_type")]
		public string SearchType { get; set; }
		
		[Column("phone_number")]
		public string PhoneNumber { get; set; }
		
		[Column("title")]
		public string Title { get; set; }
		
		[Column("description")]
		public string Description { get; set; }
		
		[Column("photo_id")]
		public string PhotoId { get; set; }
		
		[Column("geolocation", TypeName = "jsonb")]
		public Geolocation Geolocation { get; set; }
	}
}