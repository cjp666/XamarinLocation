using Android.Content;
using Android.Util;
using AndroidX.Work;
using System;

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