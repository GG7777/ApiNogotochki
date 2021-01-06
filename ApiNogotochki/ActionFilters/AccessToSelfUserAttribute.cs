using System;
using ApiNogotochki.ActionResults;
using ApiNogotochki.Exceptions;
using ApiNogotochki.Extensions;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

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

			var usersRepository = context.HttpContext.RequestServices.GetRequiredService<UsersRepository>();
			var user = usersRepository.FindById((string) id);

			var currentUser = context.HttpContext.TryGetUser();

			if (user == null || currentUser == null || user.Id != currentUser.Id)
				context.Result = new ForbidResult();
		}
	}
}