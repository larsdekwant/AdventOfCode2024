using System;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace AdventOfCode
{
    class Day8 : IDay<int>
    {
        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/8/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            char[,] map = new char[rows, cols];

            // Group positions of nodes of the same frequency together in lists
            Dictionary<char, List<(int x, int y)>> freqLocMap = new Dictionary<char, List<(int x, int y)>>();           
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    char val = input[y][x];
                    map[x, y] = val;
                    if(val != '.' && !freqLocMap.TryAdd(val, [(x, y)]))
                    {
                        freqLocMap[val].Add((x, y));
                    }                    
                }

            HashSet<(int x, int y)> antinodes = [];
            foreach (var nodes in freqLocMap.Values)           
                antinodes.UnionWith(GetFreqAntinodes(part, map, nodes));          

            return antinodes.Count;
        }

        // Loop over all pairs of nodes, compute their distances and place an antinode 1 distance away from the freq nodes.
        private HashSet<(int x, int y)> GetFreqAntinodes(int part, char[,] map, List<(int x, int y)> nodes)
        {
            HashSet<(int x, int y)> antinodes = [];
            for (int i = 0; i < nodes.Count; i++)            
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    (int x, int y) n1 = nodes[i];
                    (int x, int y) n2 = nodes[j];

                    (int x, int y) diff1 = (n1.x - n2.x, n1.y - n2.y);
                    (int x, int y) diff2 = (n2.x - n1.x, n2.y - n1.y);

                    switch (part)
                    {
                        case 1:
                            AddSingleAntinode(map, antinodes, n1, diff1);
                            AddSingleAntinode(map, antinodes, n2, diff2);
                            break;
                        case 2:
                            AddAntinodesInDirection(map, antinodes, n1, diff1);
                            AddAntinodesInDirection(map, antinodes, n2, diff2);
                            break;
                        default:
                            throw new ArgumentException("Not a valid part");
                    }                   
                }          
            return antinodes;
        }

        // Adds a single antinode in a direction after checking bounds.
        private void AddSingleAntinode(char[,] map, HashSet<(int x, int y)> antinodes, (int x, int y) anti, (int x, int y) diff)
        {
            anti = (anti.x + diff.x, anti.y + diff.y);
            if (anti.x >= 0 && anti.x < map.GetLength(0) && anti.y >= 0 && anti.y < map.GetLength(1)) 
                antinodes.Add(anti);        
        }

        // Adds antinodes to the set in a certain direction, as long as they are in bounds.
        private void AddAntinodesInDirection(char[,] map, HashSet<(int x, int y)> antinodes, (int x, int y) anti, (int x, int y) diff)
        {
            while (anti.x >= 0 && anti.x < map.GetLength(0) && anti.y >= 0 && anti.y < map.GetLength(1))
            {
                antinodes.Add(anti);
                anti = (anti.x + diff.x, anti.y + diff.y);
            }
        }
    }            
}
