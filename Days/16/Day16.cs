using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace AdventOfCode
{
    class Day16 : IDay<int>
    {
        private const char WALL  = '#';

        private readonly static (int x, int y)[] CardinalDirections = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/16/InputPart1.txt").ToArray();

            int rows = input.Length;
            int cols = input[0].Length;
            char[,] map = new char[cols, rows];

            // Parse the map
            (int x, int y) start = (0, 0);
            (int x, int y) end   = (0, 0);
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    map[x, y] = input[y][x];                  
                    if (input[y][x] == 'S') start = (x, y);
                    if (input[y][x] == 'E') end   = (x, y);
                }

            var output = DijkstraShortestPath(map, start, (1,0));

            int min = CardinalDirections.Min(v => output.cost[(end, v)]);

            HashSet<(int, int)> seats = [];
            foreach(var v in CardinalDirections.Where(v => output.cost[(end, v)] == min))            
                seats.UnionWith(Backtrack(output.prevs, start, end, v, []));
            
            return part switch
            {
                1 => min,
                2 => seats.Count,
                _ => throw new ArgumentException("Not a valid part")
            };            
        }

        private (Dictionary<((int, int), (int, int)), int> cost,
                 Dictionary<((int, int), (int, int)), List<((int, int), (int, int))>> prevs) 
            DijkstraShortestPath(char[,] map, (int x, int y) start, (int dx, int dy) dir)
        {
            // Initialisation.
            int w = map.GetLength(0);
            int h = map.GetLength(1);
            Dictionary<((int, int), (int, int)), int> cost = [];
            HashSet<((int, int), (int, int))> visited = [];
            Dictionary<((int, int), (int, int)), List<((int, int), (int, int))>> prevs = [];
            for(int x = 0; x < w; x++)            
                for(int y = 0; y < h; y++)
                    foreach(var v in CardinalDirections)
                    {
                        cost[((x, y), v)] = int.MaxValue;
                        prevs[((x, y), v)] = [];
                    }                                                         
            cost[(start, dir)] = 0;

            PriorityQueue<((int, int) p, (int, int) v), int> queue = new PriorityQueue<((int, int), (int, int)), int> ();
            queue.Enqueue((start, dir), 0);

            // Perform the search algorithm.
            while (queue.Count > 0)
            {
                ((int, int) p, (int, int) v) = queue.Dequeue();                
                if (visited.Contains((p,v))) continue;
                visited.Add((p,v));

                // Process all 4 neighbours
                foreach(((int x, int y) next, var vNext, int cNext) in GetNeighbours(p, v))
                {
                    if (map[next.x, next.y] != WALL)
                    {
                        int nextCost = cost[(p,v)] + cNext;
                        if (nextCost <= cost[(next, vNext)])
                        {
                            cost[(next, vNext)] = nextCost;
                            prevs[(next, vNext)].Add((p, v));
                        }

                        queue.Enqueue((next, vNext), nextCost);
                    }
                }                    
            }

            return (cost, prevs);
        }   

        // Generate the 4 possible neighbours, either by moving or by only rotating.
        private List<((int, int) p, (int, int) v, int cost)> GetNeighbours((int x, int y) p, (int dx, int dy) v)
        {
            int[] costs = [1, 1000, 2000, 1000];

            List<((int, int) p, (int, int) v, int cost)> nbs = [];
            for (int i = 0; i < 4; i++)
            {
                (int x, int y) next = i == 0 ? (p.x + v.dx, p.y + v.dy) : p;
                nbs.Add((next, v, costs[i]));

                // Rotate by 90 degrees
                int tmp = v.dx;
                v.dx = -v.dy;
                v.dy = tmp;
            }

            return nbs;
        }

        // Backtracks from end to start, storing all unique tiles in the path.
        private HashSet<(int, int)> Backtrack(Dictionary<((int, int), (int, int)), List<((int, int), (int, int))>> prevs,
                                              (int, int) start, (int, int) p, (int, int) v, HashSet<(int, int)> acc)
        {
            acc.Add(p);
            if(p == start) return acc;

            HashSet<(int, int)> path = [];
            foreach (((int, int) p, (int, int) v) parent in prevs[(p, v)])           
                path.UnionWith(Backtrack(prevs, start, parent.p, parent.v, acc));           

            acc.UnionWith(path);

            return acc;
        }
    }
}