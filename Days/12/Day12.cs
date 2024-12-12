using System;
using System.Collections;
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

        private Dictionary<int, (int area, int perimeter, int sides)> RegionStats = [];
        private int CurrentRegion = 0;

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/12/InputPart1.txt").ToArray();
            int rows = input.Length;
            int cols = input[0].Length;

            char[,] garden  = new char[rows, cols];
            int[,] regions = new int[rows, cols];
            this.CurrentRegion = 1;
            this.RegionStats = [];

            // Store garden in 2D array.
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    garden[x, y] = input[y][x];
                    regions[x, y] = UNKNOWN;            
                }

            // Gather all required information about the regions.
            for (int x = 0; x < garden.GetLength(0); x++)           
                for (int y = 0;y < garden.GetLength(1); y++)
                {
                    if (regions[x, y] != UNKNOWN) continue;

                    var stats = ExploreRegion(garden, regions, (x,y), (0, 0, 0));
                    RegionStats.Add(CurrentRegion, stats);
                    CurrentRegion++;
                }
            

            return part switch
            {
                1 => RegionStats.Values.Sum(region => region.area * region.perimeter),
                2 => RegionStats.Values.Sum(region => region.area * region.sides),
                _ => throw new ArgumentException("Not a valid part")
            };              
        }

        // Explores the entire region starting from a given tile, returns the area, perimiter and amount of corners of that region.
        private (int area, int perimeter, int sides) ExploreRegion(char[,] garden, int[,] regions, (int x, int y) tile, (int area, int perimeter, int sides) stats)
        {
            if(regions[tile.x, tile.y] == CurrentRegion) return stats;

            regions[tile.x, tile.y] = CurrentRegion;

            int area = 1;
            int perimeter = 4;
            int sides = CountCorners(garden, regions, tile);

            // Explore all non-diagonal neighbours
            foreach ((int dx, int dy) in CardinalDirections)
            {
                (int x, int y) next = (tile.x + dx, tile.y + dy);
                if (InBounds(garden, next) && garden[next.x, next.y] == garden[tile.x, tile.y])
                {
                    var region = ExploreRegion(garden, regions, next, stats);
                    area += region.area;
                    perimeter += region.perimeter;
                    sides += region.sides;

                    // Had neighbour in the region, so reduce perimeter by 1.
                    perimeter--;
                }
            }

            return (stats.area + area, stats.perimeter + perimeter, sides);
        }

        // Count all corners for a tile, both inward and outward corners
        private int CountCorners(char[,] garden, int[,] regions, (int x, int y) tile)
        {
            int corners = 0;
            char plant = garden[tile.x, tile.y];
            int region = regions[tile.x, tile.y];
            for (int corner = 0; corner < 4; corner++)
            {
                // [(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1)];
                // Take the 3 directional vectors that determine the quadrant of this corner
                (int dx, int dy)[] checks = CornerDirections.Skip(2 * corner).Take(3).ToArray();

                // The 3 tiles (clockwise order) on the corner to check
                (int x, int y) c0 = (tile.x + checks[0].dx, tile.y + checks[0].dy);
                (int x, int y) c1 = (tile.x + checks[1].dx, tile.y + checks[1].dy);
                (int x, int y) c2 = (tile.x + checks[2].dx, tile.y + checks[2].dy);

                // If one of the neighbours was already visited, the corner is already accounted for.
                if (InBounds(garden, c0) && regions[c0.x, c0.y] == region) continue;
                if (InBounds(garden, c2) && regions[c2.x, c2.y] == region) continue;

                // True if out of bounds or not belonging to this region
                bool bound0 = !InBounds(garden, c0) || garden[c0.x, c0.y] != plant;
                bool bound1 = !InBounds(garden, c1) || garden[c1.x, c1.y] != plant;
                bool bound2 = !InBounds(garden, c2) || garden[c2.x, c2.y] != plant;

                // Any outward corner, including the diagonal corners, for example:
                //  ...       ..A
                //  AA.  and  AA.
                //  .A.       .A.
                if (bound0 && bound2)
                {
                    corners++;
                }
                // Any inward corner, for example:
                //  A.
                //  AA
                else if (((bound0 && !bound1 && !bound2)
                      || (!bound0 && bound1 && !bound2)
                      || (!bound0 && !bound1 && bound2))
                      && regions[c1.x, c1.y] != region)
                {
                    corners++;
                }
            }
            return corners;
        }
      
        private static bool InBounds<T>(T[,] map, (int x, int y) pos) =>
            pos.x >= 0 && pos.x < map.GetLength(0) && pos.y >= 0 && pos.y < map.GetLength(1);  
    }        
}