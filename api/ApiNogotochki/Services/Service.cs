using ApiNogotochki.Items;

namespace ApiNogotochki.Services
{
	public class Service
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }
		public bool IsRemoved { get; set; }
		
		public Title Title { get; set; }
		public Description Description { get; set; }
		public Photos Photos { get; set; }
		public SocialNetwork[] SocialNetworks { get; set; }
		public Geolocation[] Geolocations { get; set; }
	}
}