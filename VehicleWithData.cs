using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;

namespace GeotabChallenge
{
    public class VehicleWithData
    {
        public readonly Device Device;
        public readonly double OdometerData;
        public readonly double Latitude;
        public readonly double Longitude;

        public VehicleWithData(Device Device, double odometerData, double latitude, double longitude){
            Device = Device;
            OdometerData = odometerData;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}