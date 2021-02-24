using ApiNogotochki.Items;

namespace ApiNogotochki.Services
{
	public class Service
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }
		public bool IsRemoved { get; set; }
		
		public string SearchType { get; set; }
		public string PhoneNumber { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string PhotoId { get; set; }
		public Geolocation Geolocation { get; set; }
	}
}