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

        DateTime lastblockbreak;

        int RENDER_DIST = 3;

        public Server()
        {
            world = new World.World();
            world.PopulateChunks();

            //playerpos = new Vector3(14f, 14f, 14f);
            playerpos = new Vector3(2f, 2f, 2f);

            lastblockbreak = DateTime.Now;
        }

        // Gets full chunk data given current player position.
        public Dictionary<int[], Chunk> GetFullChunkData()
        {
            return world.GetFullChunkData(playerpos, RENDER_DIST);
        }

        public GameUpdate UpdateServer(FrameEventArgs e, Vector3 playerpos, OpenTK.Input.MouseState mousestate, int[] lookingat)
        {
            this.playerpos = playerpos;

            GameUpdate updatereturn = new GameUpdate(playerpos);

            if (mousestate.IsButtonDown(OpenTK.Input.MouseButton.Left) && DateTime.Now - lastblockbreak > TimeSpan.FromMilliseconds(100))
            {
                updatereturn.chunkupdate.Add(world.DestroyBlock(lookingat[0], lookingat[1], lookingat[2]));
                lastblockbreak = DateTime.Now;
            }            

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
