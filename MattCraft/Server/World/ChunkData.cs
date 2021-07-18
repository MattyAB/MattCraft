using MattCraft.Client.Render;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Server.World
{
    public class ChunkData
    {
        List<Chunk> chunks;

        public ChunkData()
        {
            chunks = new List<Chunk>();
        }

        public void Add(Chunk chunk)
        {
            chunks.Add(chunk);
        }

        public Chunk GetChunk(int[] coords)
        {
            foreach(Chunk chunk in chunks)
            {
                int[] loc = chunk.GetLocation();
                if (loc[0] == coords[0] && loc[1] == coords[1] && loc[2] == coords[2])
                {
                    return chunk;
                }
            }

            throw new Exception("No chunk with such coordinates exists.");
        }

        public void SetChunk(Chunk chunk)
        {
            int[] mainloc = chunk.GetLocation();
            // Get rid of the old chunk (if one exists)
            for(int i = 0; i < chunks.Count; i++)
            {
                int[] oldloc = chunks[i].GetLocation();
                if (mainloc[0] == oldloc[0] && mainloc[1] == oldloc[1] && mainloc[2] == oldloc[2])
                {
                    chunks.RemoveAt(i);
                    break; //So that we don't try to reference a null at the end of the list, as we have just deleted one
                }
            }

            // Now set the new chunk
            chunks.Add(chunk);
        }

        internal ChunkData GetChunksByRadius(Vector3 playerpos, int renderdistance)
        {
            ChunkData returner = new ChunkData();

            foreach (Chunk chunk in chunks)
            {
                int[] loc = chunk.GetLocation();
                Vector3 chunkcentre = new Vector3(loc[0] * 16 + 8, loc[1] * 16 + 8, loc[2] * 16 + 8);
                if (Vector3.Distance(chunkcentre, playerpos) < (float)(16 * renderdistance))
                {
                    returner.SetChunk(chunk);
                }
            }

            return returner;
        }

        // WARNING: should only be run on local chunk data
        public List<Face> GenerateChunkFaces()
        {
            List<Face> faces = new List<Face>();

            foreach(Chunk chunk in chunks)
            {
                List<Face> newfaces = chunk.GetRenderFaces();

                for (int i = 0; i < newfaces.Count; i++)
                {
                    int[] loc = chunk.GetLocation();
                    newfaces[i].x += 16 * loc[0];
                    newfaces[i].y += 16 * loc[1];
                    newfaces[i].z += 16 * loc[2];

                    for (int j = 0; j < faces.Count; j++)
                    {
                        if (newfaces[i].x == faces[j].x &&
                            newfaces[i].y == faces[j].y &&
                            newfaces[i].z == faces[j].z &&
                            newfaces[i].direction == faces[j].direction)
                        {
                            faces.RemoveAt(j);
                            newfaces.RemoveAt(i);
                            i--; // So that it doesn't skip out faces due to the removal of previous ones.
                            break;
                        }
                    }
                }
                faces.AddRange(newfaces);
            }

            return faces;
        }
    }
}
