using ApiNogotochki.ActionFilters;
using ApiNogotochki.Dto;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using ApiNogotochki.Manager;
using ApiNogotochki.Repository;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("api/v1/authentication")]
	public class AuthenticationController : ControllerBase
	{
		private readonly PhoneNumberValidator phoneNumberValidator;
		private readonly JwtTokenProvider tokenProvider;
		private readonly UsersRepository usersRepository;

		public AuthenticationController(UsersRepository usersRepository,
										JwtTokenProvider tokenProvider,
										PhoneNumberValidator phoneNumberValidator)
		{
			this.usersRepository = usersRepository;
			this.tokenProvider = tokenProvider;
			this.phoneNumberValidator = phoneNumberValidator;
		}

		[HttpPost]
		[ConfirmedPhoneNumber]
		public IActionResult Authorize()
		{
			var phoneNumber = HttpContext.TryGetConfirmedPhoneNumber();
			if (string.IsNullOrEmpty(phoneNumber))
				throw new InvalidStateException("PhoneNumber should not be null or empty");

			var phoneNumberValidationError = phoneNumberValidator.Validate(phoneNumber);
			if (phoneNumberValidationError != null)
				throw new InvalidStateException("PhoneNumber should be valid");

			var user = usersRepository.GetOrCreate(phoneNumber);
			var token = tokenProvider.GetToken(user);

			return Ok(new AuthorizationDto
			{
				UserId = user.Id,
				AuthorizationToken = token
			});
		}
	}
}