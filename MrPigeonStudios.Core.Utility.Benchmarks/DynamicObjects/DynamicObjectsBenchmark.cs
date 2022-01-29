using BenchmarkDotNet.Attributes;

namespace MrPigeonStudios.Core.Utility.Benchmarks.DynamicObjects {

    [MinColumn, MaxColumn, MemoryDiagnoser]
    public class DynamicObjectsBenchmark {
        private readonly LoadDynamicObjects.Accessors accessors;
        private readonly LoadDynamicObjects.Comparer comparer;

        public DynamicObjectsBenchmark() {
            accessors = new LoadDynamicObjects.Accessors(10000); // 1M
            comparer = new LoadDynamicObjects.Comparer(10000); // 1M
        }

        //[Benchmark] public double Accessors_Dynamic() => accessors.SumDynamicObjects();

        //[Benchmark] public double Accessors_Plain() => accessors.SumPlainObjects();

        [Benchmark] public double Distinct_WithComparer() => comparer.DistinctWithComparer();

        [Benchmark] public double Distinct_WithoutComparer() => comparer.DistinctWithoutComparer();
    }
}