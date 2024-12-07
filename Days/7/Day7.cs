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

                if (TryOperators([Multiply, Sum], goal, nums)) total += goal;
            }

            return total;
        }       

        public long RunPart2()
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

        // Start recursion by just summing 0 + number1 (does nothing)
        private bool TryOperators(Func<long, long, long>[] ops, long goal, long[] nums) => 
            TryOperator(Sum, ops, goal, 0, nums, 0);

        private bool TryOperator(Func<long, long, long> op, Func<long, long, long>[] ops, long goal, long x, long[] nums, int nextI)
        {
            if (x > goal) return false;
            if (nextI == nums.Length) return x == goal;

            long result = op(x, nums[nextI]);
            return ops.Any(op => TryOperator(op, ops, goal, result, nums, nextI + 1));
        }

        private long Multiply(long x, long y) => x * y;
        private long Sum(long x, long y) => x + y;
        private long Concat(long x, long y) => long.Parse($"{x}{y}");
    }
}
