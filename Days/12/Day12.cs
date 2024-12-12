using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode
{
    class Day12 : IDay<int>
    {
        private readonly static (int x, int y)[] CardinalDirections = [(0, -1), (1, 0), (0, 1), (-1, 0)];
        private readonly static (int x, int y)[] CornerDirections = [(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1)];
        private readonly static int UNKNOWN = 0;

        private Dictionary<char, int> RegionsOfType = [];
        private int CurrentRegion = 0;
        private Dictionary<int, (int area, int perimeter)> RegionStats = [];

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/12/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            char[,] garden  = new char[rows, cols];
            int[,] regions = new int[rows, cols];
            this.RegionsOfType = [];
            this.CurrentRegion = 0;
            this.RegionStats = [];

            // Store garden in 2D array.
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    garden[x, y] = input[y][x];
                    regions[x, y] = UNKNOWN;
                    RegionsOfType.TryAdd(input[y][x], 0);                   
                }

            for (int x = 0; x < garden.GetLength(0); x++)
            {
                for (int y = 0;y < garden.GetLength(1); y++)
                {
                    if (regions[x, y] != UNKNOWN) continue;

                    CurrentRegion++;
                    char plant = garden[x, y];
                    var stats = ExploreRegion(garden, regions, (x,y), 0, 0);
                    int corners = CountRegionCorners(regions, new bool[rows, cols], (x, y), 0);
                    //int sides = WalkRegionSides(garden, regions, CurrentRegion, (x, y-1), (x, y-1), (1, 0), 0);
                    RegionStats.Add(CurrentRegion, (stats.area, corners));
                }
            }

            return RegionStats.Values.Sum(region => region.area * region.perimeter);
        }

        private (int area, int perimeter) ExploreRegion(char[,] garden, int[,] regions, (int x, int y) start, int area, int perimeter)
        {
            if(regions[start.x, start.y] == CurrentRegion) return (area, perimeter);

            regions[start.x, start.y] = CurrentRegion;
            char plant = garden[start.x, start.y];
            int extraPerimeter = 4;
            int extraArea = 1;
            foreach((int dx, int dy) in CardinalDirections)
            {
                (int x, int y) next = (start.x + dx, start.y + dy);
                if (InBounds(garden, next) && garden[next.x, next.y] == plant)
                {
                    extraPerimeter--;
                    var stats = ExploreRegion(garden, regions, next, area, perimeter);
                    extraArea += stats.area;
                    extraPerimeter += stats.perimeter;
                }
            }

            return (area + extraArea, perimeter + extraPerimeter);
        }

        private int CountRegionCorners(int[,] regions, bool[,] visited, (int x, int y) start, int corners)
        {
            if (visited[start.x, start.y]) return corners;

            int region = regions[start.x, start.y];
            visited[start.x, start.y] = true;
            int totalcorners = 0;
            for (int corner = 0; corner < 4; corner++)
            {
                (int dx, int dy)[] cornerChecks = CornerDirections.Skip(2 * corner).Take(3).ToArray();
                
                // WIP
                int isCorner = 0;
                for (int dir = corner * 2; dir < (corner * 2) + 3; dir++)
                {
                    (int dx, int dy) cDir = CornerDirections[dir];
                    (int x, int y) check = (start.x + cDir.dx, start.y + cDir.dy);
                    if (InBounds(regions, check) && visited[check.x, check.y])
                    {
                        isCorner = 0;
                        break;
                    }
                    if(!InBounds(regions, check) || regions[check.x, check.y] != region) isCorner ++;                   
                }

                // Fix this, cuz '2' is also a possible corner in the following configuration:
                // AABB
                // BBAA
                if (isCorner == 1 || isCorner == 3) 
                    totalcorners++;
            }                

            foreach ((int dx, int dy) in CardinalDirections)
            {
                (int x, int y) next = (start.x + dx, start.y + dy);
                if (InBounds(regions, next) && regions[next.x, next.y] == region)
                {                  
                    totalcorners += CountRegionCorners(regions, visited, next, corners);                    
                }
            }

            return totalcorners;
        }

        private int WalkRegionSides(char[,] garden, int[,] regions, int region, (int x, int y) start, (int x, int y) pos, (int dx, int dy) dir, int sides)
        {
            if (pos == start && sides > 1) return sides;

            (int x, int y) next = (pos.x + dir.dx, pos.y + dir.dy);
            (int dx, int dy) right = (-dir.dy, dir.dx);
            (int x, int y) nextRight = (next.x + right.dx, next.y + right.dy);

            // Turn right onto new side
            if (!InBounds(garden, nextRight) || regions[nextRight.x, nextRight.y] != region)
            {
                return WalkRegionSides(garden, regions, region, start, nextRight, right, sides + 1);
            }
            // Turn left onto new side
            if (InBounds(garden, next) && regions[next.x, next.y] == region)
            {
                return WalkRegionSides(garden, regions, region, start, pos, (dir.dy,  -dir.dx), sides + 1);
            }
            // Walk forward on same side
            return WalkRegionSides(garden, regions, region, start, next, dir, sides);
        }

        private bool InBounds<T>(T[,] garden, (int x, int y) pos) =>
            pos.x >= 0 && pos.x < garden.GetLength(0) && pos.y >= 0 && pos.y < garden.GetLength(1);  
    }        
}