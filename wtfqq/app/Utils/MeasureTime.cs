using System.Diagnostics;

namespace wtfqq.app.Utils;

public static class MeasureTime
{
    public static void Start(string label, Action action)
    {
        var sw = Stopwatch.StartNew();
        action.Invoke();
        sw.Stop();
        Console.WriteLine($"{label} completed in {sw.ElapsedMilliseconds} ms");
    }
}