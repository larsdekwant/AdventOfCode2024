using System;

namespace AdventOfCode
{
    class Day6 : IDay<int>
    {
        const int STRAIGHT = 0;
        const int UP = -1;

        readonly int rows;
        readonly int cols;
        char[,] map;

        readonly int startX;
        readonly int startY;

        public Day6()
        {
            string[] input = File.ReadLines("../../../Days/6/InputPart1.txt").ToArray();
            rows = input.Length;
            cols = input[0].Length;

            map = new char[rows, cols];

            // Find guard starting position, and copy input into a mutable 2D char-array
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    map[j, i] = input[i][j];
                    if (input[i][j] != '.' && input[i][j] != '#')
                    {
                        startX = j;
                        startY = i;
                    }
                }
        }

        public int RunPart1()
        {
            return Move(startX, startY, STRAIGHT, UP).visited.Count();
        }     

        public int RunPart2()
        {
            int loops = 0;
            foreach ((int x, int y) in Move(startX, startY, STRAIGHT, UP).visited.Skip(1))
            {
                // Place obstacle at every spot in the path once (except start), and test for loops.
                char old = map[x, y];
                map[x, y] = '#';
                loops += Move(startX, startY, STRAIGHT, UP).loop;
                map[x, y] = old;
            }

            return loops;
        }        

        // Walk the guard starting from a certain position into a certain direction.
        // Ends when guard goes out of bounds, or enters a loop.
        private (IEnumerable<(int, int)> visited, int loop) Move(int x, int y, int dx, int dy)
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
                if (nextX < 0 || nextX >= cols || nextY < 0 || nextY >= rows) break;

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
