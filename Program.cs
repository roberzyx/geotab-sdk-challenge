using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;

namespace GeotabChallenge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string user = "robertoruiz@geotab-challenge.com";
            string password = "GP7VUeCUTv9WCqh$";
            string server = "mypreview.geotab.com";

            var api = new API(user, password, null, "Demo_robertoruiz_challenge", server);
            await api.AuthenticateAsync();

            var devices = await api.CallAsync<IList<Device>>("Get", typeof(Device));

            foreach (var device in devices)
            {
                // Console.WriteLine(device.Id);
                // Console.WriteLine(device.LicensePlate);
                // Console.WriteLine(device.VehicleIdentificationNumber);
                // Console.WriteLine(device.Odometer);
                Console.WriteLine(device.ToString());

                var statusDataSearch = new StatusDataSearch
                {
                    DeviceSearch = new DeviceSearch(device.Id),
                    DiagnosticSearch = new DiagnosticSearch(KnownId.DiagnosticOdometerAdjustmentId),
                    FromDate = DateTime.MaxValue
                };

                IList<StatusData> statusData = await api.CallAsync<IList<StatusData>>("Get", typeof(StatusData), new { search = statusDataSearch});

                var odometerReading = statusData[0]?.Data ?? 0;

                foreach (var x in statusData){
                    Console.WriteLine(x.ToString());
                }

                Console.WriteLine(odometerReading);

                CustomVehicleDevice customVehicleDevice = await api.CallAsync<CustomVehicleDevice>("Get", typeof(CustomVehicleDevice), new { search = new DeviceSearch(device.Id)});

                Console.WriteLine(customVehicleDevice.LicensePlate);

                Console.WriteLine("-----------------------------------------------------");
            }
        }
    }
}