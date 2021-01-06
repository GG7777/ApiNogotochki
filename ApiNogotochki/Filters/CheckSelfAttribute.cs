using System;
using ApiNogotochki.Extensions;
using ApiNogotochki.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNogotochki.Filters
{
	public class CheckSelfAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ActionArguments.TryGetValue("id", out var id))
			{
				context.Result = new BadRequestResult();
				return;
			}

			var usersRepository = context.HttpContext.RequestServices.GetService<UsersRepository>();
			var user = usersRepository.FindById((string) id);

			var currentUser = context.HttpContext.GetDbUser();
			
			if (user == null || currentUser == null || user.Id != currentUser.Id)
				context.Result = new ForbidResult();
		}
	}
}