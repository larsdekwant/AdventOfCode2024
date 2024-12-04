using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    static class Day2
    {
        public static int RunPart1()
        {
            var lines = File.ReadLines("../../../Days/2/InputPart1.txt");

            Stopwatch stopwatch = Stopwatch.StartNew();

            int count = 0;
            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var report = nums.Select(int.Parse).ToList();
                count += CheckReport(report);
            }

            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {stopwatch.ElapsedMilliseconds}");

            return count;
        }

        public static int CheckReport(List<int> report)
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

        public static int RunPart2()
        {
            var lines = File.ReadLines("../../../Days/2/InputPart2.txt");

            Stopwatch stopwatch = Stopwatch.StartNew();

            int count = 0;
            foreach (var line in lines)
            {
                string[] nums = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var report = nums.Select(int.Parse).ToList();
                count += CheckReportPart2(report);
            }

            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {stopwatch.ElapsedMilliseconds}");

            return count;
        }

        public static int CheckReportPart2(List<int> report, int dampener = 1)
        {
            int mult = 1;
            int diff0 = report[0] - report[1];
            if (diff0 < 0) mult = -1; // denotes whether the report is increasing or decreasing.

            for (int i = 0; i < report.Count - 1; i++)
            {
                int diff = report[i] - report[i + 1];
                if (diff * mult < 1 || diff * mult > 3)
                {                  
                    if (dampener > 0)
                    {
                        // if dampener still available, retry with removal of levels which can cause a report to be unsafe
                        int rm1 = CheckReportPart2(report.Where((v, id) => id != i).ToList(), 0);
                        int rm2 = CheckReportPart2(report.Where((v, id) => id != i + 1).ToList(), 0);
                        int rm3 = CheckReportPart2(report.Where((v, id) => id != i - 1).ToList(), 0);

                        if (rm1 + rm2 + rm3> 0) return 1;
                    }
                    return 0;
                }
            }

            return 1;
        }
    }
}
