using System.Collections.Generic;
using System.Linq;
using ApiNogotochki.Enums;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Indexers.SearchIndexers
{
	public class UserIndexer : SearchIndexerBase<DbUser>
	{
		public UserIndexer(RepositoryContextFactory contextFactory) : base(contextFactory)
		{
		}

		protected override List<DbSearchIndexRecord> GetRecords(DbUser obj)
		{
			var records = new List<DbSearchIndexRecord>();

			records.AddRange(CreateRecords(obj, "nickname", x => x.Nickname));
			records.AddRange(CreateRecords(obj, "name", x => x.Name));
			records.AddRange(CreateRecords(obj, "description", x => x.Description));
			records.AddRange(CreateRecords(obj, "social-networks", x => x.SocialNetworks.Select(z => z.Value)));

			return records;
		}

		protected override DbSearchIndexRecord CreateRecord(DbUser obj)
		{
			return new DbSearchIndexRecord
			{
				TargetId = obj.Id,
				TargetType = TargetTypeEnum.User
			};
		}
	}
}