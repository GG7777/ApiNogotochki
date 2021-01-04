using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiNogotochki.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#nullable enable

namespace ApiNogotochki.Filters
{
	public class DbUserAuthorizeAttribute : Attribute, IActionFilter
	{
		private readonly string[] roles;
		
		public DbUserAuthorizeAttribute(params string[] roles)
		{
			this.roles = roles;
		}
		
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var dbUser = context.HttpContext.GetDbUser();
			if (dbUser == null || dbUser.Roles.All(x => !roles.Contains(x)))
				context.Result = new ForbidResult();
		}

		private class ForbidResult : IActionResult
		{
			public async Task ExecuteResultAsync(ActionContext context)
			{
				context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
				await context.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Сюда не хади"));
			}
		}
	}
}