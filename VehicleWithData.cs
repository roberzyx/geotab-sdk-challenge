using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;

namespace GeotabChallenge
{
    //Class to wrap the device with other data like odometer measurement and position to later write it to a CSV
    public class VehicleWithData
    {
        public readonly Device Device;
        public readonly double OdometerData;

        //We accept null values because some devices could not have a location. We don't want to use 0, because that is a valida value for latitude and longitude
        public readonly double? Latitude;
        public readonly double? Longitude;

        public VehicleWithData(Device Device, double odometerData, double? latitude, double? longitude){
            Device = Device;
            OdometerData = odometerData;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}