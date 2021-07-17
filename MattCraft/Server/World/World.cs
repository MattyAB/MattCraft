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
        Dictionary<int[], Chunk> chunkdata;

        Chunk defaultchunk;

        public World()
        {
            chunkdata = new Dictionary<int[], Chunk>();
        }

        // For dev purporses.
        public void PopulateChunks()
        {
            defaultchunk = new Chunk();

            
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    for (int k = 0; k < 8; k++)
                        if (i + j + k < 10)
                            defaultchunk.SetBlock(new Blocks.Dirt(), i, j, k);
                        else if (i + j + k == 10)
                            defaultchunk.SetBlock(new Blocks.Snow(), i, j, k);
            

            /**
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    for (int k = 0; k < 16; k++)
                        chunk.SetBlock(new Blocks.Dirt(), i, j, k);**/

            chunkdata.Add(new int[] { 0, 0, 0 }, defaultchunk);
            chunkdata.Add(new int[] { 1, 0, 0 }, defaultchunk);
            chunkdata.Add(new int[] { 0, 1, 0 }, defaultchunk);
            chunkdata.Add(new int[] { 0, 0, 1 }, defaultchunk);
            chunkdata.Add(new int[] { 1, 1, 0 }, defaultchunk);
            chunkdata.Add(new int[] { 0, 1, 1 }, defaultchunk);
            chunkdata.Add(new int[] { 1, 0, 1 }, defaultchunk);
            chunkdata.Add(new int[] { 1, 1, 1 }, defaultchunk);
        }

        internal Dictionary<int[], Chunk> GetFullChunkData(Vector3 playerpos, int renderdistance)
        {
            Dictionary<int[], Chunk> returner = new Dictionary<int[], Chunk>();

            foreach(KeyValuePair<int[], Chunk> chunk in chunkdata)
            {
                Vector3 chunkcentre = new Vector3(chunk.Key[0] * 16 + 8, chunk.Key[1] * 16 + 8, chunk.Key[2] * 16 + 8);
                if(Vector3.Distance(chunkcentre, playerpos) < (float)(16 * renderdistance))
                {
                    returner.Add(chunk.Key, chunk.Value);
                }
            }

            return returner;
        }
    }
}
