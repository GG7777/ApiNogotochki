﻿using System.Linq;
using ApiNogotochki.Dto;
using ApiNogotochki.Enums;
using ApiNogotochki.Manager;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace ApiNogotochki.Controllers
{
	[Controller]
	[Route("authorization")]
    public class AuthorizationController : ControllerBase
	{
        private readonly SmsConfirmationCodeManager smsConfirmationCodeManager;
        private readonly UsersRepository usersRepository;
        private readonly JwtTokenProvider tokenProvider;

        public AuthorizationController(SmsConfirmationCodeManager smsConfirmationCodeManager, 
                                       UsersRepository usersRepository, 
                                       JwtTokenProvider tokenProvider)
        {
            this.smsConfirmationCodeManager = smsConfirmationCodeManager;
            this.usersRepository = usersRepository;
            this.tokenProvider = tokenProvider;
        }
        
        [HttpPost("sms-sending")]
        public IActionResult SendConfirmationSmsCode([FromBody] SmsConfirmationDto? confirmationDto)
        {
            if (confirmationDto == null)
                return BadRequest("Body is required");
            
            if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
                return BadRequest("Phone number is required");

            var validatePhoneNumberError = ValidatePhoneNumber(confirmationDto.PhoneNumber);
            if (validatePhoneNumberError != null)
                return BadRequest(validatePhoneNumberError);

            var sendSmsCodeError = smsConfirmationCodeManager.TrySendSmsCode(confirmationDto.PhoneNumber, 
                                                                             ConfirmationTypeEnum.Authorization);
            if (sendSmsCodeError != null)
                return BadRequest(sendSmsCodeError);

            return Ok();
        }

        [HttpPost("sms-confirmation")]
        public IActionResult ConfirmSmsCode([FromBody] SmsConfirmationDto? confirmationDto)
        {
            if (confirmationDto == null)
                return BadRequest("Body is required");
            
            if (string.IsNullOrEmpty(confirmationDto.PhoneNumber))
                return BadRequest("Phone number is required");

            var validatePhoneNumberError = ValidatePhoneNumber(confirmationDto.PhoneNumber);
            if (validatePhoneNumberError != null)
                return BadRequest(validatePhoneNumberError);

            if (string.IsNullOrEmpty(confirmationDto.ConfirmationCode))
                return BadRequest("Confirmation code is required");

            var confirmSmsCodeError = smsConfirmationCodeManager.TryConfirmSmsCode(confirmationDto.PhoneNumber, 
                                                                                   ConfirmationTypeEnum.Authorization, 
                                                                                   confirmationDto.ConfirmationCode);
            if (confirmSmsCodeError != null)
                return BadRequest(confirmSmsCodeError);

            var user = usersRepository.GetOrCreate(confirmationDto.PhoneNumber);
            var token = tokenProvider.GetToken(user);

            return Ok(new AuthenticationDto
            {
                UserId = user.Id,
                AuthenticationToken = token,
            });
        }

        private string? ValidatePhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Any(x => !char.IsDigit(x)))
                return "Phone number should contain only digits";

            if (phoneNumber.Length != 11)
                return "Phone number should contain only 11 digits";

            if (!phoneNumber.StartsWith("79"))
                return "Phone number should start with '79'";

            return null;
        }
    }
}