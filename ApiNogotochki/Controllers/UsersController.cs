using ApiNogotochki.Enums;
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
	[Route("users")]
	public class UsersController : ControllerBase
	{
		private readonly UsersRepository usersRepository;
		private readonly PhoneNumberValidator phoneNumberValidator;

		public UsersController(UsersRepository usersRepository, PhoneNumberValidator phoneNumberValidator)
		{
			this.usersRepository = usersRepository;
			this.phoneNumberValidator = phoneNumberValidator;
		}

		[HttpPut("{id}")]
		[DbUserAuthorize(UserRoleEnum.User)]
		[CheckSelf]
		public IActionResult Update([FromRoute] string id, [FromBody] DbUser user)
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
		[DbUserAuthorize(UserRoleEnum.User)]
		[CheckSelf]
		[Confirmation(ConfirmationTypeEnum.PhoneNumber)]
		public IActionResult UpdatePhoneNumber([FromRoute] string id, [FromBody] DbUser user)
		{
			if (HttpContext.GetConfirmedValue() != user.PhoneNumber)
				return new Filters.ForbidResult();
			
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");
			
			if (user == null || string.IsNullOrEmpty(user.PhoneNumber))
				return BadRequest($"{nameof(DbUser.PhoneNumber)} is required");

			var phoneNumberValidationError = phoneNumberValidator.Validate(user.PhoneNumber);
			if (phoneNumberValidationError != null)
				return BadRequest(phoneNumberValidationError);

			var updatedUser = usersRepository.TryUpdatePhoneNumber(id, user.PhoneNumber);
			if (updatedUser == null)
				return BadRequest("Can not update user phone number");

			return Ok(updatedUser);
		}

		[HttpPatch("{id}/nickname")]
		[DbUserAuthorize(UserRoleEnum.User)]
		[CheckSelf]
		public IActionResult UpdateNickname([FromRoute] string id, [FromBody] DbUser user)
		{
			if (string.IsNullOrEmpty(id))
				return BadRequest($"{nameof(id)} is required");
			if (user == null || string.IsNullOrEmpty(user.Nickname))
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