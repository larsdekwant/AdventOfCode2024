using System.Diagnostics;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDay<int> day = new Day12();            

            Console.WriteLine(day.RunPart(1));
            Console.WriteLine(day.RunPart(2));
        }
    }

    interface IDay<T>
    {
        T RunPart(int part);
    }
}