using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

namespace BenchmarkingApp
{
    public class PerformanceTest
    {
        [Params(true, false)]
        public bool UsePrecomputedList { get; set; }

        private IEnumerable<UInt128>? testValues;

        [GlobalSetup]
        public void Setup()
        {
            if (UsePrecomputedList)
            {
                testValues = GenerateValues128().ToList();    
            }
            else
            {
                testValues = GenerateValues128();
            }
        }

        private IEnumerable<UInt128> GenerateValues128()
        {
            return _generateValues(UInt128.MaxValue, iterations: 100_000, stepSize: (UInt128)1_000_000_001);
        }

        private static IEnumerable<UInt128> _generateValues(UInt128 largestValue, int iterations, UInt128 stepSize)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return largestValue;
                largestValue -= stepSize;
            }
        }

        [Benchmark(Baseline = true)]
        public UInt128 MathSqrt()
        {
            UInt128 result = 0;
            foreach (UInt128 value in testValues!)
            {
                result = (UInt128)Math.Sqrt((double)value);
            }
            return result;
        }

        [Benchmark]
        public UInt128 CustomRoot()
        {
            UInt128 result = 0;
            foreach (UInt128 value in testValues!)
            {
                result = DavinLib.Numerics.NearestIntegerRoot(value);
            }
            return result;
        }
    }
}