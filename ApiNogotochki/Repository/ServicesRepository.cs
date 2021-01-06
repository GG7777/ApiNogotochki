using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApiNogotochki.Indexers;
using ApiNogotochki.Model;
using ApiNogotochki.Registry;
using ApiNogotochki.Services;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class ServicesRepository
	{
		private readonly RepositoryContextFactory contextFactory;
		private readonly Indexer indexer;

		private readonly Dictionary<string, Type> serviceTypeStringToType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceTypeString,
																										  x => x.ServiceType);

		private readonly Dictionary<Type, string> serviceTypeToStringType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceType,
																										  x => x.ServiceTypeString);

		public ServicesRepository(RepositoryContextFactory contextFactory, Indexer indexer)
		{
			this.contextFactory = contextFactory;
			this.indexer = indexer;
		}

		public Service Save(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			service.Id = Guid.NewGuid().ToString();

			using var context = contextFactory.Create();

			context.Services.Add(ToDbService(service));
			context.SaveChanges();

			indexer.Index(service);

			return service;
		}

		public Service? TryUpdate(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			using var context = contextFactory.Create();

			if (!context.Services.Any(x => x.Id == service.Id && !x.IsRemoved))
				return null;

			context.Services.Update(ToDbService(service));
			context.SaveChanges();

			indexer.Index(service);

			return service;
		}

		public bool TryRemove(string id)
		{
			using var context = contextFactory.Create();

			var dbService = context.Services.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (dbService == null)
				return false;

			dbService.IsRemoved = true;

			context.SaveChanges();

			return true;
		}

		public Service? TryGet(string id)
		{
			using var context = contextFactory.Create();
			var dbService = context.Services.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (dbService == null)
				return null;

			return FromDbService(dbService);
		}

		public Service[] Get(string[] ids)
		{
			using var context = contextFactory.Create();
			return context.Services
						  .Where(x => ids.Contains(x.Id) && !x.IsRemoved).AsEnumerable()
						  .Select(FromDbService)
						  .ToArray();
		}

		private void ValidateTypes(string serviceTypeString, Type serviceType)
		{
			if (serviceTypeStringToType[serviceTypeString] != serviceType ||
				serviceTypeToStringType[serviceType] != serviceTypeString)
				throw new Exception("");
		}

		private DbService ToDbService(Service service)
		{
			return new DbService
			{
				Id = service.Id,
				Type = service.Type,
				IsRemoved = false,
				Content = JsonSerializer.Serialize(service, serviceTypeStringToType[service.Type])
			};
		}

		private Service FromDbService(DbService dbService)
		{
			return (Service) JsonSerializer.Deserialize(dbService.Content, serviceTypeStringToType[dbService.Type]);
		}
	}
}