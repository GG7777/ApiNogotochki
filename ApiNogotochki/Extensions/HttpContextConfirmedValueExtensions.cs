using Microsoft.AspNetCore.Http;

#nullable enable

namespace ApiNogotochki.Extensions
{
	public static class HttpContextConfirmedValueExtensions
	{
		private const string ConfirmedValue = "confirmedValue";

		public static void SetConfirmedValue(this HttpContext context, string value)
		{
			context.Items[ConfirmedValue] = value;
		}

		public static string? GetConfirmedValue(this HttpContext context)
		{
			return context.Items[ConfirmedValue] as string;
		}
	}
}