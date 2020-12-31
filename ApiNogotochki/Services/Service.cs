using ApiNogotochki.Services.Items;

namespace ApiNogotochki.Services
{
	public class Service
	{
		public string Id { get; set; }
		public string Type { get; set; }
		
		public ScheduleItem Schedule { get; set; }
		public PriceItem Price { get; set; }
		public PhotoItem Photo { get; set; }
		public AddressItem Address { get; set; }
		public PhotoItem Certificates { get; set; }
		public SocialNetworkItem SocialNetwork { get; set; }
	}
}