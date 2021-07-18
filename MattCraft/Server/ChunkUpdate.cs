using MattCraft.Server.World;

namespace MattCraft.Server
{
    public class ChunkUpdate
    {
        public Chunk chunk;
        public bool remove;

        public ChunkUpdate(Chunk chunk, bool remove)
        {
            this.chunk = chunk;
            this.remove = remove;
        }
    }
}