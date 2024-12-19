namespace AdventOfCode
{
    class Day19 : IDay<long>
    {
        Dictionary<string, long> MatchableInWays = [];

        public long RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/19/InputPart1.txt").ToArray();       
            string[] sortedPatterns = input[0].Split(", ").OrderByDescending(pat => pat.Length).ToArray();

            int count = 0;
            long allMatches = 0;
            for (int i = 2; i < input.Length; i++)
            {
                long matches = Match(input[i], sortedPatterns);
                if (matches > 0) count++;
                allMatches += matches;                
            }

            return part switch
            {
                1 => count,
                2 => allMatches,
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        // Match string using dynamic programming, storing the number of ways to match it.
        private long Match(string remaining, string[] sortedPats)
        {
            if (remaining.Length == 0) return 1;
            if (MatchableInWays.TryGetValue(remaining, out long matches)) return matches;

            long numOfWays = 0;
            foreach (string pattern in sortedPats.SkipWhile(pat => pat.Length > remaining.Length))
            {
                if (remaining[..pattern.Length] != pattern) continue;
                numOfWays += Match(remaining[pattern.Length..], sortedPats);
            }            

            MatchableInWays.TryAdd(remaining, numOfWays);
            return numOfWays;
        }
    }   
}