namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDay day = new Day6();
            Console.WriteLine(day.RunPart1());
            Console.WriteLine(day.RunPart2());
        }
    }

    interface IDay
    {
        int RunPart1();
        int RunPart2();
    }
}