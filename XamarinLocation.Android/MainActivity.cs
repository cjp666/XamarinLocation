using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Util;
using Android.App.Job;
using Android.Widget;
using System;
using AndroidX.Work;

namespace XamarinLocation.Droid
{
    [Activity(Label = "XamarinLocation", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            Log.Info("LOCTEST", $"LOCTEST - MainActivity.OnCreate");

            var jobInfo = this.CreateJobBuilderUsingJobId<LocationService>(1)
                .SetBackoffCriteria(30000, Android.App.Job.BackoffPolicy.Linear)
                .SetPeriodic(JobInfo.MinPeriodMillis)
                .SetPersisted(true)
                .SetRequiredNetworkType(Android.App.Job.NetworkType.Any)
                .Build();
            var jobScheduler = (JobScheduler)this.GetSystemService("jobscheduler");
            var scheduleResult = jobScheduler.Schedule(jobInfo);

            Log.Info("LOCTEST", $"LOCTEST - Schedule {scheduleResult}");

            if (scheduleResult != JobScheduler.ResultSuccess)
            {
                Log.Error("LOCTEST", "LOCTEST - failed to schedule!");
                Toast.MakeText(this, "LOC - didn't work", ToastLength.Long).Show();
            }

            var locationWorker = PeriodicWorkRequest.Builder
                .From<LocationWorker>(TimeSpan.FromMinutes(15))
                .Build();
            WorkManager.Instance.Enqueue(locationWorker);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            Log.Debug("LOCTEST", "LOCTEST - OnDestroy");

            base.OnDestroy();
        }
    }
}