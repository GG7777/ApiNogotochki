using ApiNogotochki.Enums;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Indexers
{
	public class UserGeolocationsIndexer : GeolocationsIndexerBase<DbUser>
	{
		public UserGeolocationsIndexer(RepositoryContextFactory contextFactory) : base(contextFactory)
		{
		}

		protected override DbGeolocationIndexRecord CreateRecord(DbUser obj)
		{
			return new DbGeolocationIndexRecord
			{
				TargetId = obj.Id,
				TargetType = TargetTypeEnum.User,
				Geolocations = obj.Geolocations
			};
		}
	}
}