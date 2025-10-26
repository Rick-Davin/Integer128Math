namespace DemoApp
{

    internal class Program
    {
        // The purpose of this demo app is twofold.
        // (1) It shows that Math.Sqrt is incorrect for certain huge integers, and
        // (2) It shows that the custom NearestIntegerRoot method is correct.
        static void Main(string[] args)
        {
            AnalyzeOneValue.Run(UInt128.MaxValue, $"UInt128.MaxValue to test most extreme value");

            // Given the staggering amount of values to inspect, how did I find this one?  Pure, random luck!
            // I was running down a train thought and stumbled upon this value.  Later I realized that
            // my thoughts proved to be based on poor reasoning (wrong train!) but I walked away this gem.
            UInt128 specialCase = new UInt128(40_532_396_646_334_464UL, 1_729_382_256_910_270_465UL);
            AnalyzeOneValue.Run(specialCase, $"Special Case where both square roots are good but not equal!");

            ValidatePowersOf2.ShowBadPowersOf2();
            ValidatePowersOf2.RunWithOffset();

            // On my PC in Debug mode, these tests take around 30-60 seconds but in Release mode takes less than 5-10 seconds.
            ValidateSignedIntegers.RunParallel();
            ValidateUnsignedIntegers.RunParallel(startValue: (uint)int.MaxValue + 1);

            Console.WriteLine();
            AppHelper.ShowTitle("VALIDATION DEMO DONE!  Press ENTER key to exit...", ConsoleColor.White, ConsoleColor.DarkRed);
            Console.ReadLine();
        }
    }
}


