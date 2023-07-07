using System;
using System.Threading.Tasks;

public static class TimedExecutionHelper
{
    //This is a multithreaded approach which might conflict when calling a monobehaviour functions directly.

    public static bool Threaded_ExitActionIfExceedsTimeLimit(TimeSpan strictTimeLimit, Action methodToExecute)
    {
        try
        {
            Task task = Task.Factory.StartNew(() => methodToExecute());
            task.Wait(strictTimeLimit);

            return task.IsCompleted;
        }
        catch (AggregateException aggregateException)
        {
            YCLogger.Error("TimedExecutionHelper", "Caught aggregate exception for ExitActionIfExceedsTimeLimit() Function",
                            "Exception Type - " + aggregateException);

            return false; //Stop the application Immediately.
        }
    }

    public delegate bool MethodToExecute();

    public static bool ExitActionIfExceedsTimeLimit(float strictTimeLimit, MethodToExecute methodToExecute)
    {
        System.Diagnostics.Stopwatch timeSpent = new System.Diagnostics.Stopwatch();
        timeSpent.Start();

        while(!methodToExecute.Invoke())
        {
            if(timeSpent.Elapsed > System.TimeSpan.FromMilliseconds(strictTimeLimit)) { return false; }
        }

        timeSpent.Stop();
        return true;
    }
}
    