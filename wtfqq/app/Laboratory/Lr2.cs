using wtfqq.app.Laboratory.RecipeData;
using wtfqq.app.Utils;

namespace wtfqq.app.Laboratory;

public static class Lr2
{

    private static void RunTaskWithStart(int count)
    {
        MeasureTime.Start("Task using 'new Task()' and 'Start()'",
            () => { RecipeManager.SortRecipes(RecipeManager.RecipeList(count)); });
    }

    private static void RunTaskWithFactoryStartNew(int count)
    {
        MeasureTime.Start("Task using 'Task.Factory.StartNew()'",
            () =>
            {
                Task.Factory.StartNew(() => { RecipeManager.SortRecipes(RecipeManager.RecipeList(count)); }).Wait();
            });
    }

    private static void RunTaskWithResult(int count)
    {
        MeasureTime.Start("Task with Result", () =>
        {
            var task = new Task<List<Recipe>>(() => RecipeManager.SortRecipes(RecipeManager.RecipeList(count)));
            task.Start();

            var taskResult = task.Result.Count;
        });
    }

    private static void TaskWithSleep(int count)
    {
        MeasureTime.Start("Task with Sleep", () =>
        {
            var task = new Task(() =>
            {
                RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                Thread.Sleep(1000); 
            });
            task.Start();
            task.Wait();
        });
    }

    private static void TaskWithSpinWait(int count)
    {
        MeasureTime.Start("Task with SpinWait", () =>
        {
            var task = new Task(() =>
            {
                RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                Thread.SpinWait(10000000); 
            });
            task.Start();
            task.Wait();
        });
    }

    private static void TaskWithWaitTimeout(int count)
    {
        MeasureTime.Start("Task with Wait Timeout", () =>
        {
            var task = new Task(() => { RecipeManager.SortRecipes(RecipeManager.RecipeList(count)); });
            task.Start();
            var completed = task.Wait(1500); 
            Console.WriteLine(completed ? "Task completed in time." : "Task timed out.");
        });
    }

    private static void TaskWithWaitAll(int count)
    {
        MeasureTime.Start("Task with WaitAll", () =>
        {
            var task0 = new Task(() => RecipeManager.SortRecipes(RecipeManager.RecipeList(count)));
            var task1 = new Task(() => RecipeManager.SortRecipes(RecipeManager.RecipeList(count / 2)));

            task0.Start();
            task1.Start();
            Task.WaitAll(task0, task1);
        });
    }

    private static void TaskWithWaitAny(int count)
    {
        MeasureTime.Start("Task with WaitAny", () =>
        {
            var task0 = new Task(() => RecipeManager.SortRecipes(RecipeManager.RecipeList(count)));
            var task1 = new Task(() => RecipeManager.SortRecipes(RecipeManager.RecipeList(count / 2)));

            task0.Start();
            task1.Start();
            Task.WaitAny(task0, task1); 
        });
    }

    public static void TestSorting(int count)
    {
        RunTaskWithStart(count); 
        RunTaskWithFactoryStartNew(count);
        RunTaskWithResult(count); 
        TaskWithSleep(count); 
        TaskWithSpinWait(count); 
        TaskWithWaitTimeout(count); 
        TaskWithWaitAll(count); 
        TaskWithWaitAny(count); 
    }
}