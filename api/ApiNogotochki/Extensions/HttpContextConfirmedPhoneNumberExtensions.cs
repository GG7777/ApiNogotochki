using Microsoft.AspNetCore.Http;

#nullable enable

namespace ApiNogotochki.Extensions
{
	public static class HttpContextConfirmedPhoneNumberExtensions
	{
		private const string ConfirmedPhoneNumber = "confirmedPhoneNumber";

		public static void SetConfirmedPhoneNumber(this HttpContext context, string confirmedPhoneNumber)
		{
			context.Items[ConfirmedPhoneNumber] = confirmedPhoneNumber;
		}

		public static string? TryGetConfirmedPhoneNumber(this HttpContext context)
		{
			return context.Items.TryGetValue(ConfirmedPhoneNumber, out var phoneNumber)
					   ? phoneNumber as string
					   : null;
		}
	}
}