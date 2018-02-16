using System.Collections.Generic;

namespace Covalence.Contracts
{

    public class LocationContract
    {
        public LocationContract() {
            Latitude = double.NaN;
            Longitude = double.NaN;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}