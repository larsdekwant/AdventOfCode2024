using System;
using System.Diagnostics;

namespace AdventOfCode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDay<long> day = new Day13();
            Console.WriteLine(day.RunPart(1));
            Console.WriteLine(day.RunPart(2));

            //int runs = 1000;

            //Stopwatch sw = Stopwatch.StartNew();
            //for (int i = 0; i < runs; i++)
            //{
            //    IDay<int> day = new Day12();

            //    day.RunPart(1);
            //    day.RunPart(2);
            //}

            //Console.WriteLine($"{sw.ElapsedMilliseconds / runs}ms");
        }
    }

    interface IDay<T>
    {
        T RunPart(int part);
    }
}