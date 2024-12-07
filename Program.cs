namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDay<long> day = new Day7();
            Console.WriteLine(day.RunPart1());
            Console.WriteLine(day.RunPart2());
        }
    }

    interface IDay<T>
    {
        T RunPart1();
        T RunPart2();
    }
}