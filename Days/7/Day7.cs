using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    class Day7 : IDay<long>
    {
        public long RunPart1()
        {
            var input = File.ReadLines("../../../Days/7/InputPart1.txt");

            long total = 0;
            foreach (var line in input)
            {
                string[] vals = line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries);
                long goal = long.Parse(vals[0]);
                var nums = vals.Skip(1).Select(long.Parse).ToArray();

                if (TryOperators([Multiply, Sum, Concat], goal, nums)) total += goal;
            }

            return total;
        }

        private bool TryOperators(Func<long, long, long>[] ops, long goal, long[] nums)
        {
            if (nums.Length < 2) return false;
            return ops.Any(op => TryOperator(op, ops, goal, nums[0], nums, 1));
        }

        private bool TryOperator(Func<long, long, long> op, Func<long, long, long>[] ops, long goal, long x, long[] rest, int restIndex)
        {
            if (restIndex == rest.Length) 
            {
                return x == goal;
            }

            long result = op(x, rest[restIndex]);
            return ops.Any(op => TryOperator(op, ops, goal, result, rest, restIndex + 1));
        }

        private long Multiply(long x, long y) => x * y;
        private long Sum(long x, long y) => x + y;
        private long Concat(long x, long y) => long.Parse(x.ToString() + y.ToString());



        public long RunPart2()
        {
            string[] input = File.ReadLines("../../../Days/6/InputPart1.txt").ToArray();
            return 0;
        }                
    }  
}
