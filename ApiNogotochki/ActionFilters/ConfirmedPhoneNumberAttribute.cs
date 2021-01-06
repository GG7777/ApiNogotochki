using System;
using ApiNogotochki.ActionResults;
using ApiNogotochki.Enums;
using ApiNogotochki.Extensions;
using ApiNogotochki.Manager;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace ApiNogotochki.ActionFilters
{
	public class ConfirmedPhoneNumberAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var confirmationToken = context.HttpContext.Request.Headers["Confirmation"].ToString();
			if (string.IsNullOrEmpty(confirmationToken))
			{
				context.Result = new ForbidResult();
				return;
			}

			var confirmationTokenManager = context.HttpContext.RequestServices.GetRequiredService<ConfirmationTokenManager>();
			var phoneNumber = confirmationTokenManager.TryConfirm(confirmationToken, ConfirmationTypeEnum.PhoneNumber);
			if (phoneNumber == null)
			{
				context.Result = new ForbidResult();
				return;
			}

			context.HttpContext.SetConfirmedPhoneNumber(phoneNumber);
		}
	}
}