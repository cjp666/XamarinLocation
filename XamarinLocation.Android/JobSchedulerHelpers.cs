using Android.App.Job;
using Android.Content;

namespace XamarinLocation.Droid
{
    public static class JobSchedulerHelpers
    {
        public static JobInfo.Builder CreateJobBuilderUsingJobId<T>(this Context context, int jobId) where T : JobService
        {
            var javaClass = Java.Lang.Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);
            var ji = new JobInfo.Builder(jobId, componentName);
            return ji;
        }
    }
}