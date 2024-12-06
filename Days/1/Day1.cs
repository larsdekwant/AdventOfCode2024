using System;

namespace AdventOfCode
{
    class Day1 : IDay
    {
        public int RunPart1()
        {
            var lines = File.ReadLines("../../../Days/1/InputPart1.txt");

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                left.Add(int.Parse(nums[0]));
                right.Add(int.Parse(nums[1]));
            }

            left.Sort();
            right.Sort();

            int count = 0;
            for(int i = 0; i<left.Count; i++)
            {
                count += Math.Abs(left[i] - right[i]);
            }

            return count;
        }

        public int RunPart2()
        {
            var lines = File.ReadLines("../../../Days/1/InputPart2.txt");

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                left.Add(int.Parse(nums[0]));
                right.Add(int.Parse(nums[1]));
            }

            Dictionary<int, int> rightCounter = new Dictionary<int, int>();
            foreach(int val in right) { 
                if(!rightCounter.TryAdd(val, 1))
                {
                    rightCounter[val]++;
                }
            }

            int count = 0;
            foreach(int val in left)
            {
                if(rightCounter.TryGetValue(val, out int rCount))
                    count += val * rCount;
            }

            return count;
        }
    }
}
