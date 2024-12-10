using System;

namespace AdventOfCode
{
    class Day1 : IDay<int>
    {
        public int RunPart(int part)
        {
            var lines = File.ReadLines($"../../../Days/01/InputPart{part}.txt");

            List<int> left = new List<int>();
            List<int> right = new List<int>();

            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                left.Add(int.Parse(nums[0]));
                right.Add(int.Parse(nums[1]));
            }

            return part switch
            {
                1 => TotalDistance(left, right),
                2 => Similarity(left, right),
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        private int TotalDistance(List<int> left, List<int> right)
        {        
            left.Sort();
            right.Sort();

            int count = 0;
            for(int i = 0; i<left.Count; i++)
            {
                count += Math.Abs(left[i] - right[i]);
            }

            return count;
        }

        private int Similarity(List<int> left, List<int> right)
        {           
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
