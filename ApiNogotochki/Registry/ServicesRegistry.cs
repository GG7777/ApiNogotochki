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
			}
		};

		public static ServiceInfo[] GetAll()
		{
			return ServiceElementsInfos;
		}
	}
}