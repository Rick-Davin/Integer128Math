using BenchmarkDotNet.Running;
using BenchmarkingApp;

var summary = BenchmarkRunner.Run<PerformanceTest>();

Console.WriteLine("\nDONE!  Press ENTER key to exit...");
Console.ReadLine();
