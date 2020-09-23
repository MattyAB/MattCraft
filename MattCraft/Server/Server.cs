using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattCraft.Server.World;
using OpenTK;

namespace MattCraft.Server
{
    public class Server
    {
        World.World world;
        Vector3 playerpos;

        int RENDER_DIST = 2;

        public Server()
        {
            world = new World.World();
            world.PopulateChunks();

            //playerpos = new Vector3(14f, 14f, 14f);
            playerpos = new Vector3(16f, 16f, 16f);
        }

        // Gets full chunk data given current player position.
        public Dictionary<int[], Chunk> GetFullChunkData()
        {
            return world.GetFullChunkData(playerpos, RENDER_DIST);
        }

        public GameUpdate UpdateServer(FrameEventArgs e)
        {
            GameUpdate updatereturn = new GameUpdate();

            return updatereturn;
        }

        public Vector3 GetPlayerPos()
        {
            return playerpos;
        }
    }

    public struct GameUpdate
    {
        List<BlockUpdate> blockupdates;
        Vector3 playerpos;
    }

    public struct BlockUpdate
    {
        public int x;
        public int y;
        public int z;
        public Block newblock;
    }
}
