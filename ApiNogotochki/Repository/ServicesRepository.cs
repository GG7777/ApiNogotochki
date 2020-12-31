using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApiNogotochki.Model;
using ApiNogotochki.Registry;
using ApiNogotochki.Services;
using ApiNogotochki.Storage;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class ServicesRepository
	{
		private readonly Dictionary<string, Type> serviceTypeStringToType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceTypeString,
																										  x => x.ServiceType);

		private readonly Dictionary<Type, string> serviceTypeToStringType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceType,
																										  x => x.ServiceTypeString);

		private readonly ServicesStorage storage;

		public ServicesRepository(ServicesStorage storage)
		{
			this.storage = storage;
		}

		public Service Save(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			service.Id = Guid.NewGuid().ToString();

			storage.Write(ToDbService(service));

			return service;
		}

		public Service? TryUpdate(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			var dbService = storage.TryRead(service.Id);
			if (dbService == null)
				return null;

			storage.Write(ToDbService(service));

			return service;
		}

		public bool TryRemove(string id)
		{
			var dbService = storage.TryRead(id);
			if (dbService == null)
				return false;

			dbService.IsRemoved = true;
			storage.Write(dbService);

			return true;
		}

		public Service? TryGet(string id)
		{
			var dbService = storage.TryRead(id);
			if (dbService == null || dbService.IsRemoved)
				return null;

			return FromDbService(dbService);
		}

		public Service[] Get(string[] ids)
		{
			return storage.Read(ids)
						  .Where(x => !x.IsRemoved)
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
				Content = JsonSerializer.Serialize(service)
			};
		}

		private Service FromDbService(DbService dbService)
		{
			return (Service) JsonSerializer.Deserialize(dbService.Content, serviceTypeStringToType[dbService.Type]);
		}
	}
}