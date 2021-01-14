using ApiNogotochki.Items;

namespace ApiNogotochki.Interfaces
{
	public interface IGeolocationsContainer
	{
		Geolocation[] Geolocations { get; set; }
	}
}