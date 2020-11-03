using System;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace ApiNogotochki.Model
{
    [Table("user_geolocation")]
    public class UserGeolocation
    {
        [Column("user_id")]
        public string UserId { get; set; }
        
        [Column("latitude")]
        public double Latitude { get; set; }
        
        [Column("longitude")]
        public double Longitude { get; set; }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return UserId?.GetHashCode() ?? 0 + Latitude.GetHashCode() + Longitude.GetHashCode();
            }
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (!(obj is UserGeolocation userGeolocation))
                return false;

            if (GetHashCode() != userGeolocation.GetHashCode())
                return false;

            const double maxError = 0.0000000001;
            
            return UserId == userGeolocation.UserId &&
                   Math.Abs(Latitude - userGeolocation.Latitude) < maxError &&
                   Math.Abs(Longitude - userGeolocation.Longitude) < maxError;
        }
    }
}