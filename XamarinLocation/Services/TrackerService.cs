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
            try
            {
                Console.WriteLine($"DEBUG - LOCTEST - Track 1");
                var timeout = new TimeSpan(0, 1, 0);
                var request = new GeolocationRequest(GeolocationAccuracy.Best, timeout);

                Console.WriteLine($"DEBUG - LOCTEST - Track 2");
                var location = await Geolocation.GetLocationAsync(request);

                Console.WriteLine($"DEBUG - LOCTEST - Track 3");
                var bl = Battery.ChargeLevel * 100;
                var m = $"{DateTime.Now.ToString()}, {location.Latitude}, {location.Longitude}, Track, {bl}";
                Console.WriteLine($"DEBUG - LOCTEST - {m}");

                Console.WriteLine($"DEBUG - LOCTEST - Track 4");
                using (var file = File.AppendText($"{this.folder}//test.csv"))
                {
                    file.WriteLine(m);
                }

                Console.WriteLine($"DEBUG - LOCTEST - Track 5");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - LOCTEST - {ex.Message}");
            }
        }
    }
}
