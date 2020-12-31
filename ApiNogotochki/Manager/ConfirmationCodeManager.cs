using System;
using Microsoft.Extensions.Caching.Memory;

#nullable enable

namespace ApiNogotochki.Manager
{
    public class ConfirmationCodeManager
    {
        private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        
        public string? TrySendSmsCode(string phoneNumber)
        {
            if (cache.TryGetValue(phoneNumber, out _))
                return $"Confirmation code is already send on phone number = {phoneNumber}";
            cache.Set(phoneNumber, "1337", TimeSpan.FromMinutes(5));
            return null;
        }
        
        public string? TryConfirmSmsCode(string phoneNumber, string confirmationCode)
        {
            if (!cache.TryGetValue<string>(phoneNumber, out var code))
                return $"Confirmation is not active for phone number {phoneNumber}";
            if (code != confirmationCode)
                return $"Confirmation code is not valid for phone number = {phoneNumber}";
            cache.Remove(phoneNumber);
            return null;
        }
    }
}