using System.Collections.Generic;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;

#nullable enable

namespace ApiNogotochki.Indexers.SearchIndexers
{
	public class HaircutSearchIndexer : ServiceSearchIndexer<HaircutService>
	{
		public HaircutSearchIndexer(RepositoryContextFactory contextFactory) : base(contextFactory)
		{
		}

		protected override List<DbSearchIndexRecord> GetRecords(HaircutService service)
		{
			var records = new List<DbSearchIndexRecord>();
			
			records.AddRange(CreateRecords(service, x => x.Description.Title));

			return records;
		}
	}
}