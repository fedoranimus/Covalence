using System;

namespace Covalence {
    public class Haversine {
        public static double CalculateZipDistance(ZipCode zipCode1, ZipCode zipCode2) {
            if(zipCode1 == null || zipCode2 == null) {
                throw new ArgumentException("One of the zipcodes does not exist");
            }

            const double earthRadius = 3956.087107103049;

            double latitude1Radians = (zipCode1.Latitude / 180) * Math.PI;
            double longitude1Radians = (zipCode1.Longitude / 180) * Math.PI;

            double latitude2Radians = (zipCode2.Latitude / 180) * Math.PI;
            double longitude2Radians = (zipCode2.Longitude / 180) * Math.PI;

            double distance = (earthRadius * 2) *
                Math.Asin(
                    Math.Sqrt(
                        Math.Pow(
                            Math.Sin((latitude1Radians - latitude2Radians) / 2), 2) +
                        Math.Cos(latitude1Radians) *
                        Math.Cos(latitude2Radians) * 
                        Math.Pow(
                            Math.Sin((longitude1Radians - longitude2Radians) / 2), 2)
                        )
                    );

            return distance;
        }
    }
}