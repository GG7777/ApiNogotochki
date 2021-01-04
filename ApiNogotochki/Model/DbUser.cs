using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
    [Table("users")]
    public class DbUser
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("roles")]
        public string[] Roles { get; set; }
        
        [Column("is_removed")]
        public bool IsRemoved { get; set; }
        
        [Index(IsUnique = true)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        
        [Column("nickname")]
        public string Nickname { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("avatar_id")]
        public string AvatarId { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column("social_network_ids")]
        public string[] SocialNetworkIds { get; set; }
        
        [Column("service_ids")]
        public string[] ServiceIds { get; set; }
    }
}