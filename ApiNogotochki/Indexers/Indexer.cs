using System;

#nullable enable

namespace ApiNogotochki.Indexers
{
	public class Indexer
	{
		private readonly IServiceProvider serviceProvider;

		public Indexer(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public void Index(object obj)
		{
			var type = typeof(IIndexer<>).MakeGenericType(obj.GetType());
			var indexer = serviceProvider.GetService(type);
			type.GetMethod(nameof(IIndexer<object>.Index))!.Invoke(indexer, new[] {obj});
		}
	}
}