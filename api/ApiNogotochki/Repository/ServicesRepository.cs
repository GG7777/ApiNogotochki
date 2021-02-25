using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Indexers;
using ApiNogotochki.Model;
using ApiNogotochki.Registry;
using ApiNogotochki.Services;
using ApiNogotochki.Validators;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace ApiNogotochki.Repository
{
	public class ServicesRepository
	{
		private readonly RepositoryContextFactory contextFactory;

		private readonly Dictionary<string, Type> serviceTypeStringToType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceTypeString,
																										  x => x.ServiceType);

		private readonly Dictionary<Type, string> serviceTypeToStringType = ServicesRegistry.GetAll()
																							.ToDictionary(x => x.ServiceType,
																										  x => x.ServiceTypeString);

		public ServicesRepository(RepositoryContextFactory contextFactory)
		{
			this.contextFactory = contextFactory;
		}

		public Service Save(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			service.Id = Guid.NewGuid().ToString();
			service.IsRemoved = false;

			using var context = contextFactory.Create();

			context.Services.Add(ToDbService(service));
			context.ServicesMetas.Add(ToDbServiceMeta(service));

			context.SaveChanges();

			return service;
		}

		public Service? TryUpdate(Service service)
		{
			ValidateTypes(service.Type, service.GetType());

			using var context = contextFactory.Create();

			var existingService = context.Services
										 .AsNoTrackingWithIdentityResolution()
										 .SingleOrDefault(x => x.Id == service.Id && !x.IsRemoved);
			if (existingService == null)
				return null;

			service.Id = existingService.Id;
			service.Type = existingService.Type;
			service.UserId = existingService.UserId;
			service.IsRemoved = existingService.IsRemoved;

			context.Services.Update(ToDbService(service));
			context.ServicesMetas.Update(ToDbServiceMeta(service));

			context.SaveChanges();

			return service;
		}

		public bool TryRemove(string id)
		{
			using var context = contextFactory.Create();

			var dbService = context.Services.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (dbService == null)
				return false;

			var dbServiceMeta = context.Services.SingleOrDefault(x => x.Id == id && !x.IsRemoved);

			dbService.IsRemoved = true;
			dbServiceMeta.IsRemoved = true;

			context.SaveChanges();

			return true;
		}

		public void RemoveUserServices(string userId, string[] excludingIds)
		{
			using var context = contextFactory.Create();

			var servicesToRemove = context.Services
										  .Where(x => x.UserId == userId &&
													  !x.IsRemoved &&
													  !excludingIds.Contains(x.Id));
			var servicesMetasToRemove = context.ServicesMetas
											   .Where(x => x.UserId == userId &&
														   !x.IsRemoved &&
														   !excludingIds.Contains(x.Id));
			foreach (var service in servicesToRemove)
				service.IsRemoved = true;
			foreach (var serviceMeta in servicesMetasToRemove)
				serviceMeta.IsRemoved = true;

			context.SaveChanges();
		}

		public Service? FindById(string id)
		{
			using var context = contextFactory.Create();
			var dbService = context.Services.SingleOrDefault(x => x.Id == id && !x.IsRemoved);
			if (dbService == null)
				return null;

			return FromDbService(dbService);
		}

		public DbServiceMeta[] FindBySearchType(string searchType, string serviceType, int offset, int count)
		{
			using var context = contextFactory.Create();
			return context.ServicesMetas.Where(x => x.SearchType == searchType &&
			                                        x.Type == serviceType &&
			                                        !x.IsRemoved)
										.Skip(offset)
										.Take(count)
										.ToArray();
		}

		public int GetCount(string searchType)
		{
			using var context = contextFactory.Create();
			return context.ServicesMetas.Count(x => x.SearchType == searchType && !x.IsRemoved);
		}

		public DbServiceMeta[] FindByIds(params string[] ids)
		{
			if (!ids.Any())
				return new DbServiceMeta[0];

			using var context = contextFactory.Create();
			return context.ServicesMetas
						  .Where(x => ids.Contains(x.Id) && !x.IsRemoved)
						  .ToArray();
		}

		private DbServiceMeta ToDbServiceMeta(Service service)
		{
			return new DbServiceMeta
			{
				Id = service.Id,
				Type = service.Type,
				UserId = service.UserId,
				IsRemoved = service.IsRemoved,
				SearchType = service.SearchType,
				PhoneNumber = service.PhoneNumber,
				Title = service.Title,
				Description = service.Description,
				PhotoId = service.PhotoId,
				Geolocation = service.Geolocation,
			};
		}

		private void ValidateTypes(string serviceTypeString, Type serviceType)
		{
			if (serviceTypeStringToType[serviceTypeString] != serviceType ||
				serviceTypeToStringType[serviceType] != serviceTypeString)
				throw new InvalidStateException($"Invalid types mapping: `{serviceTypeString}` and `{serviceType}`");
		}

		private DbService ToDbService(Service service)
		{
			return new DbService
			{
				Id = service.Id,
				Type = service.Type,
				UserId = service.UserId,
				IsRemoved = service.IsRemoved,
				Content = JsonSerializer.Serialize(service, serviceTypeStringToType[service.Type])
			};
		}

		private Service FromDbService(DbService dbService)
		{
			return (Service) JsonSerializer.Deserialize(dbService.Content, serviceTypeStringToType[dbService.Type]);
		}
	}
}