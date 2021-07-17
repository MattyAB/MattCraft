using OpenTK;
using System.Collections.Generic;

namespace MattCraft.Server
{
    public class GameUpdate
    {
        public List<ChunkUpdate> chunkupdate;
        public Vector3 playerpos;

        public GameUpdate(Vector3 position)
        {
            this.playerpos = position;
        }
    }
}