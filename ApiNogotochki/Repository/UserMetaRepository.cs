using System;
using System.Linq;
using ApiNogotochki.Model;
using JetBrains.Annotations;

namespace ApiNogotochki.Repository
{
    public class UserMetaRepository
    {
        [CanBeNull]
        public UserMeta FindById([NotNull] string userId)
        {
            using var context = new UserContext();
            return context.UserMetas.SingleOrDefault(x => x.UserId == userId);
        }

        [CanBeNull]
        public UserMeta FindByEmail([NotNull] string email)
        {
            using var context = new UserContext();
            return context.UserMetas.SingleOrDefault(x => x.Email == email);
        }

        [CanBeNull]
        public UserMeta FindByPhoneNumber([NotNull] string phoneNumber)
        {
            using var context = new UserContext();
            return context.UserMetas.SingleOrDefault(x => x.PhoneNumber == phoneNumber);
        }
        
        [NotNull]
        public UserMeta SaveNewUser([NotNull] UserMeta userMeta)
        {
            userMeta.UserId = Guid.NewGuid().ToString();
            using var context = new UserContext();
            context.UserMetas.Add(userMeta);
            context.SaveChanges();
            return userMeta;
        }
    }
}