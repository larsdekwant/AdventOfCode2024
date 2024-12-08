using System;

namespace AdventOfCode
{
    class Day4 : IDay<int>
    {
        const int STRAIGHT =  0;
        const int RIGHT    =  1;       
        const int LEFT     = -1;
        const int UP       = -1;
        const int DOWN     =  1;

        public int RunPart(int part)
        {
            string[] puzzle = File.ReadLines("../../../Days/4/InputPart1.txt").ToArray();
            int rows = puzzle.Length;
            int cols = puzzle[0].Length;

            int count = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    count += part switch
                    {
                        1 => CountCardinalDirections(puzzle, i, j),
                        2 => CountCross(puzzle, i, j),
                        _ => throw new ArgumentException("Not a valid part")
                    };
                }
            return count;
        }       

        private int CountCardinalDirections(string[] puzzle, int row, int col)
        {
            return CountDirection(puzzle, row, col, RIGHT,    STRAIGHT)
                 + CountDirection(puzzle, row, col, LEFT,     STRAIGHT)
                 + CountDirection(puzzle, row, col, STRAIGHT, UP      )
                 + CountDirection(puzzle, row, col, STRAIGHT, DOWN    )
                 + CountDirection(puzzle, row, col, RIGHT,    DOWN    )
                 + CountDirection(puzzle, row, col, RIGHT,    UP      )
                 + CountDirection(puzzle, row, col, LEFT,     DOWN    )
                 + CountDirection(puzzle, row, col, LEFT,     UP      );
        }

        private int CountDirection(string[] puzzle, int row, int col, int dirRow, int dirCol, string keyword = "XMAS")
        {
            for (int i = 0; i < keyword.Length; i++)
            {
                // Bounds checks
                if (row < 0 || row >= puzzle.Length) return 0;
                if (col < 0 || col >= puzzle[0].Length) return 0;

                // Check for the keyword
                if (puzzle[row][col] != keyword[i]) return 0;

                row += dirRow;
                col += dirCol;
            }

            return 1;
        }       

        private int CountCross(string[] puzzle, int row, int col)
        {
            if (puzzle[row][col] != 'A') return 0;
            int count = CountDirection(puzzle, row + LEFT,  col + UP,   RIGHT, DOWN, "MAS")
                      + CountDirection(puzzle, row + LEFT,  col + DOWN, RIGHT, UP,   "MAS")
                      + CountDirection(puzzle, row + RIGHT, col + UP,   LEFT,  DOWN, "MAS")
                      + CountDirection(puzzle, row + RIGHT, col + DOWN, LEFT,  UP,   "MAS");
            return count == 2 ? 1 : 0;
        }
    }
}
