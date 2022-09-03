using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSortApp.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.SlowestToFastest)]
    [RankColumn]
    [Config(typeof(Config))]
    public class FileGeneratorBanchmarks
    {
        private FileGenerator _fileGenerator = new();

        private class Config : ManualConfig
        {
            public Config()
            {
                AddJob(Job.ShortRun.WithToolchain(InProcessNoEmitToolchain.Instance));
            }
        }

        [Benchmark]
        public void GenerateFile()
        {
            var fileName = "L.txt";
            _fileGenerator.GenerateFile(500_000, fileName);
        }
    }
}
