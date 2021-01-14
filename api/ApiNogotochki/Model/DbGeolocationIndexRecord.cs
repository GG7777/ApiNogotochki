using System.ComponentModel.DataAnnotations.Schema;
using ApiNogotochki.Items;

namespace ApiNogotochki.Model
{
    [Table("geolocations_index_record")]
    public class DbGeolocationIndexRecord
    {
        [Column("target_id")]
        public string TargetId { get; set; }
        
        [Column("target_type")]
        public string TargetType { get; set; }

        [Column("geolocations", TypeName = "jsonb")]
        public Geolocation[] Geolocations { get; set; }
    }
}