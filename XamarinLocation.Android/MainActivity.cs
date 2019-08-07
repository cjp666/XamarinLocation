using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Util;
using Android.Content;

namespace XamarinLocation.Droid
{
    [Activity(Label = "XamarinLocation", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Intent myService;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            Log.Info("LOCTEST", $"MainActivity.OnCreate");

            this.myService = new Intent(this, typeof(ForegroundService));
            this.myService.SetAction("START_SERVICE");
            this.StartService(this.myService);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            Log.Debug("LOCTEST", "OnDestroy");

            this.StopService(this.myService);

            base.OnDestroy();
        }
    }
}