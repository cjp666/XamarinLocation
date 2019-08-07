using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace XamarinLocation.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true, Permission = "android.permission.RECEIVE_BOOT_COMPLETED")]
    [IntentFilter(new string[] { Intent.ActionBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                Log.Debug("LOCTEST", $"BootReceiver {intent.Action}");

                Toast.MakeText(context, $"LOC - BootReceiver {intent.Action}", ToastLength.Long).Show();

                var min = JobInfo.MinPeriodMillis;
                var jobInfo = context.CreateJobBuilderUsingJobId<LocationService>(1)
                    .SetBackoffCriteria(30000, BackoffPolicy.Linear)
                    .SetPeriodic(min)
                    .SetPersisted(true)
                    .SetRequiredNetworkType(NetworkType.Any)
                    .Build();
                var jobScheduler = (JobScheduler)context.GetSystemService("jobscheduler");
                var scheduleResult = jobScheduler.Schedule(jobInfo);

                Log.Info("LOCTEST", $"BootReceiver Schedule {scheduleResult}");

                if (scheduleResult != JobScheduler.ResultSuccess)
                {
                    Toast.MakeText(context, "LOC - didn't work", ToastLength.Long).Show();
                }
            }
            catch (System.Exception ex)
            {
                Log.Error("LOCTEST", ex.Message);
                throw;
            }
        }
    }
}