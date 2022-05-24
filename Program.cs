using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;


namespace GeotabChallenge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting backup proccess...");

                if (args.Count() != 4)
                {
                    throw new ArgumentException("Missing arguments. The format should be: dotnet.exe run -- user password database server");
                }

                string user = args[0];
                string password = args[1];
                string server = args[2];
                string database = args[3];

                //Authenticating
                Console.WriteLine("Creating API connection...");

                var api = new API(user, password, null, database, server);
                await api.AuthenticateAsync();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Succesfully authenticated");
                Console.ForegroundColor = ConsoleColor.White;

                IList<VehicleWithData> vehiclesWithData = new List<VehicleWithData>();                

                Console.WriteLine("Calling API to get the devices");

                var devices = await api.CallAsync<IList<Device>>("Get", typeof(Device));

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("GET devices call OK");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Device list: \r\n");

                foreach (var device in devices)
                {
                    //Searching the status data for each device to get the odometer measurement
                    var statusDataSearch = new StatusDataSearch
                    {
                        DeviceSearch = new DeviceSearch(device.Id),
                        DiagnosticSearch = new DiagnosticSearch(KnownId.DiagnosticOdometerAdjustmentId),
                        FromDate = DateTime.MaxValue
                    };

                    IList<StatusData> statusData = await api.CallAsync<IList<StatusData>>("Get", typeof(StatusData), new { search = statusDataSearch });

                    var odometerData = statusData[0]?.Data ?? 0;

                    //Search for the DeviceStatusInfo to get the coordinates
                    var deviceStatusInfoSearch = new DeviceStatusInfoSearch
                    {
                        DeviceSearch = new DeviceSearch(device.Id)
                    };

                    IList<DeviceStatusInfo> deviceStatusInfo = await api.CallAsync<IList<DeviceStatusInfo>>("Get", typeof(DeviceStatusInfo), new { search = deviceStatusInfoSearch });


                    Console.WriteLine($"ID: {device.Id}");
                    Console.WriteLine($"Name: {device.Name}");
                    Console.WriteLine($"Type: {device.DeviceType}");
                    Console.WriteLine($"Timestamp: {DateTime.Now}");
                    Console.WriteLine($"Odometer: {Math.Round(odometerData / 1000)} KMS");

                    
                    if (deviceStatusInfo.Count > 0)
                    {                        
                        Console.WriteLine($"Latitude: {deviceStatusInfo[0].Latitude}");
                        Console.WriteLine($"Longitude: {deviceStatusInfo[0].Longitude}");
                    }
                    else
                    {
                        Console.WriteLine("Coordinates not found");
                    }

                    //If there is no coordinates data, we pass a null value
                    vehiclesWithData.Add(new VehicleWithData(device, odometerData, deviceStatusInfo[0].Latitude ?? null, deviceStatusInfo[0].Longitude ?? null));

                    Console.WriteLine("-----------------------------------------------------");
                }

                WriteCsvFiles(vehiclesWithData, "backup", DateTime.UtcNow);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message, e.StackTrace);
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Console.WriteLine();
                Console.Write("End of the backup. Press any key to close the console...");
                Console.ReadKey(true);
            }
        }

        //Writes a CSV for each Vehicle, appending it to the already written data if there is some
        static void WriteCsvFiles(IEnumerable<VehicleWithData> vehicles, string path, DateTime utcDate)
        {
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"Writing backup for vehicle {vehicle.Device.Id}");
                using (var writer = new StreamWriter($"{path}/{vehicle.Device.Name}.csv"))
                {
                    writer.WriteLine(
                        $"{vehicle.Device.Id};" +
                        $"{vehicle.Device.Name};" +
                        $"{vehicle.Device.DeviceType};" +
                        $"{utcDate};" +
                        $"{vehicle.OdometerData};" +
                        $"{vehicle.Latitude};" +
                        $"{vehicle.Longitude}"
                    );
                }
                Console.WriteLine("File writing OK");
            }
        }
    }
}