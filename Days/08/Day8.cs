using System;
using System.Numerics;

namespace AdventOfCode
{
    class Day8 : IDay<int>
    {
        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/08/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            char[,] map = new char[rows, cols];

            // Group positions of nodes of the same frequency together in lists
            Dictionary<char, List<Vector2>> freqLocMap = new Dictionary<char, List<Vector2>>();           
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    char val = input[y][x];
                    map[x, y] = val;
                    if(val != '.' && !freqLocMap.TryAdd(val, [new(x, y)]))
                    {
                        freqLocMap[val].Add(new(x, y));
                    }                    
                }

            HashSet<Vector2> antinodes = [];
            foreach (var nodes in freqLocMap.Values)           
                antinodes.UnionWith(GetFreqAntinodes(part, map, nodes));          

            return antinodes.Count;
        }

        // Loop over all pairs of nodes, compute their distances and place an antinode 1 distance away from the freq nodes.
        private HashSet<Vector2> GetFreqAntinodes(int part, char[,] map, List<Vector2> nodes)
        {
            HashSet<Vector2> antinodes = [];
            for (int i = 0; i < nodes.Count; i++)            
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Vector2 n1 = nodes[i];
                    Vector2 n2 = nodes[j];

                    Vector2 diff1 = n1 - n2;
                    Vector2 diff2 = n2 - n1;

                    switch (part)
                    {
                        case 1:
                            AddAntinodes(map, antinodes, n1 + diff1);
                            AddAntinodes(map, antinodes, n2 + diff2);
                            break;
                        case 2:
                            AddAntinodes(map, antinodes, n1, diff1, line: true);
                            AddAntinodes(map, antinodes, n2, diff2, line: true);
                            break;
                        default:
                            throw new ArgumentException("Not a valid part");
                    }                   
                }          
            return antinodes;
        }       

        // Adds antinodes to the set in a certain direction, as long as they are in bounds.
        private void AddAntinodes(char[,] map, HashSet<Vector2> antinodes, Vector2 anti, Vector2 diff = default, bool line = false)
        {
            do
            {
                if (anti.X < 0 || anti.X >= map.GetLength(0) || anti.Y < 0 || anti.Y >= map.GetLength(1)) return;
                antinodes.Add(anti);
                anti += diff;
            } while (line);          
        }
    }   
}