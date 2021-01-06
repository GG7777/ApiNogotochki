using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApiNogotochki.ActionFilters;
using ApiNogotochki.Registry;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/services")]
	public class ServicesController : ControllerBase
	{
		private readonly ServicesRepository servicesRepository;

		private readonly Dictionary<string, Type> stringToType = ServicesRegistry.GetAll()
																				 .ToDictionary(x => x.ServiceTypeString,
																							   x => x.ServiceType);

		public ServicesController(ServicesRepository servicesRepository)
		{
			this.servicesRepository = servicesRepository;
		}

		[HttpPost]
		[Utf8StringBody]
		public IActionResult Save(string? stringBody)
		{
			if (string.IsNullOrEmpty(stringBody))
				return BadRequest("body is required");

			object Deserialize(Type type)
			{
				return JsonSerializer.Deserialize(stringBody, type, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});
			}

			var service = (Service) Deserialize(typeof(Service));

			if (string.IsNullOrEmpty(service.Type) || !stringToType.Keys.Contains(service.Type))
				return BadRequest($"{nameof(Service.Type)} is required. Supported types = " +
								  $"'{string.Join(", ", stringToType.Keys)}'");

			service = (Service) Deserialize(stringToType[service.Type]);

			return Ok(servicesRepository.Save(service));
		}

		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var service = servicesRepository.TryGet(id);
			if (service == null)
				return BadRequest($"Service with {nameof(id)}='{id}' not found");

			return Ok(service);
		}
	}
}