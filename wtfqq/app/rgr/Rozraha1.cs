namespace wtfqq.app.Rgr;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

// Клас для представлення історичної події
public class HistoricalEvent
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public List<string> Participants { get; set; }
    public string Consequences { get; set; }

    public HistoricalEvent()
    {
        Participants = new List<string>();
    }

    public override string ToString()
    {
        return $"Дата: {Date.ToShortDateString()}, Назва: {Name}, Місце: {Location}\n" +
               $"Опис: {Description}\n" +
               $"Учасники: {string.Join(", ", Participants)}\n" +
               $"Наслідки: {Consequences}\n";
    }
}

// Клас для роботи з історичними подіями
public class HistoricalEventsProcessor
{
    private List<HistoricalEvent> events;

    public HistoricalEventsProcessor()
    {
        events = new List<HistoricalEvent>();
    }

    // Генерація випадкових історичних подій для тестування
    public void GenerateRandomEvents(int count)
    {
        string[] eventNames = { "Битва", "Підписання договору", "Революція", "Коронація", "Відкриття", "Заснування" };
        string[] locations = { "Київ", "Львів", "Харків", "Париж", "Лондон", "Рим", "Берлін", "Нью-Йорк" };
        string[] descriptions =
        {
            "Важлива історична подія", "Значна подія в історії", "Вирішальний момент", "Переломний момент в історії"
        };
        string[] participants =
            { "Королі", "Президенти", "Генерали", "Науковці", "Політичні діячі", "Громадські активісти" };
        string[] consequences =
        {
            "Зміна політичного устрою", "Культурне піднесення", "Економічне зростання", "Початок війни",
            "Мирний договір"
        };

        Random random = new Random();
        events.Clear();

        for (int i = 0; i < count; i++)
        {
            HistoricalEvent ev = new HistoricalEvent
            {
                Date = new DateTime(random.Next(1700, 2023), random.Next(1, 13), random.Next(1, 29)),
                Name = $"{eventNames[random.Next(eventNames.Length)]} {random.Next(1, 100)}",
                Description = descriptions[random.Next(descriptions.Length)],
                Location = locations[random.Next(locations.Length)],
                Consequences = consequences[random.Next(consequences.Length)]
            };

            // Додаємо випадкову кількість учасників
            int participantsCount = random.Next(1, 4);
            for (int j = 0; j < participantsCount; j++)
            {
                ev.Participants.Add(participants[random.Next(participants.Length)]);
            }

            events.Add(ev);
        }
    }

    // Послідовне сортування подій за датою
    public List<HistoricalEvent> SortByDateSequential()
    {
        return events.OrderBy(e => e.Date).ToList();
    }

    // Послідовне сортування подій за назвою
    public List<HistoricalEvent> SortByNameSequential()
    {
        return events.OrderBy(e => e.Name).ToList();
    }

    // Послідовне сортування подій за місцем
    public List<HistoricalEvent> SortByLocationSequential()
    {
        return events.OrderBy(e => e.Location).ToList();
    }

    // Паралельне сортування подій за датою
    public List<HistoricalEvent> SortByDateParallel()
    {
        return events.AsParallel().OrderBy(e => e.Date).ToList();
    }

    // Паралельне сортування подій за назвою
    public List<HistoricalEvent> SortByNameParallel()
    {
        return events.AsParallel().OrderBy(e => e.Name).ToList();
    }

    // Паралельне сортування подій за місцем
    public List<HistoricalEvent> SortByLocationParallel()
    {
        return events.AsParallel().OrderBy(e => e.Location).ToList();
    }

    // Зберегти події у файл
    public void SaveEventsToFile(List<HistoricalEvent> eventsToSave, string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var ev in eventsToSave)
            {
                writer.WriteLine(ev.ToString());
                writer.WriteLine("--------------------------------------------------");
            }
        }
    }
}

public static class Rozraha1
{
    public static void Calc()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Використовуємо одне ціле число для розміру даних
        const int dataSize = 1000000;

        Console.WriteLine($"Тестування на {dataSize} історичних подіях");
        Console.WriteLine("----------------------------------------");

        HistoricalEventsProcessor processor = new HistoricalEventsProcessor();
        processor.GenerateRandomEvents(dataSize);

        // Тестування послідовної версії
        Console.WriteLine("Послідовна версія:");
        TestSequentialProcessing(processor);

        // Тестування паралельної версії
        Console.WriteLine("\nПаралельна версія:");
        TestParallelProcessing(processor);

        Console.WriteLine("\n");

        // Демонстрація сортування та збереження у файл
        HistoricalEventsProcessor demoProcessor = new HistoricalEventsProcessor();
        demoProcessor.GenerateRandomEvents(20);

        var sortedByDate = demoProcessor.SortByDateSequential();
        demoProcessor.SaveEventsToFile(sortedByDate, "events_sorted_by_date.txt");

        var sortedByName = demoProcessor.SortByNameSequential();
        demoProcessor.SaveEventsToFile(sortedByName, "events_sorted_by_name.txt");

        var sortedByLocation = demoProcessor.SortByLocationSequential();
        demoProcessor.SaveEventsToFile(sortedByLocation, "events_sorted_by_location.txt");

        Console.WriteLine("Демонстраційні файли збережено.");
        Console.WriteLine("Натисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }

    static void TestSequentialProcessing(HistoricalEventsProcessor processor)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Сортування за датою
        stopwatch.Restart();
        var sortedByDate = processor.SortByDateSequential();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за датою: {stopwatch.ElapsedMilliseconds} мс");

        // Сортування за назвою
        stopwatch.Restart();
        var sortedByName = processor.SortByNameSequential();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за назвою: {stopwatch.ElapsedMilliseconds} мс");

        // Сортування за місцем
        stopwatch.Restart();
        var sortedByLocation = processor.SortByLocationSequential();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за місцем: {stopwatch.ElapsedMilliseconds} мс");
    }

    static void TestParallelProcessing(HistoricalEventsProcessor processor)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Сортування за датою
        stopwatch.Restart();
        var sortedByDate = processor.SortByDateParallel();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за датою: {stopwatch.ElapsedMilliseconds} мс");

        // Сортування за назвою
        stopwatch.Restart();
        var sortedByName = processor.SortByNameParallel();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за назвою: {stopwatch.ElapsedMilliseconds} мс");

        // Сортування за місцем
        stopwatch.Restart();
        var sortedByLocation = processor.SortByLocationParallel();
        stopwatch.Stop();
        Console.WriteLine($"Сортування за місцем: {stopwatch.ElapsedMilliseconds} мс");
    }
}