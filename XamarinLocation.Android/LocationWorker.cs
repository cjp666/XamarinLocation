using Android.Content;
using Android.Util;
using AndroidX.Work;
using System;
using System.Threading.Tasks;
using XamarinLocation.Services;

namespace XamarinLocation.Droid
{
    public class LocationWorker : Worker
    {
        public LocationWorker(Context context, WorkerParameters workerParameters)
            : base(context, workerParameters)
        {
        }

        public override Result DoWork()
        {
            try
            {
                Log.Info("LOCTEST", "LOCTEST - DoWork");

                Task.Run(async () =>
                {
                    var folder = Android.OS.Environment.ExternalStorageDirectory.ToString();
                    var ts = new TrackerService(folder);
                    await ts.TrackAsync("LW");
                })
                    .Wait();

                return Result.InvokeSuccess();
            }
            catch (Exception ex)
            {
                Log.Error("LOCTEST", $"LOCTEST - {ex.Message}");
                return Result.InvokeRetry();
            }
        }
    }
}