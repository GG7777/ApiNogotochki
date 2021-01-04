using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;

#nullable enable

namespace ApiNogotochki.Indexers
{
	public abstract class IndexerBase<TType> : IIndexer<TType>
	{
		private readonly RepositoryContextFactory contextFactory;

		protected IndexerBase(RepositoryContextFactory contextFactory)
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
																	 Expression<Func<TType, TProperty>> expression)
		{
			var memberExpression = (MemberExpression) expression.Body;

			object? obj;
			try
			{
				obj = expression.Compile().Invoke(service);
			}
			catch (NullReferenceException)
			{
				return new List<DbSearchIndexRecord>();
			}

			if (obj is string || !(obj is IEnumerable))
				obj = new[] {obj};

			var values = new List<string>();
			foreach (var value in (IEnumerable) obj)
			{
				if (value == null)
					break;
				values.Add(value.ToString()!);
			}

			var record = CreateRecord(service);

			record.ValueType = memberExpression.Member.Name;
			record.Value = string.Join(",", values);

			return new List<DbSearchIndexRecord> {record};
		}
	}
}