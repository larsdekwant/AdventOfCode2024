using System;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Drawing;

namespace AdventOfCode
{
    class Day14 : IDay<int>
    {
        const int IN_BETWEEN_QUADRANTS = 0;
        const int XMAX = 101;
        const int YMAX = 103;

        public int RunPart(int part)
        {
            var input = File.ReadLines("../../../Days/14/InputPart1.txt");

            List<Robot> robots = [];
            foreach(string line in input)
            {
                int[] matches = Regex.Matches(line, "(-?\\d+)").Select(m => int.Parse(m.Value)).ToArray();
                robots.Add(new Robot(matches[0], matches[1], matches[2], matches[3]));
            }

            if (part == 1)
            {
                int seconds = 100;
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

            // Search for an image that clearly looks like a christmas tree.
            if (part == 2)
            {
                // If you can't find it, increase this number.
                int seconds = 10000;
                for (int i = 0; i < seconds; i++)
                {
                    Bitmap bmp = new Bitmap(XMAX, YMAX);

                    robots.ForEach(StepRobot);
                    foreach (Robot r in robots)
                    {
                        bmp.SetPixel(r.P.x, r.P.y, Color.White);
                    }
                    bmp.Save($"../../../Days/14//Renders/{i + 1}.bmp");
                }

                // Good luck searching for the easter egg in your folder.
                return 0;
            }

            throw new ArgumentException("Not a valid part");
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