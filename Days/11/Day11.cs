using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

namespace AdventOfCode
{
    class Day11 : IDay<long>
    {
        private Dictionary<(long, int), long> KnownStones = [];

        public long RunPart(int part)
        {
            string input = File.ReadAllText("../../../Days/11/InputPart1.txt");

            int blinks = part switch
            {
                1 => 25,
                2 => 75,
                _ => throw new ArgumentException("Not a valid part")
            };

            long total = 0;
            foreach (string num in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                total += CountStones(long.Parse(num), blinks);
            }                                  

            return total;
        } 
        
        // Counts how many stones there will be after N blinks starting from a particular stone
        private long CountStones(long stone, int blinks)
        {
            // If this stone has been seen before for this amount of remaining blinks, return its stored value.
            if(KnownStones.TryGetValue((stone, blinks), out long total))
            {
                return total;
            }

            long count;
            int digits;

            // Base case
            if (blinks == 0) count = 1;

            // Rule 1
            else if(stone == 0)
            {
                count = CountStones(1, blinks - 1);
            }

            // Rule 2
            else if ((digits = (int)Math.Floor(Math.Log10(stone) + 1)) % 2 == 0)
            {
                double div = Math.Pow(10, digits / 2);
                long newStone1 = (long)(stone / div);
                long newStone2 = (long)(stone % div);

                count = CountStones(newStone1, blinks - 1) + CountStones(newStone2, blinks - 1);
            }

            // Rule 3
            else
            {
                count = CountStones(stone * 2024, blinks - 1);
            }

            // Store the count in the known values map for future use.
            KnownStones.Add((stone, blinks), count);
            return count;
        }
    }
}