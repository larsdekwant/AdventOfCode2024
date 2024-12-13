using System;
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;

namespace AdventOfCode
{
    class Day13 : IDay<long>
    {
        public long RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/13/InputPart1.txt").ToArray();

            long error = part switch
            {
                1 => 0,
                2 => 10000000000000,
                _ => throw new ArgumentException("Not a valid part")
            };

            long tokens = 0;
            for(int i = 0; i < input.Length; i += 4)
            {
                // Parse the input into coefficients for A, B and the goal locations.
                string pat = "\\D+";
                int[] c_A = Regex.Split(input[i], pat).Skip(1).Select(int.Parse).ToArray();
                int[] c_B = Regex.Split(input[i + 1], pat).Skip(1).Select(int.Parse).ToArray();
                int[] goal = Regex.Split(input[i + 2], pat).Skip(1).Select(int.Parse).ToArray();

                // ILP solver
                Solver solver = Solver.CreateSolver("SCIP");

                // Add the integer!! variables: A, B >= 0
                Variable A = solver.MakeIntVar(0.0, double.PositiveInfinity, "A");
                Variable B = solver.MakeIntVar(0.0, double.PositiveInfinity, "B");

                // Add the constraints
                // A * x_1 + B * x_2 = x_goal
                // A * y_1 + B * y_2 = y_goal
                solver.Add(c_A[0] * A + c_B[0] * B == goal[0] + error);
                solver.Add(c_A[1] * A + c_B[1] * B == goal[1] + error);

                // Objective to optimize
                solver.Minimize(3 * A + B);

                // Sum the tokens needed for all machines that have an optimal solution.
                Solver.ResultStatus resultStatus = solver.Solve();
                if (resultStatus == Solver.ResultStatus.OPTIMAL)
                {
                    tokens += (long)solver.Objective().Value();                    
                }
            }

            return tokens;
        }

    }
}