using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    static class Day3
    {
        public static int RunPart1()
        {
            var lines = File.ReadAllText("../../../Days/3/InputPart1.txt");

            Stopwatch stopwatch = Stopwatch.StartNew();

            Regex pattern = new Regex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)");

            int total = 0;
            foreach (Match m in pattern.Matches(lines))           
                total += (int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
            
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {stopwatch.ElapsedMilliseconds}");

            return total;
        }
       
        public static int RunPart2()
        {
            string lines = File.ReadAllText("../../../Days/3/InputPart2.txt");

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Matches parts of the text between start/do() and don't()/end
            Regex doPattern = new Regex("(?:^|do\\(\\))(.*?)(?=(?:don't\\(\\))|$)", RegexOptions.Singleline);
            Regex mulPattern = new Regex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)");

            int total = 0;
            foreach (Match doMatch in doPattern.Matches(lines))            
                foreach (Match mulMatch in mulPattern.Matches(doMatch.Groups[1].Value))                
                    total += (int.Parse(mulMatch.Groups[1].Value) * int.Parse(mulMatch.Groups[2].Value));
                
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed (ms): {stopwatch.ElapsedMilliseconds}");

            return total;
        }       
    }
}
