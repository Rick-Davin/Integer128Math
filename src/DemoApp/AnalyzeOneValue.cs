namespace DemoApp
{
    // This validation or analysis is for a single UInt128 value.
    // If Math.Sqrt and GetNearestIntegerRoot do not match, then a deeper analysis is done.
    internal class AnalyzeOneValue
    {
        public static bool Run(UInt128 value, string? title = null)
        {
            TestValue<UInt128> test = TestValue<UInt128>.Create(value);
            return (new AnalyzeOneValue(test, title)).Run();
        }
        public static bool Run(TestValue<UInt128> test, string? title = null)
        {
            return (new AnalyzeOneValue(test, title)).Run();
        }

        private AnalyzeOneValue(TestValue<UInt128> test, string? title)
        {
            Test = test;
            Title = title;
        }

        public TestValue<UInt128> Test { get; init; }
        public string? Title { get; init; }


        public bool Run()
        {
            string fullTitle = $"ANALYZE ONE Value: {Test}";
            if (!string.IsNullOrWhiteSpace(Title))
            {
                fullTitle += $"\n{Title}"   ;
            }

            AppHelper.ShowTitle(fullTitle, ConsoleColor.Yellow, ConsoleColor.DarkBlue);

            Console.WriteLine($"\t{Test.MathSqrt}");
            Console.WriteLine($"\t{Test.NearestRoot}");
            if (Test.AreRootsEqual)
            {
                Console.WriteLine("Analysis Complete!  Both square roots are equal.\n");
                return true;
            }

            Console.WriteLine("Deeper Analysis Required!  Both square roots are NOT equal.");

            ShowMore(Test.MathSqrt);
            ShowMore(Test.NearestRoot);

            // We really already know that NearestRoot will always be good but let's be cautious.
            if (Test.MathSqrt.IsGood && Test.NearestRoot.IsGood) 
            {
                Console.WriteLine("The calculated square roots are not equal but both are properly less than the Value.");
                Console.WriteLine("The correct value to use is the larger of the two:");
                var larger = Test.NearestRoot.Root > Test.MathSqrt.Root ? Test.NearestRoot : Test.MathSqrt;
                Console.WriteLine($"\t{larger}");
            }

            Console.WriteLine("Analysis Complete!\n");
            return false;
        }

     
        private void ShowMore(TestRoot<UInt128> root)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = root.IsGood ? ConsoleColor.Green : ConsoleColor.Red;
            
            const int fieldWidth = 20;
            string text = root.UseMathSqrt ? "Math.Sqrt SQUARED" : "NearestRoot SQUARED";
            if (text.Length < fieldWidth)
            {
                text += new string(' ', fieldWidth - text.Length);
            }

            string status;
            if (root.IsGood)
            {
                status = "Good: Is properly less than or equal to Value.";
            }
            else
            {
                if (root.IsTooBig)
                {
                    status = "BAD: Is greater than Value.";

                }
                else
                {
                    status = "BAD: Exceeded MaxValue and wrapped around to MinValue.";
                }
            }

            Console.Write($"{text}: {root.Squared:N0}\n\t{status}");
            Console.ForegroundColor = originalColor;
            Console.WriteLine(" ");
        }
    }
}
