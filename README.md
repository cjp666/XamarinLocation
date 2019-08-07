# XamarinLocation #
Test Xamarin App to perform location tracking using a service

After much research it appears that each new version of Android has further restrictions on what can be done in the background

It now appears that the [JobScheduler](https://developer.android.com/reference/android/app/job/JobScheduler.html) must be used, this is passed a [JobInfo.Builder](https://developer.android.com/reference/kotlin/android/app/job/JobInfo.Builder)

## Links: ##
* [WorkManager](https://developer.android.com/topic/libraries/architecture/workmanager)
* [Android Support / AndroidX for Xamarin.Android](https://github.com/xamarin/AndroidSupportComponents)
* [Guide to background processing](https://developer.android.com/guide/background)
* [android.app.job](https://developer.android.com/reference/kotlin/android/app/job/package-summary)
* [Testing Doze](https://developer.android.com/training/monitoring-device-state/doze-standby.html#testing_doze)

## Restrictions ##
* Periodic tasks have a minimum period of 15 minutes
