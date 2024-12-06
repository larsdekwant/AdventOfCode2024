using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode
{
    static class Day6
    {
        const int STRAIGHT = 0;
        const int UP = -1;

        static int rows;
        static int cols;
        static char[,]? map = null;

        static int startX;
        static int startY;

        public static void ReadMap()
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

        public static int RunPart1()
        {
            ReadMap();
            return Move(startX, startY, STRAIGHT, UP).visited.Count();
        }     

        public static int RunPart2()
        {
            ReadMap();

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
        public static (IEnumerable<(int, int)> visited, int loop) Move(int gX, int gY, int gDx, int gDy)
        {
            HashSet<(int, int, int, int)> visited = new HashSet<(int, int, int, int)>();

            int looped = 0;
            while (true)
            {
                // Looped back around to a tile AND direction that is already visited.
                if (!visited.Add((gX, gY, gDx, gDy)))
                {
                    looped = 1;
                    break;
                }

                int nextX = gX + gDx;
                int nextY = gY + gDy;

                // Moves out of bounds, done.
                if (nextX < 0 || nextX >= cols || nextY < 0 || nextY >= rows) break;

                // Rotate by 90 degrees if obstacle ahead
                if (map[nextX, nextY] == '#')
                {
                    int temp = gDy;
                    gDy = gDx;
                    gDx = -temp;
                    continue;
                }

                // Move into direction
                gX += gDx;
                gY += gDy;
            }

            // Select only tiles with distinct x,y coordinates.
            return (visited.Select(t => (t.Item1, t.Item2)).Distinct(), looped);
        }
    }  
}
