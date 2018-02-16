using System;
using System.Collections.Generic;
using System.Globalization;

namespace Covalence {

    public class Location : IEquatable<Location> {
        public static readonly Location Unknown = new Location();

        private double _latitude;
        private double _longitude;

        public ICollection<ApplicationUser> Users { get; set; }

        public Location() : this(double.NaN, double.NaN) {
            Users = new HashSet<ApplicationUser>();
        }

        public Location(double latitude, double longitude) {
            this._latitude = latitude;
            this._longitude = longitude;
            Users = new HashSet<ApplicationUser>();
        }

        public double Latitude
        {
            get { return _latitude; }
            set 
            {
                if(value > 90.0 || value < -90.0)
                {
                    throw new ArgumentOutOfRangeException("Latitude", "Argument must be in range of -90 to 90");
                }

                _latitude = value;
            }
        }

        public double Longitude 
        {
            get { return _longitude; }
            set
            {
                if(value > 180.0 || value < -180.0)
                {
                    throw new ArgumentOutOfRangeException("Longitude", "Argument must be in range of -180 to 180");
                }

                _longitude = value;
            }
        }

        public bool IsUnknown => Equals(Unknown);

        public bool Equals(Location other)
        {
            if(ReferenceEquals(other, null))
                return false;

            var num = Latitude;

            if(!num.Equals(other.Latitude))
                return false;

            return num.Equals(other.Longitude);
        }

        public static bool operator ==(Location left, Location right)
        {
            if(ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj) 
        {
            return Equals(obj as Location);
        }

        public override int GetHashCode() 
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public double GetDistanceTo(Location other)
        {
            if (double.IsNaN(Latitude) || double.IsNaN(Longitude) || double.IsNaN(other.Latitude) ||
                double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number");
            }

            var d1 = Latitude * (Math.PI / 180.0);
            var num1 = Longitude * (Math.PI / 180.0);
            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public override string ToString()
        {
            if (this == Unknown)
            {
                return "Unknown";
            }

            return
                $"{Latitude.ToString("G", CultureInfo.InvariantCulture)}, {Longitude.ToString("G", CultureInfo.InvariantCulture)}";
        }

    }
}