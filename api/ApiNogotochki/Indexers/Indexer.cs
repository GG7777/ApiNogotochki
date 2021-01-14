using System;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace ApiNogotochki.Indexers
{
	public class Indexer
	{
		private readonly IServiceProvider serviceProvider;

		private readonly ConcurrentDictionary<Type, MethodInfo> typeToMethod = new ConcurrentDictionary<Type, MethodInfo>();

		public Indexer(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public void Index(object obj)
		{
			var type = typeof(IIndexer<>).MakeGenericType(obj.GetType());
			foreach (var indexer in serviceProvider.GetServices(type))
				typeToMethod.GetOrAdd(type, x => x.GetMethod(nameof(IIndexer<object>.Index))!).Invoke(indexer, new[] {obj});
		}
	}
}