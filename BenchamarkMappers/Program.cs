using System;

namespace BenchmarkMappers
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CassandraBenchmark>();
        }
    }
}
