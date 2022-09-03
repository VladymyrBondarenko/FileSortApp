using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.InProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace FileSortApp.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.SlowestToFastest)]
    [RankColumn]
    [Config(typeof(Config))]
    public class FileSorterBenchmarks
    {
        private FileSorter _fileSorter = new();

        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.ShortRun.WithToolchain(InProcessNoEmitToolchain.Instance));
            }
        }

        [Benchmark]
        public async Task SortFile()
        {
            var fileName = "L.txt";
            await _fileSorter.SortFile(fileName, 50_000);
        }
    }
}
