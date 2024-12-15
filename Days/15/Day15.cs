using System;
using System.Xml.Schema;

namespace AdventOfCode
{
    class Day15 : IDay<int>
    {
        private static readonly Dictionary<char, (int dx, int dy)> Dir = new Dictionary<char, (int dx, int dy)>
        {
            ['^'] = ( 0, -1),
            ['>'] = ( 1,  0),
            ['v'] = ( 0,  1),
            ['<'] = (-1,  0)
        };

        public int RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/15/InputPart1.txt").ToArray();

            return part switch
            {
                1 => NormalWarehouse(input),
                2 => WideWarehouse(input),
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        // ##########
        // # PART 1 #
        // ##########

        private int NormalWarehouse(string[] input)
        {
            int rows = input.TakeWhile(line => line != "").Count();
            int cols = input[0].Length;
            Tile[,] map = new Tile[cols, rows];

            // Parse the map
            (int x, int y) robot = (0, 0);
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    if (input[y] == "") break;
                    map[x, y] = input[y][x] switch
                    {
                        '.' => Tile.Empty,
                        '#' => Tile.Wall,
                        'O' => Tile.Box,
                        _ => Tile.Empty
                    };

                    if (input[y][x] == '@') robot = (x, y);
                }

            // Parse inputs and perform movements
            foreach ((int x, int y) dir in string.Join("", input[(rows + 1)..]).Select(move => Dir[move]))
            {
                if (Move(map, robot, dir))
                {
                    robot = (robot.x + dir.x, robot.y + dir.y);
                }
            }

            return ComputeBoxGPSCoordinates(map);
        }        

        // Returns true if moving is possible, and moves all boxes required.
        private bool Move(Tile[,] map, (int x, int y) p, (int dx, int dy) v)
        {
            (int x, int y) next = (p.x + v.dx, p.y + v.dy);           

            switch(map[next.x, next.y])
            {
                case Tile.Wall:
                    return false;

                case Tile.Box:
                    // If box could be moved, the next tile should now be free, so try moving yourself again.
                    if (Move(map, next, v)) return Move(map, p, v);
                    return false;

                case Tile.Empty:
                    // Move
                    map[next.x, next.y] = map[p.x, p.y];
                    map[p.x, p.y] = Tile.Empty;
                    return true;
            }

            return false;
        }

        private int ComputeBoxGPSCoordinates(Tile[,] map)
        {
            int total = 0;
            for (int y = 0; y < map.GetLength(1); y++)
                for (int x = 0; x < map.GetLength(0); x++)
                    if (map[x, y] == Tile.Box) total += (100 * y) + x;

            return total;
        }

        // ############
        // ## PART 2 ##
        // ############

        private int WideWarehouse(string[] input)
        {
            int rows = input.TakeWhile(line => line != "").Count();
            int cols = input[0].Length;
            WideTile[,] map = new WideTile[cols * 2, rows];

            // Parse the map
            (int x, int y) robot = (0, 0);
            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                {
                    if (input[y] == "") break;

                    WideTile tile = input[y][x] switch
                    {
                        '.' => WideTile.Empty,
                        '#' => WideTile.Wall,
                        'O' => WideTile.BoxLeft,
                        _ => WideTile.Empty
                    };
                    map[2 * x, y] = tile;
                    map[2 * x + 1, y] = tile == WideTile.BoxLeft ? WideTile.BoxRight : tile;

                    if (input[y][x] == '@') robot = (2 * x, y);
                }

            // Parse inputs and perform movements
            foreach ((int x, int y) dir in string.Join("", input[(rows + 1)..]).Select(move => Dir[move]))
            {
                // Rollback the move if it was invalid
                WideTile[,] rollback = (WideTile[,])map.Clone();
                if (MoveWide(map, robot, dir))
                    robot = (robot.x + dir.x, robot.y + dir.y);
                else map = rollback;
            }

            return ComputeWideBoxGPSCoordinates(map);
        }

        private bool MoveWide(WideTile[,] map, (int x, int y) p, (int dx, int dy) v)
        {
            (int x, int y) next = (p.x + v.dx, p.y + v.dy);
            switch (map[next.x, next.y])
            {
                case WideTile.Wall:
                    return false;

                case WideTile.BoxLeft:
                    (int x, int y) right = (next.x + 1, next.y);
                    // If both sides of the box can be moved, now the next tile should be free, so try moving yourself again.
                    if (MoveWide(map, right, v) && MoveWide(map, next, v)) return MoveWide(map, p, v);
                    return false;

                case WideTile.BoxRight:
                    (int x, int y) left = (next.x - 1, next.y);          
                    if (MoveWide(map, left, v) && MoveWide(map, next, v)) return MoveWide(map, p, v);
                    return false;

                case WideTile.Empty:
                    // Move
                    map[next.x, next.y] = map[p.x, p.y];
                    map[p.x, p.y] = WideTile.Empty;                   
                    return true;
            }

            return false;
        }       

        private int ComputeWideBoxGPSCoordinates(WideTile[,] map)
        {
            int total = 0;
            for (int y = 0; y < map.GetLength(1); y++)
                for (int x = 0; x < map.GetLength(0); x++)
                    if (map[x, y] == WideTile.BoxLeft) total += (100 * y) + x;

            return total;
        }       

        enum Tile
        {
            Empty,
            Wall,
            Box
        }

        enum WideTile
        {
            Empty,
            Wall,
            BoxLeft,
            BoxRight
        }
    }     
}