using System;

namespace AdventOfCode
{
    class Day6 : IDay<int>
    {
        const int STRAIGHT =  0;
        const int UP       = -1;    

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/6/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            char [,] map = new char[rows, cols];
            (int x, int y) start = (0,0);

            // Find guard starting position, and copy input into a mutable 2D char-array
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    map[x, y] = input[y][x];
                    if (input[y][x] != '.' && input[y][x] != '#')
                    {
                        start.x = x;
                        start.y = y;
                    }
                }

            return part switch
            {
                1 => CountPositions(map, start),
                2 => CountPossibleObstructions(map, start),
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        public int CountPositions(char[,] map, (int x , int y) start)
        {
            return Move(map, start.x, start.y, STRAIGHT, UP).visited.Count();
        }     

        public int CountPossibleObstructions(char[,] map, (int x, int y) start)
        {
            int loops = 0;
            foreach ((int x, int y) in Move(map, start.x, start.y, STRAIGHT, UP).visited.Skip(1))
            {
                // Place obstacle at every spot in the path once (except start), and test for loops.
                char old = map[x, y];
                map[x, y] = '#';
                loops += Move(map, start.x, start.y, STRAIGHT, UP).loop;
                map[x, y] = old;
            }

            return loops;
        }        

        // Walk the guard starting from a certain position into a certain direction.
        // Ends when guard goes out of bounds, or enters a loop.
        private (IEnumerable<(int, int)> visited, int loop) Move(char[,] map, int x, int y, int dx, int dy)
        {
            HashSet<(int, int, int, int)> visited = new HashSet<(int, int, int, int)>();

            int looped = 0;
            while (true)
            {
                // Looped back around to a tile AND direction that is already visited.
                if (!visited.Add((x, y, dx, dy)))
                {
                    looped = 1;
                    break;
                }

                int nextX = x + dx;
                int nextY = y + dy;

                // Moves out of bounds, done.
                if (nextX < 0 || nextX >= map.GetLength(0) || nextY < 0 || nextY >= map.GetLength(1)) break;

                // Rotate by 90 degrees if obstacle ahead
                if (map[nextX, nextY] == '#')
                {
                    int temp = dy;
                    dy = dx;
                    dx = -temp;
                    continue;
                }

                // Move into direction
                x += dx;
                y += dy;
            }

            // Select only tiles with distinct x,y coordinates.
            return (visited.Select(t => (t.Item1, t.Item2)).Distinct(), looped);
        }
    }  
}
