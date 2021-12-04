using BenchmarkDotNet.Attributes;

namespace MrPigeonStudios.Core.Utility.Benchmarks.DynamicObjects {

    [MinColumn, MaxColumn, MemoryDiagnoser]
    public class DynamicObjectsBenchmark {
        private readonly LoadDynamicObjectsBase hugeLoad;
        private readonly LoadDynamicObjectsBase largeLoad;
        private readonly LoadDynamicObjectsBase mediumLoad;
        private readonly LoadDynamicObjectsBase smallLoad;

        public DynamicObjectsBenchmark() {
            smallLoad = new LoadDynamicObjectsBase(1000);    // 1K
            mediumLoad = new LoadDynamicObjectsBase(100000); // 100K
            largeLoad = new LoadDynamicObjectsBase(1000000); // 1M
            hugeLoad = new LoadDynamicObjectsBase(10000000); // 10M
        }

        [Benchmark] public double HugeLoad_Dynamic() => hugeLoad.SumDynamicObjects();

        [Benchmark] public double HugeLoad_Plain() => hugeLoad.SumPlainObjects();

        [Benchmark] public double LargeLoad_Dynamic() => largeLoad.SumDynamicObjects();

        [Benchmark] public double LargeLoad_Plain() => largeLoad.SumPlainObjects();

        [Benchmark] public double MediumLoad_Dynamic() => mediumLoad.SumDynamicObjects();

        [Benchmark] public double MediumLoad_Plain() => mediumLoad.SumPlainObjects();

        [Benchmark] public double SmallLoad_Dynamic() => smallLoad.SumDynamicObjects();

        [Benchmark] public double SmallLoad_Plain() => smallLoad.SumPlainObjects();
    }
}