using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using XamarinLocation.Services;

namespace XamarinLocation.Droid
{
    [Service]
    public class ForegroundService : Service
    {
        private static readonly string TAG = "LOCTEST";

#pragma warning disable SA1310 // Field names should not contain underscore
        private const int DELAY_BETWEEN_LOG_MESSAGES = 60000; // 1 minute as milliseconds
        private const int SERVICE_RUNNING_NOTIFICATION_ID = 10123;
        private const string BROADCAST_MESSAGE_KEY = "loctest_broadcast_message";
        private const string NOTIFICATION_BROADCAST_ACTION = "Action";

        private const string ACTION_START_SERVICE = "START_SERVICE";
        private const string ACTION_STOP_SERVICE = "STOP_SERVICE";
        private const string ACTION_RESTART_TIMER = "RESTART_TIMER";

        private const string CHANNEL_ID = "LOCTEST";
#pragma warning restore SA1310 // Field names should not contain underscore

        private bool isStarted;
        private Handler handler;
        private Action runnable;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "OnCreate: the service is initializing.");

            this.handler = new Handler();

            this.runnable = new Action(() =>
            {
                _ = Task.Run(async () =>
                {
                    PowerManager.WakeLock wakelock = null;

                    try
                    {
                        var pmanager = (PowerManager)this.GetSystemService("power");
                        wakelock = pmanager.NewWakeLock(WakeLockFlags.Partial, "loctestwakelock");

                        Log.Debug(TAG, "ForegroundService Track");
                        var folder = Android.OS.Environment.ExternalStorageDirectory.ToString();
                        var ts = new TrackerService(folder);
                        await ts.TrackAsync();

                        Intent i = new Intent(NOTIFICATION_BROADCAST_ACTION);
                        i.PutExtra(BROADCAST_MESSAGE_KEY, "LOCTEST");
                        Android.Support.V4.Content.LocalBroadcastManager
                            .GetInstance(this)
                            .SendBroadcast(i);
                    }
                    catch (Exception ex)
                    {
                        Log.Wtf(TAG, ex.Message);
                    }
                    finally
                    {
                        if (wakelock != null && wakelock.IsHeld)
                        {
                            wakelock.Release();
                        }

                        this.handler.PostDelayed(this.runnable, DELAY_BETWEEN_LOG_MESSAGES);
                    }
                });
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action.Equals(ACTION_START_SERVICE))
            {
                if (this.isStarted)
                {
                    Log.Info(TAG, "OnStartCommand: The service is already running.");
                }
                else
                {
                    Log.Info(TAG, "OnStartCommand: The service is starting.");
                    this.RegisterForegroundService();
                    this.handler.PostDelayed(this.runnable, DELAY_BETWEEN_LOG_MESSAGES);
                    this.isStarted = true;
                }
            }
            else if (intent.Action.Equals(ACTION_STOP_SERVICE))
            {
                Log.Info(TAG, "OnStartCommand: The service is stopping.");
                this.StopForeground(true);
                this.StopSelf();
                this.isStarted = false;
            }
            else if (intent.Action.Equals(ACTION_RESTART_TIMER))
            {
                Log.Info(TAG, "OnStartCommand: Restarting");
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }

        public override void OnDestroy()
        {
            // We need to shut things down.
            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

            // Stop the handler.
            this.handler.RemoveCallbacks(this.runnable);

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)this.GetSystemService(NotificationService);
            notificationManager.Cancel(SERVICE_RUNNING_NOTIFICATION_ID);

            this.isStarted = false;
            base.OnDestroy();
        }

        private void RegisterForegroundService()
        {
            var channel = new NotificationChannel(CHANNEL_ID, "xamarinlocation", NotificationImportance.None);
            channel.LockscreenVisibility = NotificationVisibility.Public;

            var notificationManager = (NotificationManager)this.GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);

            var notification = new Notification.Builder(this, CHANNEL_ID)
                .SetContentTitle("Xamarin Location Tester")
                .SetContentText("Xamarin Location Tester is running...")
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetOngoing(true)
                .Build();

            // Enlist this instance of the service as a foreground service
            this.StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }
    }
}