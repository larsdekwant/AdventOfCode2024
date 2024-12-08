using System;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Day3 : IDay<int>
    {
        private Regex MulPattern = new Regex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)");

        public int RunPart(int part)
        {
            var input = File.ReadAllText($"../../../Days/3/InputPart{part}.txt");

            return part switch
            {
                1 => SumMultiplications(input),
                2 => SumEnabledMultiplications(input),
                _ => throw new ArgumentException("Not a valid part")
            };
        }
        private int SumMultiplications(string input)
        {
            int total = 0;
            foreach (Match m in MulPattern.Matches(input))           
                total += (int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));

            return total;
        }

        private int SumEnabledMultiplications(string input)
        {
            // Matches parts of the text between start/do() and don't()/end
            Regex doPattern = new Regex("(?:^|do\\(\\))(.*?)(?=(?:don't\\(\\))|$)", RegexOptions.Singleline);

            int total = 0;
            foreach (Match doMatch in doPattern.Matches(input))            
                foreach (Match mulMatch in MulPattern.Matches(doMatch.Groups[1].Value))                
                    total += (int.Parse(mulMatch.Groups[1].Value) * int.Parse(mulMatch.Groups[2].Value));

            return total;
        }       
    }
}
