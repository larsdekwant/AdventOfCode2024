namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDay<long> day = new Day7();
            Console.WriteLine(day.RunPart(1));
            Console.WriteLine(day.RunPart(2));
        }
    }

    interface IDay<T>
    {
        T RunPart(int part);
    }
}