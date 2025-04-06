using wtfqq.app.Laboratory.RecipeData;
using wtfqq.app.Utils;

namespace wtfqq.app.Laboratory;

public static class Lr3
{
    private static void RunWithLock(int count)
    {
        var lockObject = new object();

        MeasureTime.Start("Task using 'lock()'", () =>
        {
            var task = new Task(() =>
            {
                lock (lockObject)
                {
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                }
            });
            task.Start();
            task.Wait();
        });
    }
    
    private static void RunWithMonitorEnter(int count)
    {
        var lockObject = new object(); 

        MeasureTime.Start("Task using 'Monitor.Enter()'", () =>
        {
            var task = new Task(() =>
            {
                Monitor.Enter(lockObject);  
                try
                {
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            });
            task.Start();
            task.Wait();
        });
    }
    
    private static void RunWithMutex(int count)
    {
        var mutex = new Mutex();
        MeasureTime.Start("Task using 'Mutex'", () =>
        {
            var task = new Task(() =>
            {
                mutex.WaitOne(); 
                try
                {
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            });
            task.Start();
            task.Wait();
        });
    }

    private static void RunWithInterlocked(int count)
    {
        var sharedValue = 0; 

        MeasureTime.Start("Task using 'Interlocked'", () =>
        {
            var task = new Task(() =>
            {
                Interlocked.Add(ref sharedValue, 1);
                RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
            });
            task.Start();
            task.Wait();
            Console.WriteLine($"Shared value after task: {sharedValue}");
        });
    }
    
    public static void TestSorting(int count)
    {
        RunWithLock(count);
        RunWithMonitorEnter(count);
        RunWithMutex(count);
        RunWithInterlocked(count);
    }

}