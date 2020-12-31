using System.Collections.Generic;
using System.Linq;
using ApiNogotochki.Registry;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("services")]
	public class ServicesController : ControllerBase
	{
		private readonly ServicesRepository servicesRepository;

		private readonly HashSet<string> supportedTypes = ServicesRegistry.GetAll()
																		  .Select(x => x.ServiceTypeString)
																		  .ToHashSet();

		public ServicesController(ServicesRepository servicesRepository)
		{
			this.servicesRepository = servicesRepository;
		}

		[HttpPost]
		public IActionResult Save([FromBody] HaircutService? service)
		{
			if (service == null)
				return BadRequest("body is required");

			if (string.IsNullOrEmpty(service.Type) || !supportedTypes.Contains(service.Type))
				return BadRequest($"{nameof(Service.Type)} is required. Supported types = " +
								  $"'{string.Join(", ", supportedTypes)}'");

			return Ok(servicesRepository.Save(service));
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var element = servicesRepository.TryGet(id);
			if (element == null)
				return BadRequest($"Service with {nameof(id)}='{id}' not found");

			return Ok(element);
		}
	}
}