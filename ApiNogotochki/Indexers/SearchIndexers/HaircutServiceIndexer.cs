using System.Collections.Generic;
using System.Linq;
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
			
			records.AddRange(CreateRecords(service, "title", x => x.Title.TitleValue));
			records.AddRange(CreateRecords(service, "description", x => x.Description.DescriptionValue));
			records.AddRange(CreateRecords(service, "social-networks", x => x.SocialNetworks.Select(z => z.Value)));

			return records;
		}
	}
}