using System;
using ApiNogotochki.Extensions;
using ApiNogotochki.Manager;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNogotochki.Filters
{
	public class ConfirmationAttribute : Attribute, IActionFilter
	{
		private readonly string confirmationType;

		public ConfirmationAttribute(string confirmationType)
		{
			this.confirmationType = confirmationType;
		}

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

			var confirmationTokenManager = context.HttpContext.RequestServices.GetService<ConfirmationTokenManager>();
			var value = confirmationTokenManager.Confirm(confirmationToken, confirmationType);
			if (value == null)
			{
				context.Result = new ForbidResult();
				return;
			}
			
			context.HttpContext.SetConfirmedValue(value);
		}
	}
}