using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
    [Table("user_info")]
    public class UserInfo
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }
        
        [Column("avatar_id")]
        public string AvatarId { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("surname")]
        public string Surname { get; set; }
        
        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}