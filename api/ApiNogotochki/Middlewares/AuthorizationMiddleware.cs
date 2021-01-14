using System.Threading.Tasks;
using ApiNogotochki.Extensions;
using ApiNogotochki.Manager;
using Microsoft.AspNetCore.Http;

#nullable enable

namespace ApiNogotochki.Middlewares
{
	public class AuthorizationMiddleware
	{
		private readonly RequestDelegate next;
		private readonly JwtTokenProvider tokenProvider;

		public AuthorizationMiddleware(RequestDelegate next, JwtTokenProvider tokenProvider)
		{
			this.next = next;
			this.tokenProvider = tokenProvider;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var token = context.Request.Headers["Authorization"].ToString();

			context.SetUser(tokenProvider.TryGetUser(token));

			await next(context);
		}
	}
}