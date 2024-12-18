using System;

namespace AdventOfCode
{
    class Day18 : IDay<string>
    {
        private const int WIDTH  = 71;
        private const int HEIGHT = 71;

        private const char WALL  = '#';
        private const char EMPTY = '.';

        public string RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/18/InputPart1.txt").ToArray();

            
            char[,] map = new char[WIDTH + 2, HEIGHT + 2];
            VectorInt2D start = new VectorInt2D(1, 1);
            VectorInt2D end = new VectorInt2D(WIDTH, HEIGHT);

            // Parse the map, add an outside wall to avoid bounds checking.
            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = EMPTY;                  
                    if (x == 0 || x == WIDTH + 1 || y == 0 || y == HEIGHT + 1) map[x, y] = WALL;
                }

            // Perform Dijkstra to find the shortest path.
            // If a new wall appears on the path, recompute the path.
            // If no new path exists, return the wall that caused this.
            VectorInt2D wall = VectorInt2D.EMPTY;
            var output = DijkstraShortestPath(map, start, end);
            List<int> shortestPathAfterByte = [];
            HashSet<VectorInt2D> path = Backtrack(output.prevs, start, end);
            for (int i = 0; i < input.Length; i++)
            {
                shortestPathAfterByte.Add(output.cost[end.X, end.Y]);
                if (path.Count == 0) break;

                int[] coord = input[i].Split(',').Select(int.Parse).ToArray();
                wall = new VectorInt2D(coord[0] + 1, coord[1] + 1);
                map[wall.X, wall.Y] = WALL;

                if (path.Contains(wall))
                {
                    output = DijkstraShortestPath(map, start, end);
                    path = Backtrack(output.prevs, start, end);
                }             
            }

            return part switch
            {
                1 => shortestPathAfterByte[1024].ToString(),
                2 => $"{wall.X - 1},{wall.Y - 1}",
                _ => throw new ArgumentException("Not a valid part")
            };            
        }        

        private (int[,] cost, VectorInt2D?[,] prevs) DijkstraShortestPath(char[,] map, VectorInt2D start, VectorInt2D end)
        {
            // Initialisation.
            int w = map.GetLength(0);
            int h = map.GetLength(1);
            int[,] cost = new int[w, h];
            bool[,] visited = new bool[w, h];
            VectorInt2D?[,] prevs = new VectorInt2D?[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    cost[x, y] = int.MaxValue;
                    visited[x, y] = false;
                    prevs[x, y] = null;
                }                   
                    
            cost[start.X, start.Y] = 0;

            PriorityQueue<VectorInt2D, int> queue = new PriorityQueue<VectorInt2D, int>();
            queue.Enqueue(start, 0);

            // Perform the search algorithm.
            while (queue.Count > 0)
            {
                VectorInt2D p = queue.Dequeue();
                if (p == end) break;

                if (visited[p.X, p.Y]) continue;
                visited[p.X, p.Y] = true;

                foreach(VectorInt2D nb in p.GetCardinalNeighbours())
                {
                    if (map[nb.X, nb.Y] == WALL) continue;

                    int nbCost = cost[p.X, p.Y] + 1;
                    if(nbCost < cost[nb.X, nb.Y])
                    {
                        cost[nb.X, nb.Y] = nbCost;
                        prevs[nb.X, nb.Y] = p;
                    }

                    queue.Enqueue(nb, nbCost);
                }                             
            }

            return (cost, prevs);
        }


        // Backtracks from end to start, storing all unique tiles in the path.
        private HashSet<VectorInt2D> Backtrack(VectorInt2D?[,] prevs, VectorInt2D start, VectorInt2D pos)
        {
            HashSet<VectorInt2D> path = [];

            VectorInt2D curr = pos;
            while(curr != start)
            {
                path.Add(curr);

                var next = prevs[curr.X, curr.Y];
                if (next == null) return [];
                curr = (VectorInt2D)next;
            }

            return path;
        }
    }   
}