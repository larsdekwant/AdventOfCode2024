using System;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Day14 : IDay<int>
    {
        const int IN_BETWEEN_QUADRANTS = 0;
        const int XMAX = 101;
        const int YMAX = 103;

        private readonly static (int x, int y)[] CardinalDirections = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        public int RunPart(int part)
        {
            var input = File.ReadLines("../../../Days/14/InputPart1.txt");

            List<Robot> robots = [];
            foreach(string line in input)
            {
                int[] matches = Regex.Matches(line, "(-?\\d+)").Select(m => int.Parse(m.Value)).ToArray();
                robots.Add(new Robot(matches[0], matches[1], matches[2], matches[3]));
            }

            return part switch
            {
                1 => GetSafetyFactor(robots, 100),
                2 => FindChristmasTree(robots),
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        private int GetSafetyFactor(List<Robot> robots, int seconds = 100)
        {
            for (int i = 0; i < seconds; i++)
            {
                robots.ForEach(StepRobot);
            }

            return robots
                .Select(GetQuadrant)
                .Where(q => q != IN_BETWEEN_QUADRANTS)
                .GroupBy(q => q)
                .Aggregate(1, (acc, val) => acc * val.Count());
        }

        // Assuming that a christmas tree is made out of a large group of robots that are direct neighbours.
        private int FindChristmasTree(List<Robot> robots)
        {
            int seconds = 0;
            while (true)
            {
                robots.ForEach(StepRobot);
                if (LargestGroup(robots) >= robots.Count / 4) return seconds + 1;
                seconds++;
            }
        }

        // Finds the size of the largest group of robots that are direct neighbours.
        private int LargestGroup(List<Robot> robots)
        {
            int largest = 0;

            // Map of robot positions
            bool[,] map = new bool[XMAX, YMAX];
            foreach(Robot robot in robots)
            {
                map[robot.P.x, robot.P.y] = true;
            }

            bool[,] visited = new bool[XMAX, YMAX];
            for (int x  = 0; x < XMAX; x++)           
                for (int y = 0; y < YMAX; y++)
                {
                    if(visited[x, y]) continue;

                    int count = CountGroup((x, y), map, visited);
                    if (count > largest) largest = count;
                }
            
            return largest;
        }

        // Explore an entire group of robots and count its members.
        private int CountGroup((int x, int y) pos, bool[,] robots, bool[,] visited)
        {
            if (pos.x < 0 || pos.x >= XMAX || pos.y < 0 || pos.y >= YMAX) return 0;
            if (!robots[pos.x, pos.y]) return 0;
            if (visited[pos.x, pos.y]) return 0;

            visited[pos.x, pos.y] = true;

            int total = 0;
            foreach((int dx, int dy) in CardinalDirections)
            {
                total += CountGroup((pos.x + dx, pos.y + dy), robots, visited);
            }

            return total + 1;
        }

        private void StepRobot(Robot robot) 
        {
            robot.P = (Mod(robot.P.x + robot.V.dx, XMAX), Mod(robot.P.y + robot.V.dy, YMAX));
        }

        private int GetQuadrant(Robot robot)
        {
            if (robot.P.x < XMAX / 2)
            {
                if (robot.P.y < YMAX / 2) return 1;
                if (robot.P.y > YMAX / 2) return 2;
            }

            if (robot.P.x > XMAX / 2)
            {
                if (robot.P.y < YMAX / 2) return 3;
                if (robot.P.y > YMAX / 2) return 4;
            }

            return IN_BETWEEN_QUADRANTS;
        }
        
        // Modulo for negative and positive numbers
        private static int Mod(int x, int mod)
        {
            int rem = x % mod;
            return rem < 0 ? rem + mod : rem;
        }
    }

    class Robot
    {
        public (int x,  int y ) P { get; set; }
        public (int dx, int dy) V { get; set; }

        public Robot(int p0, int p1, int v0, int v1)
        {
            this.P = (p0, p1);
            this.V = (v0, v1);
        }
    }
}