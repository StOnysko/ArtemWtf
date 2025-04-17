namespace wtfqq.app.Rgr;


using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Rozraha1
{
    public static void Start()
    {
        const int passwordCount = 1000000;
        const int passwordLength = 12;

        Console.WriteLine("Random Password Generation & Strength Checking");
        Console.WriteLine($"Total passwords: {passwordCount:N0}, Length: {passwordLength}");

        // 1. Sequential
        Console.WriteLine("\ndefault algorithm");
        var sequentialValidCount = MeasureExecutionTime(() =>
            CheckPasswordsSequential(passwordCount, passwordLength), out var sequentialTime);
        Console.WriteLine($"   Valid passwords: {sequentialValidCount:N0}");
        Console.WriteLine($"   Time: {sequentialTime:F3} ms");

        // 2. Parallel (Tasks)
        Console.WriteLine("\n using Task");
        var taskValidCount = MeasureExecutionTime(() =>
            CheckPasswordsParallelTask(passwordCount, passwordLength), out var taskTime);
        Console.WriteLine($"   Valid passwords: {taskValidCount:N0}");
        Console.WriteLine($"   Time: {taskTime:F3} ms");

        // 3. Parallel (Parallel.For)
        Console.WriteLine("\n using Parallel");
        var parallelValidCount = MeasureExecutionTime(() =>
            CheckPasswordsParallelFor(passwordCount, passwordLength), out var parallelTime);
        Console.WriteLine($"   Valid passwords: {parallelValidCount:N0}");
        Console.WriteLine($"   Time: {parallelTime:F3} ms");
    }

    // Password complexity: must contain uppercase, lowercase, digit, special character
    private static bool IsPasswordStrong(string password)
    {
        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(c => "!@#$%^&*".Contains(c));
    }

    private static string GeneratePassword(int length, Random rng)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
        var sb = new StringBuilder(length);
        for (var i = 0; i < length; i++)
            sb.Append(chars[rng.Next(chars.Length)]);
        return sb.ToString();
    }

    private static int CheckPasswordsSequential(int count, int length)
    {
        var rng = new Random();
        var validCount = 0;

        for (var i = 0; i < count; i++)
        {
            var password = GeneratePassword(length, rng);
            if (IsPasswordStrong(password)) validCount++;
        }

        return validCount;
    }

    private static int CheckPasswordsParallelTask(int count, int length)
    {
        var coreCount = Environment.ProcessorCount;
        var perTask = count / coreCount;
        var tasks = new Task<int>[coreCount];

        for (var i = 0; i < coreCount; i++)
        {
            var taskIndex = i;
            tasks[i] = Task.Run(() =>
            {
                var rng = new Random(Guid.NewGuid().GetHashCode());
                var localValid = 0;
                var localCount = taskIndex == coreCount - 1 ? perTask + count % coreCount : perTask;

                for (var j = 0; j < localCount; j++)
                {
                    var password = GeneratePassword(length, rng);
                    if (IsPasswordStrong(password)) localValid++;
                }

                return localValid;
            });
        }

        Task.WaitAll(tasks);
        return tasks.Sum(t => t.Result);
    }

    private static int CheckPasswordsParallelFor(int count, int length)
    {
        var validCount = 0;
        object lockObj = new();

        Parallel.For(0, count, () => { return new Random(Guid.NewGuid().GetHashCode()); },
            (i, _, rng) =>
            {
                var password = GeneratePassword(length, rng);
                if (!IsPasswordStrong(password)) return rng;
                lock (lockObj)
                {
                    validCount++;
                }
                return rng;
            },
            _ => { });

        return validCount;
    }

    private static T MeasureExecutionTime<T>(Func<T> func, out double elapsedMs)
    {
        var sw = Stopwatch.StartNew();
        var result = func();
        sw.Stop();
        elapsedMs = sw.Elapsed.TotalMilliseconds;
        return result;
    }
}