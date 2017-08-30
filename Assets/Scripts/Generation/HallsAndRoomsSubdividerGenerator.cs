namespace DLS.LD39.Generation
{
    using System;
    using System.Collections.Generic;
    using Utility;
    using Random = UnityEngine.Random;

    public class HallsAndRoomsSubdividerGenerator
    {
        public int MinimumSplittableArea = 3;
        public float MaxHallPercentage = 0.15f;
        public int Width;
        public int Height;

        private IntRect _building;
        private int _totalHallArea;
        private Queue<IntRect> _chunks = new Queue<IntRect>();
        private Queue<IntRect> _halls = new Queue<IntRect>();
        private Queue<IntRect> _blocks = new Queue<IntRect>();
        private Queue<IntRect> _rooms = new Queue<IntRect>();

        private void Generate()
        {
            _building = new IntRect(0, 0, Width, Height);


        }

        private void ChunksToBlocks()
        {
            
        }

        private void SplitChunk(IntRect chunk)
        {
            if (ShouldSplit())
            {
                
            }
        }

        private void BlocksToRooms()
        {
            
        }

        private static bool ShouldSplit()
        {
            return Random.Range(0.0f, 1.0f) < 0.5f;
        }

        private void RandomChunkSplit3(IntRect chunk, out IntRect chunkA, out IntRect hall, out IntRect chunkB)
        {
            throw new NotImplementedException();
        }

        private void RandomChunkSplit2(IntRect chunk, out IntRect chunkA, out IntRect hall)
        {
            throw new NotImplementedException();
        }
    }
}