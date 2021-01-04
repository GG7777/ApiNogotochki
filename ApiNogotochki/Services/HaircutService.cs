using ApiNogotochki.Services.Items;

namespace ApiNogotochki.Services
{
	public class HaircutService : Service
	{
		public OwnerItem Owner { get; set; }
		public DescriptionItem Description { get; set; }
		public PhotosItem Photos { get; set; }

		public ScheduleItem Schedule { get; set; }
		public PriceItem Price { get; set; }
		public AddressItem Address { get; set; }
		public PhotosItem Certificates { get; set; }
		public SocialNetworkItem SocialNetwork { get; set; }
	}
}