using System;
using System.Linq;
using ApiNogotochki.ActionResults;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable enable

namespace ApiNogotochki.ActionFilters
{
	public class AccessToSelfServiceAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ActionArguments.TryGetValue("id", out var id))
				throw new InvalidStateException("id should exist");

			var user = context.HttpContext.TryGetUser();

			if (user?.ServiceIds == null || user.ServiceIds.All(x => x != (string) id))
				context.Result = new ForbidResult();
		}
	}
}