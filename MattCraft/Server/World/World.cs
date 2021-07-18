using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Server.World
{
    class World
    {
        ChunkData chunkdata;

        Chunk defaultchunk;

        public World()
        {
            chunkdata = new ChunkData();
        }

        // For dev purporses.
        public void PopulateChunks()
        {
            Block[,,] defaultblockdata = new Block[16, 16, 16];

            defaultblockdata.Clone();

            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    for (int k = 0; k < 16; k++)
                    {
                        defaultblockdata[i, j, k] = new Blocks.Air();
                        if (i + j + k < 10)
                            defaultblockdata[i, j, k] = new Blocks.Dirt();
                        else if (i + j + k == 10)
                            defaultblockdata[i, j, k] = new Blocks.Snow();
                    }



            chunkdata.SetChunk(new Chunk((Block[,,]) defaultblockdata.Clone(), new int[] { 0, 0, 0 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 1, 0, 0 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 0, 1, 0 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 0, 0, 1 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 1, 1, 0 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 0, 1, 1 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 1, 0, 1 }));
            chunkdata.SetChunk(new Chunk((Block[,,])defaultblockdata.Clone(), new int[] { 1, 1, 1 }));
        }

        public ChunkUpdate DestroyBlock(int x, int y, int z)
        {
            int[] targetchunk = new int[] { x / 16, y / 16, z / 16 };
            Chunk chunk = chunkdata.GetChunk(targetchunk);
            chunk.SetBlock(new Blocks.Air(), x % 16, y % 16, z % 16);
            chunkdata.SetChunk(chunk);
            return new ChunkUpdate(chunk, false);
        }

        internal ChunkData GetFullChunkData(Vector3 playerpos, int renderdistance)
        {
            return chunkdata.GetChunksByRadius(playerpos, renderdistance);
        }
    }
}
