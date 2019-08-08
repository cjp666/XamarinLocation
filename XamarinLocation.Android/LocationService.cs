using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Util;
using XamarinLocation.Services;

namespace XamarinLocation.Droid
{
    [Service(Name = "com.companyname.xamarinlocation.LocationService",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class LocationService : JobService
    {
        public override bool OnStartJob(JobParameters args)
        {
            _ = Task.Run(async () =>
            {
                Log.Debug("LOCTEST", "LOCTEST - OnStartJob - 1");

                var folder = Android.OS.Environment.ExternalStorageDirectory.ToString();
                var ts = new TrackerService(folder);
                await ts.TrackAsync("LS");

                Log.Debug("LOCTEST", "LOCTEST - OnStartJob - 2");

                JobFinished(args, false);
            });

            return true;
        }

        public override bool OnStopJob(JobParameters args)
        {
            Log.Info("LOCTEST", "LOCTEST - OnStopJob");
            return true;
        }
    }
}