using ApiNogotochki.Services;

#nullable enable

namespace ApiNogotochki.Registry
{
	public static class ServicesRegistry
	{
		private static readonly ServiceInfo[] ServiceElementsInfos =
		{
			new ServiceInfo
			{
				ServiceType = typeof(HaircutService),
				ServiceTypeString = "haircut"
			},
			new ServiceInfo
			{
				ServiceType = typeof(NailsService),
				ServiceTypeString = "nails"
			},
			new ServiceInfo
			{
				ServiceType = typeof(EyebrowsService),
				ServiceTypeString = "eyebrows"
			},
			new ServiceInfo
			{
				ServiceType = typeof(EyelashesService),
				ServiceTypeString = "eyelashes"
			}, 
		};

		public static ServiceInfo[] GetAll()
		{
			return ServiceElementsInfos;
		}
	}
}