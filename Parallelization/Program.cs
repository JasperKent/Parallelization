using System.Collections.Concurrent;

var results = new double[500_000_000];
var usage = new ConcurrentDictionary<int, int>();
var resultsBag = new ConcurrentBag<double>();

//for (int i= 0; i < results.Length; ++i)
//{
//    results[i] = Factorial(i % 20 + 1);
//}

Parallel.For(0, results.Length, i =>
{
    results[i] = Factorial(i % 20 + 1);

    usage.AddOrUpdate(Environment.CurrentManagedThreadId, 1, (_, count) => count + 1);
});

var values = Enumerable.Range(0, results.Length);

//Parallel.ForEach(values, (value, _) =>
//{
//    resultsBag.Add(Factorial(value % 20 + 1));

//    usage.AddOrUpdate(Environment.CurrentManagedThreadId, 1, (_, count) => count + 1);
//});

// var linqResults = (from val in values.AsParallel() select Count(Factorial(val % 20 + 1))).Average();

Console.WriteLine(results.AsParallel().Average());

Console.WriteLine($"{usage.Count} threads used:");

foreach (var pair in usage)
    Console.WriteLine($"Thread {pair.Key} used {pair.Value} times.");

double Factorial(int n)
{
    return Enumerable.Range(1, n).Aggregate((a, b) => a * b);
}

double Count (double val)
{
    usage.AddOrUpdate(Environment.CurrentManagedThreadId, 1, (_, count) => count + 1);

    return val;
}
