using System;
using System.Linq;
using ApiNogotochki.ActionFilters;
using ApiNogotochki.Enums;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using ApiNogotochki.Filters;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/users")]
	public class UsersController : ControllerBase
	{
		private readonly PhoneNumberValidator phoneNumberValidator;
		private readonly ServicesRepository servicesRepository;
		private readonly UserFilter userFilter;
		private readonly UsersRepository usersRepository;

		public UsersController(UsersRepository usersRepository,
							   ServicesRepository servicesRepository,
							   PhoneNumberValidator phoneNumberValidator,
							   UserFilter userFilter)
		{
			this.usersRepository = usersRepository;
			this.phoneNumberValidator = phoneNumberValidator;
			this.servicesRepository = servicesRepository;
			this.userFilter = userFilter;
		}

		[HttpPut("{id}")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfUser]
		public IActionResult Update([FromRoute] string? id, [FromBody] DbUser? user)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (user == null)
				return BadRequest("body is required");

			user.Id = id;

			var updatedUser = usersRepository.TryUpdate(user);
			if (updatedUser == null)
				return BadRequest("Can not update user");

			return Ok(updatedUser);
		}

		[HttpPatch("{id}/phone-number")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfUser]
		[ConfirmedPhoneNumber]
		public IActionResult UpdatePhoneNumber([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var phoneNumber = HttpContext.TryGetConfirmedPhoneNumber();
			if (string.IsNullOrEmpty(phoneNumber))
				throw new InvalidStateException("phone number should not be null or empty");

			var phoneNumberValidationError = phoneNumberValidator.Validate(phoneNumber);
			if (phoneNumberValidationError != null)
				throw new InvalidStateException("phone number should be valid");

			var updatedUser = usersRepository.TryUpdatePhoneNumber(id, phoneNumber);
			if (updatedUser == null)
				return BadRequest("Can not update user phone number");

			return Ok(updatedUser);
		}

		[HttpPatch("{id}/nickname")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfUser]
		public IActionResult UpdateNickname([FromRoute] string? id, [FromBody] DbUser? user)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (user == null)
				return BadRequest("body is required");

			if (string.IsNullOrEmpty(user.Nickname))
				return BadRequest($"{nameof(DbUser.Nickname)} is required");

			var updatedUser = usersRepository.TryUpdateNickname(id, user.Nickname);
			if (updatedUser == null)
				return BadRequest("Can not update user nickname");

			return Ok(updatedUser);
		}

		[HttpPatch("{id}/service-ids")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfUser]
		public IActionResult UpdateServiceIds([FromRoute] string? id, [FromBody] DbUser? user)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (user == null)
				return BadRequest("body is required");

			if (user.ServiceIds?.Any(string.IsNullOrEmpty) == true)
				return BadRequest($"{nameof(DbUser.ServiceIds)} should not contain null or empty ids");

			var serviceIds = user.ServiceIds;
			if (serviceIds != null)
			{
				var services = servicesRepository.FindByIds(serviceIds);
				if (services.Length != serviceIds.Length)
					return BadRequest("All services should exists");
				if (services.Any(x => x.UserId != id))
					return BadRequest("All services should belong to user");
			}

			var updatedUser = usersRepository.TryUpdateServiceIds(id, serviceIds);
			if (updatedUser == null)
				return BadRequest("Can not update service ids");

			servicesRepository.RemoveUserServices(updatedUser.Id, serviceIds ?? new string[0]);

			return Ok(updatedUser);
		}

		[HttpGet("{id}/services")]
		public IActionResult GetServices([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (string.Equals(id, "my", StringComparison.InvariantCultureIgnoreCase))
			{
				var me = HttpContext.TryGetUser();
				if (me != null)
					return Ok(servicesRepository.FindByIds(me.ServiceIds ?? new string[0]));
				return BadRequest("You are not authorized");
			}

			var user = usersRepository.FindById(id);
			if (user == null)
				return BadRequest($"User with id = '{id}' not found");

			return Ok(servicesRepository.FindByIds(user.ServiceIds ?? new string[0]));
		}

		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (string.Equals(id, "me", StringComparison.InvariantCultureIgnoreCase))
			{
				var me = HttpContext.TryGetUser();
				if (me != null)
					return Ok(userFilter.Filter(me, true));
				return BadRequest("You are not authorized");
			}

			var user = usersRepository.FindById(id);
			if (user == null)
				return BadRequest($"User with id = '{id}' not found");

			return Ok(userFilter.Filter(user, HttpContext.TryGetUser()?.Id == user.Id));
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string?[]? ids)
		{
			if (ids == null)
				return BadRequest($"{nameof(ids)} is required");

			var notNullIds = ids.Where(x => !string.IsNullOrEmpty(x)).ToArray();

			return Ok(usersRepository.FindByIds(notNullIds!)
									 .Select(x => userFilter.Filter(x, HttpContext.TryGetUser()?.Id == x.Id)));
		}
	}
}