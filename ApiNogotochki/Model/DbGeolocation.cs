using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNogotochki.Model
{
    [Table("geolocations")]
    public class DbGeolocation
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("target_id")]
        public string TargetId { get; set; }
        
        [Column("target_type")]
        public string TargetType { get; set; }
        
        [Column("latitude")]
        public double Latitude { get; set; }
        
        [Column("longitude")]
        public double Longitude { get; set; }
    }
}