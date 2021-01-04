using ApiNogotochki.Manager;
using ApiNogotochki.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApiNogotochki.Extensions
{
	public static class AuthenticationMiddlewareExtensions
	{
		public static void UseDbUserAuthentication(this IApplicationBuilder app)
		{
			app.UseMiddleware<DbUserAuthenticationMiddleware>(app.ApplicationServices.GetService<JwtTokenProvider>());
		}
	}
}