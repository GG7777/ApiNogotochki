using ApiNogotochki.ActionFilters;
using ApiNogotochki.Enums;
using ApiNogotochki.Extensions;
using ApiNogotochki.Model;
using ApiNogotochki.Repository;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;
using ForbidResult = ApiNogotochki.ActionResults.ForbidResult;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/users")]
	public class UsersController : ControllerBase
	{
		private readonly PhoneNumberValidator phoneNumberValidator;
		private readonly UsersRepository usersRepository;

		public UsersController(UsersRepository usersRepository, PhoneNumberValidator phoneNumberValidator)
		{
			this.usersRepository = usersRepository;
			this.phoneNumberValidator = phoneNumberValidator;
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
				return BadRequest("can not update user");

			return Ok(updatedUser);
		}

		[HttpPatch("{id}/phone-number")]
		[Authorize(UserRoleEnum.User)]
		[AccessToSelfUser]
		[ConfirmedPhoneNumber]
		public IActionResult UpdatePhoneNumber([FromRoute] string? id, [FromBody] DbUser? user)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			if (user == null)
				return BadRequest("body is required");

			if (string.IsNullOrEmpty(user.PhoneNumber))
				return BadRequest($"{nameof(DbUser.PhoneNumber)} is required");

			var phoneNumberValidationError = phoneNumberValidator.Validate(user.PhoneNumber);
			if (phoneNumberValidationError != null)
				return BadRequest(phoneNumberValidationError);

			if (HttpContext.TryGetConfirmedPhoneNumber() != user.PhoneNumber)
				return new ForbidResult();

			var updatedUser = usersRepository.TryUpdatePhoneNumber(id, user.PhoneNumber);
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

		[HttpGet("{id}")]
		public IActionResult Get([FromRoute] string? id)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");

			var user = usersRepository.FindById(id);
			if (user == null)
				return BadRequest($"User with id = '{id}' not found");

			return Ok(user);
		}
	}
}