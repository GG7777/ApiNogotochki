using System;
using ApiNogotochki.ActionResults;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable enable

namespace ApiNogotochki.ActionFilters
{
	public class AccessToSelfUserAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ActionArguments.TryGetValue("id", out var id))
				throw new InvalidStateException("id should exist");

			var currentUser = context.HttpContext.TryGetUser();

			if (currentUser == null || currentUser.Id != (string) id)
				context.Result = new ForbidResult();
		}
	}
}