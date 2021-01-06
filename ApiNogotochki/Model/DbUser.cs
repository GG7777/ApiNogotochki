using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiNogotochki.Items;
using ApiNogotochki.Services.Items;

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
        
        [Index(IsUnique = true)]
        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("name")]
        public string Name { get; set; }
        
        [Column("avatar_id")]
        public string AvatarId { get; set; }
        
        [Column("description")]
        public string Description { get; set; }

        [Column("service_ids")]
        public string[] ServiceIds { get; set; }
        
        [Column("social_networks", TypeName = "jsonb")]
        public SocialNetwork[] SocialNetworks { get; set; }

        [Column("geolocations", TypeName = "jsonb")]
        public Geolocation[] Geolocations { get; set; }
    }
}