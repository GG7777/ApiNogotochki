using System;
using Microsoft.Extensions.Caching.Memory;

#nullable enable

namespace ApiNogotochki.Manager
{
    public class SmsConfirmationCodeManager
    {
        private readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        
        public string? TrySendSmsCode(string phoneNumber, string confirmationType)
        {
            if (cache.TryGetValue((phoneNumber, confirmationType), out _))
                return $"Confirmation code is already send on phone number = {phoneNumber}";
            
            cache.Set((phoneNumber, confirmationType), "1337", TimeSpan.FromMinutes(5));
            
            return null;
        }
        
        public string? TryConfirmSmsCode(string phoneNumber, string confirmationType, string confirmationCode)
        {
            if (!cache.TryGetValue<string>((phoneNumber, confirmationType), out var code))
                return $"Confirmation is not active for phone number {phoneNumber}";
            
            if (code != confirmationCode)
                return $"Confirmation code is not valid for phone number = {phoneNumber}";
            
            cache.Remove((phoneNumber, confirmationType));
            
            return null;
        }
    }
}