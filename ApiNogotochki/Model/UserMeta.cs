using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
    [Table("user_meta")]
    public class UserMeta
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }
        
        [Index(IsUnique = true)]
        [Column("email")]
        public string Email { get; set; }
        
        [Index(IsUnique = true)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}