using ApiNogotochki.Model;
using Microsoft.AspNetCore.Http;

#nullable enable

namespace ApiNogotochki.Extensions
{
	public static class HttpContextDbUserExtensions
	{
		private const string DbUser = "dbUser";
		
		public static void SetDbUser(this HttpContext context, DbUser? dbUser)
		{
			context.Items[DbUser] = dbUser;
		}

		public static DbUser? GetDbUser(this HttpContext context)
		{
			return context.Items[DbUser] as DbUser;
		}
	}
}