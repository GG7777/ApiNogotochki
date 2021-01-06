using ApiNogotochki.Manager;
using ApiNogotochki.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

#nullable enable

namespace ApiNogotochki.Extensions
{
	public static class AuthorizationMiddlewareExtensions
	{
		public static void UseCustomAuthorization(this IApplicationBuilder app)
		{
			app.UseMiddleware<AuthorizationMiddleware>(app.ApplicationServices.GetService<JwtTokenProvider>());
		}
	}
}