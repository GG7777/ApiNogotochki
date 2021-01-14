using System;
using System.Linq;
using ApiNogotochki.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using ForbidResult = ApiNogotochki.ActionResults.ForbidResult;

#nullable enable

namespace ApiNogotochki.ActionFilters
{
	public class AuthorizeAttribute : Attribute, IActionFilter
	{
		private readonly string[] roles;
		
		public AuthorizeAttribute(params string[] roles)
		{
			this.roles = roles;
		}
		
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var dbUser = context.HttpContext.TryGetUser();
			if (dbUser == null || dbUser.Roles.All(x => !roles.Contains(x)))
				context.Result = new ForbidResult();
		}
	}
}