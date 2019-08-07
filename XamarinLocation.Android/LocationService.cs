using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Runtime;
using Android.Util;
using XamarinLocation.Services;

namespace XamarinLocation.Droid
{
    [Service(Name = "com.companyname.xamarinlocation.LocationService",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class LocationService : JobService
    {
        public override void OnCreate()
        {
            Log.Info("LOCTEST", "OnCreate");

            base.OnCreate();
        }

        public override void OnDestroy()
        {
            Log.Info("LOCTEST", "OnDestroy");

            base.OnDestroy();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Info("LOCTEST", "OnStartCommand");

            return base.OnStartCommand(intent, flags, startId);
        }

        public override bool OnStartJob(JobParameters args)
        {
            _ = Task.Run(async () =>
            {
                Log.Info("LOCTEST", "OnStartJob - 1");
                var folder = Android.OS.Environment.ExternalStorageDirectory.ToString();
                var ts = new TrackerService(folder);
                await ts.TrackAsync();

                Log.Info("LOCTEST", "OnStartJob - 2");

                JobFinished(args, false);
            });

            return true;
        }

        public override bool OnStopJob(JobParameters args)
        {
            Log.Info("LOCTEST", "OnStopJob");
            return true;
        }
    }
}