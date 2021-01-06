using ApiNogotochki.Dto;
using ApiNogotochki.Enums;
using ApiNogotochki.Manager;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("confirmation")]
	public class ConfirmationController : Controller
	{
		private readonly SmsConfirmationCodeManager smsConfirmationCodeManager;
		private readonly ConfirmationTokenManager confirmationTokenManager;
		private readonly PhoneNumberValidator phoneNumberValidator;

		public ConfirmationController(SmsConfirmationCodeManager smsConfirmationCodeManager,
									  ConfirmationTokenManager confirmationTokenManager, 
									  PhoneNumberValidator phoneNumberValidator)
		{
			this.smsConfirmationCodeManager = smsConfirmationCodeManager;
			this.confirmationTokenManager = confirmationTokenManager;
			this.phoneNumberValidator = phoneNumberValidator;
		}

		[HttpPost("sms/sending")]
		public IActionResult SendSms([FromBody] SmsConfirmationDto? confirmationDto)
		{
			if (confirmationDto == null)
				return BadRequest("body is required");
			
			if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
				return BadRequest($"{nameof(SmsConfirmationDto.PhoneNumber)} is required");
			
			if (string.IsNullOrEmpty(confirmationDto.ConfirmationType))
				return BadRequest($"{nameof(SmsConfirmationDto.ConfirmationType)} is required");

			var phoneNumberValidationError = phoneNumberValidator.Validate(confirmationDto.PhoneNumber);
			if (phoneNumberValidationError != null)
				return BadRequest(phoneNumberValidationError);

			var sendError = smsConfirmationCodeManager.TrySendSmsCode(confirmationDto.PhoneNumber, confirmationDto.ConfirmationType);
			if (sendError != null)
				return BadRequest(sendError);

			return Ok();
		}

		[HttpPost("sms")]
		public IActionResult ConfirmSms([FromBody] SmsConfirmationDto? confirmationDto)
		{
			if (confirmationDto == null)
				return BadRequest("body is required");
			
			if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
				return BadRequest($"{nameof(SmsConfirmationDto.PhoneNumber)} is required");
			
			if (string.IsNullOrEmpty(confirmationDto.ConfirmationType))
				return BadRequest($"{nameof(SmsConfirmationDto.ConfirmationType)} is required");

			if (string.IsNullOrEmpty(confirmationDto.ConfirmationCode))
				return BadRequest($"{nameof(SmsConfirmationDto.ConfirmationCode)} is required");

			var phoneNumberValidationError = phoneNumberValidator.Validate(confirmationDto.PhoneNumber);
			if (phoneNumberValidationError != null)
				return BadRequest(phoneNumberValidationError);

			var confirmError = smsConfirmationCodeManager.TryConfirmSmsCode(confirmationDto.PhoneNumber, 
																			confirmationDto.ConfirmationType, 
																			confirmationDto.ConfirmationCode);
			if (confirmError != null)
				return BadRequest(confirmError);

			return Ok(new ConfirmationDto
			{
				ConfirmationToken = confirmationTokenManager.Create(ConfirmationTypeEnum.PhoneNumber, 
																	confirmationDto.PhoneNumber),
			});
		}
	}
}