using System.Diagnostics;

namespace DemoApp
{
    // This was the first bulk validation test written by using int, i.e. Int32, or a Signed 32-bit integer.  
    // The primary purpose was to validate the correctness of NearestIntegerRoot() method.
    // As signed integers will wrap around to negative values, the next validation created was for
    // unsigned integers in the ValidateUnsignedIntegers class.
    internal static class ValidateSignedIntegers
    {
        public static void RunParallel(int startValue = 0, int endValue = int.MaxValue)
        {
            AppHelper.ShowTitle($"ValidateSignedIntegers: test values from {startValue:N0} to {endValue:N0}", ConsoleColor.Black, ConsoleColor.Cyan);

            // This checks all 2.1 billion values, so its best to run in parallel.
            // Relative comparison on my 16 core machine: serial takes around 10 minutes, parallel takes slightly over 1 minute (1:17)
            var sw = Stopwatch.StartNew();
            int mismatches = 0;
            Parallel.For(startValue, toExclusive: endValue, new ParallelOptions { MaxDegreeOfParallelism = AppHelper.MaxDegreeOfParallelism }, i =>
             {
                 TestValue<int> test = TestValue<int>.Create(i);
                 if (!test.AreRootsEqual)
                 {
                     Console.WriteLine($"Mismatch for {i}: {test.MathSqrt}, {test.NearestRoot}");
                     Interlocked.Increment(ref mismatches);
                 }
             });
            {
                TestValue<int> test = TestValue<int>.Create(endValue);
                if (!test.AreRootsEqual)
                {
                    Console.WriteLine($"Mismatch for {endValue}: {test.MathSqrt}, {test.NearestRoot}");
                    mismatches++;
                }
            }
            sw.Stop();
            Console.WriteLine($"DONE.   Mismatches =  {mismatches}.  Elapsed = {sw.Elapsed}\n");
        }
    }
}
