using System;

namespace AdventOfCode
{
    class Day2 : IDay<int>
    {

        public int RunPart(int part)
        {
            var lines = File.ReadLines($"../../../Days/02/InputPart{part}.txt");

            int count = 0;
            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var report = nums.Select(int.Parse).ToList();
                count += part switch
                {
                    1 => CheckReport(report),
                    2 => TryFixReport(report),
                    _ => throw new ArgumentException("Not a valid part")
                };
            }

            return count;
        }       

        private int CheckReport(List<int> report)
        {
            int mult = 1;
            int diff0 = report[0] - report[1];
            if (diff0 < 0) mult = -1; // denotes whether the report is increasing or decreasing.

            for (int i = 0; i < report.Count - 1; i++)
            {
                int diff = report[i] - report[i + 1];
                if (diff * mult < 1 || diff * mult > 3) return 0;                
            }

            return 1;
        }       

        private int TryFixReport(List<int> report)
        {
            int mult = 1;
            int diff0 = report[0] - report[1];
            if (diff0 < 0) mult = -1; // denotes whether the report is increasing or decreasing.

            for (int i = 0; i < report.Count - 1; i++)
            {
                int diff = report[i] - report[i + 1];
                if (diff * mult < 1 || diff * mult > 3)
                {                                     
                    // try to see if it is fixable by removal of a level
                    int rm1 = CheckReport(report.Where((v, id) => id != i).ToList());
                    int rm2 = CheckReport(report.Where((v, id) => id != i + 1).ToList());
                    int rm3 = CheckReport(report.Where((v, id) => id != i - 1).ToList());
                    if (rm1 + rm2 + rm3 > 0) return 1;
                   
                    return 0;
                }
            }

            return 1;
        }
    }
}
