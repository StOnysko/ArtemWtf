using wtfqq.app.Laboratory.RecipeData;
using wtfqq.app.Utils;

namespace wtfqq.app.Laboratory
{
    public static class Lr4
    {
        private static void RunWithTaskFactoryContinueWhenAll(int count)
        {
            MeasureTime.Start("Task using 'ContinueWhenAll()'", () =>
            {
                var task1 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task 1 started.");
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                });

                var task2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task 2 started.");
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count / 2));
                });

                Task.Factory.ContinueWhenAll([task1, task2], tasks =>
                {
                    Console.WriteLine("Both tasks are complete. ContinueWhenAll triggered.");
                }).Wait(); 
            });
        }

        private static void RunWithTaskFactoryContinueWhenAny(int count)
        {
            MeasureTime.Start("Task using 'ContinueWhenAny()'", () =>
            {
                var task1 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task 1 started.");
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                });

                var task2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Task 2 started.");
                    RecipeManager.SortRecipes(RecipeManager.RecipeList(count / 2));
                });

                Task.Factory.ContinueWhenAny([task1, task2], _ =>
                {
                    Console.WriteLine("At least one task is complete. ContinueWhenAny triggered.");
                }).Wait(); 
            });
        }

        private static void RunWithNestedTasks(int count)
        {
            MeasureTime.Start("Task with nested tasks", () =>
            {
                var parentTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Parent Task started.");
                    var nestedTask1 = Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine("Nested Task 1 started.");
                        RecipeManager.SortRecipes(RecipeManager.RecipeList(count));
                    }, TaskCreationOptions.AttachedToParent);

                    var nestedTask2 = Task.Factory.StartNew(() =>
                    {
                        Console.WriteLine("Nested Task 2 started.");
                        RecipeManager.SortRecipes(RecipeManager.RecipeList(count / 2));
                    }, TaskCreationOptions.AttachedToParent);
                });

                parentTask.Wait();
            });
        }

        public static void TestSorting(int count)
        {
            RunWithTaskFactoryContinueWhenAll(count);
            RunWithTaskFactoryContinueWhenAny(count);
            RunWithNestedTasks(count);
        }
    }
}