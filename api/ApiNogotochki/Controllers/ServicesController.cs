using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ApiNogotochki.ActionFilters;
using ApiNogotochki.Enums;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using ApiNogotochki.Filters;
using ApiNogotochki.Registry;
using ApiNogotochki.Repository;
using ApiNogotochki.Services;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/services")]
	public class ServicesController : ControllerBase
	{
		private static readonly Dictionary<string, Type> StringToType = ServicesRegistry.GetAll()
																						.ToDictionary(x => x.ServiceTypeString,
																									  x => x.ServiceType);

		private readonly ServicesRepository servicesRepository;
		private readonly UserFilter userFilter;
		private readonly UsersRepository usersRepository;
		private readonly PhoneNumberValidator phoneNumberValidator;

		public ServicesController(ServicesRepository servicesRepository,
								  UsersRepository usersRepository,
								  UserFilter userFilter,
								  PhoneNumberValidator phoneNumberValidator)
		{
			this.servicesRepository = servicesRepository;
			this.usersRepository = usersRepository;
			this.userFilter = userFilter;
			this.phoneNumberValidator = phoneNumberValidator;
		}

		[HttpPost]
		[Authorize(UserRoleEnum.User)]
		[Utf8StringBody]
		public IActionResult Save(string? stringBody)
		{
			if (string.IsNullOrEmpty(stringBody))
				return BadRequest("body is required");

			if (!TryDeserializeService(stringBody, out var deserializedService, out var error))
				return BadRequest(error);

			var service = deserializedService!;

			if (service.SearchType != SearchTypeEnum.Master && service.SearchType != SearchTypeEnum.Model)
				return BadRequest($"{nameof(service.SearchType)} should be '{SearchTypeEnum.Master}' or '{SearchTypeEnum.Model}'");

			if (!string.IsNullOrEmpty(service.PhoneNumber))
			{
				var phoneNumberValidationError = phoneNumberValidator.Validate(service.PhoneNumber);
				if (phoneNumberValidationError != null)
					return BadRequest(phoneNumberValidationError);
			}

			var user = HttpContext.TryGetUser();
			if (user == null)
				throw new InvalidStateException("User should be not null");

			service.UserId = user.Id;

			service = servicesRepository.Save(service);

			user.ServiceIds = (user.ServiceIds ?? new string[0]).Append(service.Id).ToArray();

			if (usersRepository.TryUpdateServiceIds(user.Id, user.ServiceIds) == null)
				throw new InvalidStateException("Can not update user service ids");

			return Ok(service);
		}

		[HttpPut("{id}")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfService]
		[Utf8StringBody]
		public IActionResult Update([FromRoute] string? id, string? stringBody)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (string.IsNullOrEmpty(stringBody))
				return BadRequest("body is required");

			if (!TryDeserializeService(stringBody, out var deserializedService, out var error))
				return BadRequest(error);

			var service = deserializedService!;
			
			if (service.SearchType != SearchTypeEnum.Master && service.SearchType != SearchTypeEnum.Model)
				return BadRequest($"{nameof(service.SearchType)} should be '{SearchTypeEnum.Master}' or '{SearchTypeEnum.Model}'");
			
			if (!string.IsNullOrEmpty(service.PhoneNumber))
			{
				var phoneNumberValidationError = phoneNumberValidator.Validate(service.PhoneNumber);
				if (phoneNumberValidationError != null)
					return BadRequest(phoneNumberValidationError);
			}

			service.Id = id;

			var updatedService = servicesRepository.TryUpdate(service);
			if (updatedService == null)
				return BadRequest("Can not update service");

			return Ok(updatedService);
		}

		[HttpGet("{id}/user")]
		public IActionResult GetUser([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var service = servicesRepository.FindById(id);
			if (service == null)
				return BadRequest($"Service with {nameof(id)}='{id}' not found");

			var user = usersRepository.FindById(service.UserId);
			if (user == null)
				throw new InvalidStateException($"User ({service.UserId}) of service (`{service.Id}`) not found");

			return Ok(userFilter.Filter(user, HttpContext.TryGetUser()?.Id == user.Id));
		}

		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var service = servicesRepository.FindById(id);
			if (service == null)
				return BadRequest($"Service with {nameof(id)}='{id}' not found");

			return Ok(service);
		}

		private bool TryDeserializeService(string str, out Service? service, out string? error)
		{
			error = null;
			service = DeserializeService(str, typeof(Service));
			if (string.IsNullOrEmpty(service.Type) || !StringToType.Keys.Contains(service.Type))
			{
				error = $"{nameof(Service.Type)} is required. Supported types = " +
						$"'{string.Join(", ", StringToType.Keys)}'";
				return false;
			}

			service = DeserializeService(str, StringToType[service.Type]);
			return true;
		}

		private Service DeserializeService(string str, Type type)
		{
			return (Service) JsonSerializer.Deserialize(str, type, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}
	}
}