using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Indexers.SearchIndexers
{
	public abstract class SearchIndexerBase<TType> : IIndexer<TType>
	{
		private readonly RepositoryContextFactory contextFactory;

		protected SearchIndexerBase(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public void Index(TType obj)
		{
			var records = GetRecords(obj);
			if (records.Any())
			{
				using var context = contextFactory.Create();

				foreach (var record in records)
				{
					var dbRecord = context.SearchIndices.SingleOrDefault(x => x.TargetId == record.TargetId &&
																			  x.TargetType == record.TargetType &&
																			  x.ValueType == record.ValueType);
					if (dbRecord != null)
						dbRecord.Value = record.Value;
					else
						context.SearchIndices.Add(record);
				}

				context.SaveChanges();
			}
		}

		protected abstract List<DbSearchIndexRecord> GetRecords(TType obj);

		protected abstract DbSearchIndexRecord CreateRecord(TType obj);

		protected List<DbSearchIndexRecord> CreateRecords<TProperty>(TType service,
																	 string valueType,
																	 Func<TType, TProperty> getPropertyValue)
		{
			var emptyList = new List<DbSearchIndexRecord>();

			object? obj;
			try
			{
				obj = getPropertyValue(service);
			}
			catch (NullReferenceException)
			{
				return emptyList;
			}

			if (obj is string || !(obj is IEnumerable))
				obj = new[] {obj};

			var values = new List<string>();
			foreach (var value in (IEnumerable) obj)
			{
				var strValue = value?.ToString()?.Replace(",", "");
				if (string.IsNullOrEmpty(strValue))
					break;
				values.Add(strValue);
			}

			var record = CreateRecord(service);

			record.ValueType = valueType;
			record.Value = string.Join(",", values);

			if (string.IsNullOrWhiteSpace(record.Value))
				return emptyList;

			return new List<DbSearchIndexRecord> {record};
		}
	}
}