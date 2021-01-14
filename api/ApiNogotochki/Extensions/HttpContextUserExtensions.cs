using ApiNogotochki.Model;
using Microsoft.AspNetCore.Http;

#nullable enable

namespace ApiNogotochki.Extensions
{
	public static class HttpContextUserExtensions
	{
		private const string User = "User";

		public static void SetUser(this HttpContext context, DbUser? dbUser)
		{
			context.Items[User] = dbUser;
		}

		public static DbUser? TryGetUser(this HttpContext context)
		{
			return context.Items.TryGetValue(User, out var user)
					   ? user as DbUser
					   : null;
		}
	}
}