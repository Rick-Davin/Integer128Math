using System.Diagnostics;

namespace DemoApp
{
    // This was the second bulk validation test written by using uint, i.e. UInt32, or an Unsigned 32-bit integer.  
    // The primary purpose was to validate the correctness of NearestIntegerRoot() method.
    internal static class ValidateUnsignedIntegers
    {
        public static void RunParallel(uint startValue = 0, uint endValue = uint.MaxValue) 
        {
            AppHelper.ShowTitle($"ValidateUnsignedIntegers: test values from {startValue:N0} to {endValue:N0}", ConsoleColor.Black, ConsoleColor.Yellow);

            var sw = Stopwatch.StartNew();
            uint mismatches = 0;
            // Parallel.For supports int or long, so we need to cast our uint values to long.
            Parallel.For((long)startValue, toExclusive: (long)endValue, new ParallelOptions { MaxDegreeOfParallelism = AppHelper.MaxDegreeOfParallelism }, i =>
            {
                TestValue<uint> test = TestValue<uint>.Create((uint)i);
                if (!test.AreRootsEqual)
                {
                    Console.WriteLine($"Mismatch for {test.Value}: {test.MathSqrt}, {test.NearestRoot}");
                    Interlocked.Increment(ref mismatches);
                }
            });
            { // a purely locally scoped block
                TestValue<uint> test = TestValue<uint>.Create(endValue);
                if (!test.AreRootsEqual)
                {
                    Console.WriteLine($"Mismatch for {test.Value}: {test.MathSqrt}, {test.NearestRoot}");
                    mismatches++;
                }
            }
            sw.Stop();
            Console.WriteLine($"DONE.   Mismatches =  {mismatches}.  Elapsed = {sw.Elapsed}\n");
        }
    }
}
