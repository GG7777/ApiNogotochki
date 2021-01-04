using System.Collections.Generic;
using ApiNogotochki.Enums;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;

#nullable enable

namespace ApiNogotochki.Indexers.Services
{
	public class ServiceIndexer<TService> : IndexerBase<TService>
		where TService : Service
	{
		protected ServiceIndexer(RepositoryContextFactory contextFactory) : base(contextFactory)
		{
		}

		protected override List<DbSearchIndexRecord> GetRecords(TService service)
		{
			return new List<DbSearchIndexRecord>();
		}

		protected override DbSearchIndexRecord CreateRecord(TService service)
		{
			return new DbSearchIndexRecord
			{
				TargetId = service.Id,
				TargetType = TargetTypeEnum.Service,
			};
		}
	}
}