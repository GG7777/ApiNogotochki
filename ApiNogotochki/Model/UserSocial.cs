using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ApiNogotochki.Model
{
    [Table("user_social")]
    public class UserSocial
    {
        [Column("user_id")]
        public string UserId { get; set; }
        
        [Column("social_type")]
        public string SocialType { get; set; }
        
        [Column("social_mention")]
        public string SocialMention { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                return UserId?.GetHashCode() ?? 0 + SocialType?.GetHashCode() ?? 0 + SocialMention?.GetHashCode() ?? 0;
            }
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (!(obj is UserSocial userSocial))
                return false;

            if (GetHashCode() != userSocial.GetHashCode())
                return false;

            return UserId == userSocial.UserId &&
                   SocialType == userSocial.SocialType &&
                   SocialMention == userSocial.SocialMention;
        }
    }
}