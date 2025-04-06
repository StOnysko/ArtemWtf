using wtfqq.app.Laboratory.RecipeData;
using wtfqq.app.Utils;

namespace wtfqq.app.Laboratory;

public static class Lr1
{
    private const int FirstTry = 100000;
    private const int SecondTry = 500000;
    private const int ThirdTry = 1000000;

    private static void SortWithThread(int count)
    {
        var recipes = RecipeManager.RecipeList(count);

        var sortingThread = new Thread(() =>
        {
            var sorted = RecipeManager.SortRecipes(RecipeManager.RecipeList(count));

            Console.WriteLine(sorted.Count + " recipes sorted (in thread)");
        });

        sortingThread.Start();
    }

    private static void TestSorting(int count)
    {
        Console.WriteLine("== Синхронне сортування ==");
        MeasureTime.Start("First Try (Sync)", () =>
        {
            var recipes = RecipeManager.RecipeList(count);
            RecipeManager.SortRecipes(recipes);
        });

        Console.WriteLine("\n== Асинхронне сортування з Thread ==");
        MeasureTime.Start("Second Try (Thread)", () => { SortWithThread(count); });
    }
    
    public static void CompareSorting()
    {
        var iterations = new List<int> { FirstTry, SecondTry, ThirdTry };
        foreach (var i in iterations)
        {
            Console.WriteLine($"\n------------ TRY WITH {i} INSTANCES --------------\n");
            TestSorting(i);
        }
    }
}