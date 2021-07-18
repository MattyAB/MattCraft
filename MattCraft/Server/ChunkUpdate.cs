using MattCraft.Server.World;

namespace MattCraft.Server
{
    public class ChunkUpdate
    {
        public Chunk chunk;
        public int[] coords;
        public bool remove;

        public ChunkUpdate(Chunk chunk, int[] coords, bool remove)
        {
            this.chunk = chunk;
            this.coords = coords;
            this.remove = remove;
        }
    }
}