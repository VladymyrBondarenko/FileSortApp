
using BenchmarkDotNet.Running;
using FileSortApp;
using FileSortApp.Benchmarks;
using System.Diagnostics;

var fileName = "L.txt";

var sw = Stopwatch.StartNew();

var fileGenerator = new FileGenerator();
fileGenerator.GenerateFile(10, fileName);

var fileSorter = new FileSorter();
await fileSorter.SortFile(fileName, 2);

//BenchmarkRunner.Run<FileGeneratorBanchmarks>();
//BenchmarkRunner.Run<FileSorterBenchmarks>();

sw.Stop();
Console.WriteLine($"Execution time: {sw.Elapsed}");