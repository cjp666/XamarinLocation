using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinLocation.Services
{
    public class TrackerService
    {
        private readonly string folder;

        public TrackerService(string folder)
        {
            this.folder = folder;
        }

        public async Task TrackAsync()
        {
            Console.WriteLine("DEBUG - LOCTEST - TrackerService.Track");

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                using (var file = File.AppendText($"{this.folder}//test.csv"))
                {
                    file.WriteLine($"{DateTime.Now.ToString()}, {location.Latitude}, {location.Longitude}, Track");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
            }
        }
    }
}
