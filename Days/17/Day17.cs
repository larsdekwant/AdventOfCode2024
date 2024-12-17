using System;

namespace AdventOfCode
{
    class Day17 : IDay<string>
    {       
        public string RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/17/InputPart1.txt").ToArray();

            long[] regs = new long[3];
            for (int i = 0; i < 3 ; i++)
            {
                regs[i] = long.Parse(input[i].Split(' ')[2]);
            }

            int[] program = input[4].Split([' ', ',']).Skip(1).Select(int.Parse).ToArray();

            return part switch
            {
                1 => string.Join(',', RunProgram(program, regs).Select(x => x.ToString())),
                2 => FindSelfCopy(program, regs).ToString(),
                _ => throw new ArgumentException("Not a valid part")
            };
        }

        // Runs the given program with the given registers, makes a local copy of the registers to not update values globally.
        private List<long> RunProgram(int[] program, long[] regs)
        {
            long[] localRegs = (long[])regs.Clone();
            int pointer = 0;
            List<long> output = [];
            while (pointer < program.Length)
            {
                int operand = program[pointer + 1];
                pointer = PerformOpCode((OpCode)program[pointer], operand, localRegs, pointer, output);
            }

            return output;
        }

        // Finds what value of register A will output a copy of the program.
        // When the output has N digits, register A will be at most 8^N.
        // Found a pattern: when the last X digits match for a certain regA value, last X digits will occur again for the first time
        // in an output of (X + 1) digits, when regA is set to regA * 8.
        private long FindSelfCopy(int[] program, long[] regs)
        {
            long regA;
            for (regA = 0; regA < (long)Math.Pow(8, program.Length); regA++)
            {
                regs[0] = regA;
                List<long> output = RunProgram(program, regs);

                // Check whether the output of size N matches with the last N digits of program
                bool matchLastDigits = program[(program.Length - output.Count)..]
                    .Select((x, i) => output[i] == x)
                    .All(b => b);                

                if (matchLastDigits)
                {
                    // Found the value if all digits match, and output is same size as program.
                    if (output.Count == program.Length) break;
                    regA = (regA * 8) - 1; // pre-adjust -1 cuz loop counter will + 1 again.
                }               
            }

            return regA;
        }

        private int PerformOpCode(OpCode opcode, int operand, long[] regs, int pointer, List<long> output)
        {           
            switch (opcode)
            {
                case OpCode.ADV:
                    long numerator = regs[0];
                    long denominator = (long)Math.Pow(2, ComboOperand(operand, regs));
                    regs[0] = numerator / denominator;
                    break;

                case OpCode.BXL:
                    regs[1] = regs[1] ^ operand;
                    break;

                case OpCode.BST:
                    regs[1] = ComboOperand(operand, regs) % 8;
                    break;

                case OpCode.JNZ:
                    if (regs[0] == 0) break;
                    pointer = operand;
                    return pointer;

                case OpCode.BXC:
                    regs[1] = regs[1] ^ regs[2];
                    break;

                case OpCode.OUT:
                    output.Add(ComboOperand(operand, regs) % 8);
                    break;

                case OpCode.BDV:
                    numerator = regs[0];
                    denominator = (long)Math.Pow(2, ComboOperand(operand, regs));
                    regs[1] = numerator / denominator;
                    break;

                case OpCode.CDV:
                    numerator = regs[0];
                    denominator = (long)Math.Pow(2, ComboOperand(operand, regs));
                    regs[2] = numerator / denominator;
                    break;
            }

            return pointer + 2;
        }

        private long ComboOperand(int value, long[] regs)
        {
            return value switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => regs[0],
                5 => regs[1],
                6 => regs[2],
                _ => throw new ArgumentException($"Not a valid operand: {value}")
            };
        }

        private enum OpCode
        {
            ADV = 0,
            BXL = 1,
            BST = 2,
            JNZ = 3,
            BXC = 4,
            OUT = 5,
            BDV = 6,
            CDV = 7
        }
    }
}