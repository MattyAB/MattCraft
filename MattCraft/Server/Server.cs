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
            playerpos = new Vector3(2f, 2f, 2f);
        }

        // Gets full chunk data given current player position.
        public Dictionary<int[], Chunk> GetFullChunkData()
        {
            return world.GetFullChunkData(playerpos, RENDER_DIST);
        }

        public GameUpdate UpdateServer(FrameEventArgs e, Vector3 playerpos)
        {
            this.playerpos = playerpos;

            Console.WriteLine(playerpos);

            GameUpdate updatereturn = new GameUpdate(playerpos);

            // Now add chunk data
            

            return updatereturn;
        }

        public Vector3 GetPlayerPos()
        {
            return playerpos;
        }
    }

    public struct BlockUpdate
    {
        public int x;
        public int y;
        public int z;
        public Block newblock;
    }
}
