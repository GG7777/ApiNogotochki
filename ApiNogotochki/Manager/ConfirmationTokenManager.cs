using System;
using Microsoft.Extensions.Caching.Memory;

#nullable enable

namespace ApiNogotochki.Manager
{
	public class ConfirmationTokenManager
	{
		private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

		public string Create(string type, string value)
		{
			var token = Guid.NewGuid().ToString();

			cache.Set((token, type), value, TimeSpan.FromMinutes(5));

			return token;
		}

		public string? TryConfirm(string token, string type)
		{
			if (!cache.TryGetValue<string>((token, type), out var value))
				return null;

			cache.Remove((token, type));

			return value;
		}
	}
}