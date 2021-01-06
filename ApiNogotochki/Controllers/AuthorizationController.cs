using System;
using ApiNogotochki.Dto;
using ApiNogotochki.Enums;
using ApiNogotochki.Extensions;
using ApiNogotochki.Filters;
using ApiNogotochki.Manager;
using ApiNogotochki.Repository;
using ApiNogotochki.Validators;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("authorization")]
    public class AuthorizationController : ControllerBase
	{
        private readonly UsersRepository usersRepository;
        private readonly JwtTokenProvider tokenProvider;
        private readonly PhoneNumberValidator phoneNumberValidator;

        public AuthorizationController(UsersRepository usersRepository, 
                                       JwtTokenProvider tokenProvider, 
                                       PhoneNumberValidator phoneNumberValidator)
        {
            this.usersRepository = usersRepository;
            this.tokenProvider = tokenProvider;
            this.phoneNumberValidator = phoneNumberValidator;
        }

        [HttpPost]
        [Confirmation(ConfirmationTypeEnum.PhoneNumber)]
        public IActionResult Authorize()
        {
            var phoneNumber = HttpContext.GetConfirmedValue();
            if (phoneNumber == null)
                throw new Exception("GG WP, WE BROKEN");

            var phoneNumberValidationError = phoneNumberValidator.Validate(phoneNumber);
            if (phoneNumberValidationError != null)
                throw new Exception("GG WP, PHONE NUMBER BROKEN");
            
            var user = usersRepository.GetOrCreate(phoneNumber);
            var token = tokenProvider.GetToken(user);

            return Ok(new AuthenticationDto
            {
                UserId = user.Id,
                AuthenticationToken = token,
            });
        }
        
        // [HttpPost("sms/sending")]
        // public IActionResult SendConfirmationSmsCode([FromBody] SmsConfirmationDto? confirmationDto)
        // {
        //     if (confirmationDto == null)
        //         return BadRequest("Body is required");
        //     
        //     if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
        //         return BadRequest("Phone number is required");
        //
        //     var validatePhoneNumberError = phoneNumberValidator.Validate(confirmationDto.PhoneNumber);
        //     if (validatePhoneNumberError != null)
        //         return BadRequest(validatePhoneNumberError);
        //
        //     var sendSmsCodeError = smsConfirmationCodeManager.TrySendSmsCode(confirmationDto.PhoneNumber, 
        //                                                                      ConfirmationTypeEnum.PhoneNumber);
        //     if (sendSmsCodeError != null)
        //         return BadRequest(sendSmsCodeError);
        //
        //     return Ok();
        // }
        //
        // [HttpPost("sms/confirmation")]
        // public IActionResult ConfirmSmsCode([FromBody] SmsConfirmationDto? confirmationDto)
        // {
        //     if (confirmationDto == null)
        //         return BadRequest("Body is required");
        //     
        //     if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
        //         return BadRequest("Phone number is required");
        //
        //     var validatePhoneNumberError = phoneNumberValidator.Validate(confirmationDto.PhoneNumber);
        //     if (validatePhoneNumberError != null)
        //         return BadRequest(validatePhoneNumberError);
        //
        //     if (string.IsNullOrEmpty(confirmationDto.ConfirmationCode))
        //         return BadRequest("Confirmation code is required");
        //
        //     var confirmSmsCodeError = smsConfirmationCodeManager.TryConfirmSmsCode(confirmationDto.PhoneNumber, 
        //                                                                            ConfirmationTypeEnum.PhoneNumber, 
        //                                                                            confirmationDto.ConfirmationCode);
        //     if (confirmSmsCodeError != null)
        //         return BadRequest(confirmSmsCodeError);
        //
        //     var user = usersRepository.GetOrCreate(confirmationDto.PhoneNumber);
        //     var token = tokenProvider.GetToken(user);
        //
        //     return Ok(new AuthenticationDto
        //     {
        //         UserId = user.Id,
        //         AuthenticationToken = token,
        //     });
        // }
    }
}