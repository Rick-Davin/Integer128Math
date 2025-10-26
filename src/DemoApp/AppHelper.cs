namespace DemoApp
{
    internal static class AppHelper
    {
        public static double ParallelismFactor => 2;

        public static int MaxDegreeOfParallelism => (int)(Environment.ProcessorCount * ParallelismFactor);

        public static void ShowTitle(string title, ConsoleColor foreColor, ConsoleColor backColor)
        {
            Console.WriteLine();
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;
            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(DashLine);
            var lines = title.Split('\n');
            foreach (var line in lines)
            {
                Console.WriteLine(TextWithMinimumLength(line, MinConsoleTextWidth));
            }
            // Odd artifact somestimes has the colored lines bleed into other areas
            // so I do not use WriteLine below.
            Console.Write(DashLine);
            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;
            Console.Write("  \n");
        }   

        public const int MinConsoleTextWidth = 80;
        public static string DashLine => new string('-', MinConsoleTextWidth);

        public static string TextWithMinimumLength(string text, int minLength)
        {
            return text.Length >= minLength ? text : text + new string(' ', minLength - text.Length);
        }
    }
}
