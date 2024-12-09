using System;

namespace AdventOfCode
{
    class Day9 : IDay<long>
    {
        public long RunPart(int part)
        {
            string input = File.ReadAllText("../../../Days/9/InputPart1.txt");

            List<DiskSpace> filesystem = new List<DiskSpace>();

            bool space = false;
            for (int i = 0; i < input.Length; i++)
            {
                int blockSize = (int)char.GetNumericValue(input[i]);
                Block block = new Block(blockSize);
                for (int j = 0; j < block.Size; j++)
                {
                    if (space) filesystem.Add(DiskSpace.Empty(block)); else filesystem.Add(DiskSpace.File(i / 2, block));
                }
                space = !space; // alternate
            }

            List<DiskSpace> compacted = part switch
            {
                1 => CompactWithFragmentation(filesystem),
                2 => CompactWithoutFragmentation(filesystem),
                _ => throw new ArgumentException("Not a valid part")
            };

            long checksum = 0;
            for (int i = 0; i < compacted.Count; i++)
            {
                checksum += compacted[i].IsEmpty ? 0 : compacted[i].Id * i;
            }
            return checksum;
        }

        // O(N) Moves over the entire filesystem once (from both ends simultationsly)
        private List<DiskSpace> CompactWithFragmentation(List<DiskSpace> filesystem)
        {
            int firstOpenSpace = 0;
            int lastFileIndex = filesystem.Count - 1;
            while (true)
            {
                // Move to last occupied disk space
                while (filesystem[lastFileIndex].IsEmpty)
                {
                    lastFileIndex--;
                }

                // Move to first empty disk space
                while (!filesystem[firstOpenSpace].IsEmpty)
                {
                    firstOpenSpace++;
                }

                if (firstOpenSpace >= lastFileIndex) break;

                // Swap the disk spaces
                DiskSpace temp = filesystem[lastFileIndex];
                filesystem[lastFileIndex] = filesystem[firstOpenSpace];
                filesystem[firstOpenSpace] = temp;
            }               
            return filesystem;
        }

        // O(N^2) For each block, finds the first available empty space with enough size.
        private List<DiskSpace> CompactWithoutFragmentation(List<DiskSpace> filesystem)
        {
            int firstOpenSpace = 0;
            int lastFileIndex = filesystem.Count - 1;
            while (lastFileIndex >= 0)
            {
                // Move to last occupied disk space
                while (filesystem[lastFileIndex].IsEmpty)
                {
                    lastFileIndex--;
                }

                int sizeToSwap = filesystem[lastFileIndex].Size;

                // Move to first empty disk space from the start with enough available size
                firstOpenSpace = 0;
                while (firstOpenSpace < lastFileIndex)
                {
                    if (filesystem[firstOpenSpace].IsEmpty && filesystem[firstOpenSpace].Size >= sizeToSwap) break;
                    firstOpenSpace++;                    
                }

                // Skip the current block, since no available empty space was found.
                if (firstOpenSpace >= lastFileIndex)
                {
                    lastFileIndex -= sizeToSwap;
                    continue;
                }

                // Swap every file in the block.
                filesystem[firstOpenSpace].Block.Size -= sizeToSwap;
                for (int swap = 0; swap < sizeToSwap; swap++)
                {
                    int emptyId = firstOpenSpace + swap;
                    int fileId = lastFileIndex - swap;

                    // Swap the disk spaces
                    DiskSpace temp = filesystem[fileId];
                    filesystem[fileId] = filesystem[emptyId];
                    filesystem[emptyId] = temp;
                }
            }            
            return filesystem;
        }
    }
        
    
    class DiskSpace
    {
        public static DiskSpace Empty(Block block)        => new DiskSpace(default, block, true);
        public static DiskSpace File(int id, Block block) => new DiskSpace(id, block, false);

        public bool IsEmpty = true;
        public int  Id;
        public Block Block;

        public DiskSpace(int id, Block block, bool empty)
        {
            this.IsEmpty = empty;
            this.Id = id;
            this.Block = block;
        }

        public int Size => this.Block.Size;
    }

    class Block(int size)
    {
        public int Size = size;
    }
}