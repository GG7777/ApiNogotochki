using System.Threading.Tasks;
using ApiNogotochki.Extensions;
using ApiNogotochki.Manager;
using Microsoft.AspNetCore.Http;

namespace ApiNogotochki.Middlewares
{
	public class DbUserAuthenticationMiddleware
	{
		private readonly RequestDelegate next;
		private readonly JwtTokenProvider tokenProvider;

		public DbUserAuthenticationMiddleware(RequestDelegate next, JwtTokenProvider tokenProvider)
		{
			this.next = next;
			this.tokenProvider = tokenProvider;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var token = context.Request.Headers["Authentication"].ToString();
			
			context.SetDbUser(tokenProvider.TryGetUser(token));

			await next(context);
		}
	}
}