using System.Linq;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Indexers
{
	public abstract class GeolocationsIndexerBase<TType> : IIndexer<TType>
	{
		private readonly RepositoryContextFactory contextFactory;

		protected GeolocationsIndexerBase(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public void Index(TType obj)
		{
			var record = CreateRecord(obj);
			
			using var context = contextFactory.Create();
			
			var dbRecord = context.GeolocationIndices.SingleOrDefault(x => x.TargetId == record.TargetId &&
																		   x.TargetType == record.TargetType);
			
			if (record.Geolocations == null)
			{
				if (dbRecord != null)
					context.GeolocationIndices.Remove(dbRecord);
			}
			else
			{
				if (dbRecord != null)
					dbRecord.Geolocations = record.Geolocations;
				else
					context.GeolocationIndices.Add(record);
			}

			context.SaveChanges();
		}

		protected abstract DbGeolocationIndexRecord CreateRecord(TType obj);
	}
}