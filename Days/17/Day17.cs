using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;


namespace AdventOfCode
{
    class Day17 : IDay<string>
    {
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

        public string RunPart(int part)
        {
            string[] input = File.ReadLines("../../../Days/17/InputPart1.txt").ToArray();

            int[] regs = new int[3];
            for (int i = 0; i < 3 ; i++)
            {
                regs[i] = int.Parse(input[i].Split(' ')[2]);
            }

            int[] program = input[4].Split([' ', ',']).Skip(1).Select(int.Parse).ToArray();
            int pointer = 0;
            List<int> output = [];
            while(pointer < program.Length)
            {
                int operand = program[pointer + 1];
                pointer = PerformOpCode((OpCode)program[pointer], operand, regs, pointer, output);
            }           

            return string.Join(',', output.Select(x => x.ToString()));
        }

        private int PerformOpCode(OpCode opcode, int operand, int[] regs, int pointer, List<int> output)
        {           
            switch (opcode)
            {
                case OpCode.ADV:
                    int numerator = regs[0];
                    int denominator = 1 << ComboOperand(operand, regs);
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
                    denominator = 1 << ComboOperand(operand, regs);
                    regs[1] = numerator / denominator;
                    break;

                case OpCode.CDV:
                    numerator = regs[0];
                    denominator = 1 << ComboOperand(operand, regs);
                    regs[2] = numerator / denominator;
                    break;
            }

            return pointer + 2;
        }

        private int ComboOperand(int value, int[] regs)
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
    }
}