using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Day10 : IDay<int>
    {
        private readonly static (int x, int y)[] CardinalDirections = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/10/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            int[,] map = new int[rows, cols];

            // Store topological map into 2D array, and track all the possible starting positions.
            List<(int x, int y)> starts = [];
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    map[x, y] = (int)char.GetNumericValue(input[y][x]);
                    if (input[y][x] == '0')
                    {
                        starts.Add((x, y));
                    }
                }

            int total = 0;
            foreach (var start in starts)
            {
                total += part switch
                {
                    1 => GetTrailTails(map, start).Distinct().Count(),
                    2 => GetTrailTails(map, start).Count,
                    _ => throw new ArgumentException("Not a valid part")
                };
            }

            return total;
        }

        // Returns all the trail tails (not unique) from all possible trail paths that are reachable from start.
        private List<(int, int)> GetTrailTails(int[,] map, (int x, int y) start)
        {           
            int curHeight = map[start.x, start.y];
            if (curHeight == 9) return [start];

            List<(int, int)> tails = [];
            foreach ((int dx, int dy) in CardinalDirections)
            {
                (int x, int y) next = (start.x + dx, start.y + dy);

                if (next.x < 0 || next.x >= map.GetLength(0) || next.y < 0 || next.y >= map.GetLength(1)) continue;
                if (map[next.x, next.y] == curHeight + 1)
                {
                    tails.AddRange(GetTrailTails(map, next));
                }
            }

            return tails;
        }
    }
}