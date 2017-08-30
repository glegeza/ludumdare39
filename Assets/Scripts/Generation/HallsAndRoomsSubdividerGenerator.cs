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

//class HouseGenerator
//{
//    private:
//    static const area MinSplittableArea = ...; // 3 squares at least;
//    static const float MaxHallRate = ...; // 15%..40% I guess;

//    rect House;
//    area TotalHallArea = 0;
//    queue<rect> Chunks, Halls, Blocks, Rooms;

//    public:
//    HouseGenerator() // ctor;
//    {
//        House = rect(x, y);
//    }

//    void Generate()
//    {
//        ChunksToBlocks();
//        BlocksToRooms();
		
//            ... //// TODO:
		
//        // carve halls;
//        // where hall faces much older hall:
//        // place wall;
			
//        // carve rooms, leaving walls;
			
//        // put every room in queue of unreachable rooms;
//        // while this queue is not empty:
//        // get next room from queue;
//        // if room is touching any number of halls:
//        // make door, facing any avaliable hall;
//        // put this room in queue of reachable rooms;
//        // `continue`;
//        // if room is touching any other reachable room:
//        // connect this with other;
//        // place door, if Random wants so;
//        // `continue`;
//        // put this room in queue of unreachable rooms;
			
//        // place windows;
//    }

//    private:
//    void ChunksToBlocks()
//    {
//        Chunks << House;
//        while ((not Chunks.empty) && (TotalHallArea / House.area < MaxHallRate))
//        {
//            rect chunk;
//            Chunks >> chunk;

//            if (chunk.area > MinSplittableArea)
//                SplitChunk(chunk);
//            else Blocks << chunk;
//        }
//    }

//    void SplitChunk(rect chunk)
//    {
//        rect hall;

//        if (coin_flip()) // split in three;
//        {
//            rect chunk_a, chunk_b;
//            RandomChunkSplit3(current_block, chunk_a, hall, chunk_b);
//            Chunks << chunk_a << chunk_b;
//        }
//        else // split in two;
//        {
//            rect chunk_a;
//            RandomChunkSplit3(current_block, chunk_a, hall);
//            Chunks << chunk_a;
//        }

//        Halls << hall;
//        TotalHallArea += hall.area;
//    }

//    void BlocksToRooms()
//    {
//        while (not Blocks.empty)
//        {
//            rect block;
//            Blocks >> block;

//            if (WantSplitBlock(block))
//            {
//                rect block_a, block_b;
//                RandomBlockSplit(block, block_a, block_b);
//                Blocks << block_a << block_b;
//            }
//            else
//                Rooms << block;
//        }
//    }

//    //// TODO:
//    void RandomChunkSplit3(...);
//    void RandomChunkSplit2(...);
//    bool WantSplitBlock(...);
//    void RandomBlockSplit(...);
//}
