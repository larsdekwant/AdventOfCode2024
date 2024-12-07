using System;

namespace AdventOfCode
{
    class Day4 : IDay<int>
    {
        const int STRAIGHT = 0;
        const int RIGHT = 1;       
        const int LEFT = -1;
        const int UP = -1;
        const int DOWN = 1;

        string[] puzzle = Array.Empty<string>();
        int rows;
        int cols;

        public int RunPart1()
        {
            puzzle = File.ReadLines("../../../Days/4/InputPart1.txt").ToArray();
            rows = puzzle.Length;
            cols = puzzle[0].Length;

            int count = 0;
            for (int i = 0; i < rows; i++)           
                for (int j = 0; j < cols; j++)
                {
                    count += CountDirection(i, j, RIGHT,    STRAIGHT);
                    count += CountDirection(i, j, LEFT,     STRAIGHT);
                    count += CountDirection(i, j, STRAIGHT, UP      );
                    count += CountDirection(i, j, STRAIGHT, DOWN    );
                    count += CountDirection(i, j, RIGHT,    DOWN    );
                    count += CountDirection(i, j, RIGHT,    UP      );
                    count += CountDirection(i, j, LEFT,     DOWN    );
                    count += CountDirection(i, j, LEFT,     UP      );
                }
            return count;
        }

        private int CountDirection(int row, int col, int dirRow, int dirCol, string keyword = "XMAS")
        {
            for (int i = 0; i < keyword.Length; i++)
            {
                // Bounds checks
                if (row < 0 || row >= rows) return 0;
                if (col < 0 || col >= cols) return 0;

                // Check for the keyword
                if (puzzle[row][col] != keyword[i]) return 0;

                row += dirRow;
                col += dirCol;
            }

            return 1;
        }

        public int RunPart2()
        {
            puzzle = File.ReadLines("../../../Days/4/InputPart2.txt").ToArray();
            rows = puzzle.Length;
            cols = puzzle[0].Length;

            int count = 0;
            // Can never start at outer layer, so move bounds in by 1 for efficiency
            for (int i = 1; i < rows - 1; i++)
                for (int j = 1; j < cols - 1; j++)               
                    count += CountCross(i, j);
            return count;
        }

        private int CountCross(int row, int col)
        {
            int count = CountDirection(row + LEFT,  col + UP,   RIGHT, DOWN, "MAS")
                      + CountDirection(row + LEFT,  col + DOWN, RIGHT, UP,   "MAS")
                      + CountDirection(row + RIGHT, col + UP,   LEFT,  DOWN, "MAS")
                      + CountDirection(row + RIGHT, col + DOWN, LEFT,  UP,   "MAS");
            return count == 2 ? 1 : 0;
        }
    }
}
