using ApiNogotochki.Enums;
using ApiNogotochki.Interfaces;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;

namespace ApiNogotochki.Indexers
{
	public class ServicesGeolocationsIndexer<TType> : GeolocationsIndexerBase<TType> 
		where TType : Service, IGeolocationsContainer
	{
		public ServicesGeolocationsIndexer(RepositoryContextFactory contextFactory) : base(contextFactory)
		{
		}

		protected override DbGeolocationIndexRecord CreateRecord(TType obj)
		{
			return new DbGeolocationIndexRecord
			{
				TargetId = obj.Id,
				TargetType = TargetTypeEnum.Service,
				Geolocations = obj.Geolocations
			};
		}
	}
}