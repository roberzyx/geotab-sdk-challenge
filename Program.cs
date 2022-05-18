using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;

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

            var devices = await api.CallAsync<IList<CustomVehicleDevice>>("Get", typeof(CustomVehicleDevice));

            foreach (var device in devices){
                Console.WriteLine(device.Id);
                Console.WriteLine(device.LicensePlate);
                Console.WriteLine(device.VehicleIdentificationNumber);
                Console.WriteLine(device.Odometer);
                Console.WriteLine("-----------------------------------------------------");
            }
        }
    }
}