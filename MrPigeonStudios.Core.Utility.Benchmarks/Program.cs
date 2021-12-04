using BenchmarkDotNet.Running;

namespace MrPigeonStudios.Core.Utility.Benchmarks {

    internal class Program {

        private static void Main(string[] args) {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}