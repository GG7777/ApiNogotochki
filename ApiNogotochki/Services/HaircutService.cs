using ApiNogotochki.Interfaces;
using ApiNogotochki.Items;
using ApiNogotochki.Services.Items;

namespace ApiNogotochki.Services
{
	public class HaircutService : Service, IGeolocationsContainer
	{
		public Owner Owner { get; set; }
		public Description Description { get; set; }
		public Photos Photos { get; set; }

		public Schedule Schedule { get; set; }
		public Price Price { get; set; }
		public Address Address { get; set; }
		public Photos Certificates { get; set; }
		public SocialNetwork SocialNetwork { get; set; }
		public Geolocation[] Geolocations { get; set; }
	}
}