using System.Diagnostics;

namespace DemoApp
{
    // This was the third bulk validation test written by using UInt128.
    // It cannot be an exahustive test because there are 2^128 (over 340 undecillion) possible values.
    // Instead, this test focuses on values around powers of 2, with plus/minus an offset.
    // For very large values, the Math.Sqrt method becomes less precise, so some discrepancies are expected.
    // But for our NearestIntegerRoot method, ZERO discrepancies are tolerated - in other words, this calculation
    // demands correctness over performance.
    internal class ValidatePowersOf2
    {
        public static void ShowBadPowersOf2()
        {
            // Running the analysis 3 times on increasing powers of 2 to demonstrate that once a mismatch is found that a larger number may match again.
            // "Bad" means the computed square roots are not equal.
            AppHelper.ShowTitle("See which powers of 2 may produce different square roots ...", ConsoleColor.White, ConsoleColor.DarkRed);
            Dictionary<int, TestValue<UInt128>> dict = new Dictionary<int, TestValue<UInt128>>();
            for (int bit = 0; bit < 128; bit++)
            {
                TestValue<UInt128> test = TestValue<UInt128>.Create(UInt128.One << bit);
                if (!test.AreRootsEqual)
                {
                    dict.Add(bit, test);
                }
            }
            Console.WriteLine($"\tMismatch Count = {dict.Count}.");
            Console.WriteLine($"\tBit\tValue");
            foreach (var entry in dict)
            {
                Console.WriteLine($"\t{entry.Key} \t{entry.Value}");
            }
            Console.WriteLine("Consider bit 105.  The next bit 106 is good, so there is no clear cutoff value to separate good versus bad.");
            Console.WriteLine("Math.Sqrt loses precision not due to the magnitude of the number but to the significant digits.");
            Console.WriteLine("100_000_000_000_000_000_000_000_000_000_000_000_000 is huge with 39 digits but only has 1 significant digit.");
            Console.WriteLine("One would expect no loss of precision in that case.");
            Console.Write("Or rather given a value of a 1 followed by 38 zeroes, we would expect the square root to be a 1 followed by 19 zeroes.");

            var big = UInt128.Parse("100_000_000_000_000_000_000_000_000_000_000_000_000".Replace("_", ""));
            AnalyzeOneValue.Run(big);
        }

        public static void RunWithOffset(int offset = 1000)
        {
            AppHelper.ShowTitle($"VALIDATE Around Powers of 2 using Offset: {offset}", ConsoleColor.Black, ConsoleColor.Green);

            var sw = Stopwatch.StartNew();
            
            IEnumerable<UInt128> values = GenerateValues((UInt128)offset);

            int total = 0;
            int mismatches = 0;  
            int badSqrtCount = 0;
            int badRootCount = 0;

            foreach (UInt128 value in values) 
            {
                total++;  
                TestValue<UInt128> test = TestValue<UInt128>.Create(value);
                if (!test.AreRootsEqual)
                {
                   mismatches++;
                   if (!test.MathSqrt.IsGood)
                    {
                       // I expect for Math.Sqrt to be wrong and less precise as the numbers get larger towards teh 128 bit limit.
                       // Therefore, I do not need to log these.
                       badSqrtCount++;
                    }
                   else if (!test.NearestRoot.IsGood)
                   {
                       // My main concern is the correctness of my NearestIntegerRoot implementation, so log these.
                       // However, this really should NEVER be triggered.
                       Console.WriteLine($"NearestRoot incorrect {value}.");
                       badRootCount++;
                   }
                }
            }
            sw.Stop();
            Console.WriteLine($"\tValue Count: {total}, completed in {sw.Elapsed}");
            Console.WriteLine($"\tTotal Mismatches = {mismatches} ... some are expected");
            Console.WriteLine($"\tBad double  Sqrt = {badSqrtCount} ... some are expected");
            Console.WriteLine($"\tBad UInt128 Root = {badRootCount} ... NONE are expected, ZERO are tolerated.\n");
        }

        private static IEnumerable<UInt128> GetBasePowersOf2()
        {
            for (int bit = 0; bit < 128; bit++)
            {
                yield return UInt128.One << bit;
            }
        }

        private static IEnumerable<UInt128> GenerateValues(UInt128 offset)
        {
            // This generates around 236K values with offset = 10000
            foreach (UInt128 power in GetBasePowersOf2())
            { 
                if (power <= UInt128.Zero)
                {
                    break;
                }
                for (UInt128 j = power - offset; j < power + offset; j++)
                {
                    if (j > UInt128.Zero)
                    {
                        yield return j;
                    }
                }
            }
        }


    }
}
