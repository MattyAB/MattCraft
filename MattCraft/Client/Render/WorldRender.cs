using MattCraft.Server;
using MattCraft.Server.World;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Client.Render
{
    class WorldRender
    {
        VertexArray VAO;
        Shader shader;
        TextureTiled blocktextures;
        NetConstructor constructor;

        ChunkData chunkdata;

        int width;
        int height;

        const int DRAW_LIMIT = 100000;

        public WorldRender(int Width, int Height, ChunkData initialchunkdata, Vector3 playerpos)
        {
            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray();
            VAO.SetupWorldRender();
            shader = new Shader("../../../MattCraft/Client/Shader/worldshader.vert", "../../../MattCraft/Client/Shader/blockshader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);

            blocktextures = new TextureTiled("../../../MattCraft/Client/terrain.png", 16, 16);

            constructor = new NetConstructor(blocktextures);

            //render = new render(playerpos);

            VAO.PushVertexArray(constructor.GetVertexData(initialchunkdata.GenerateChunkFaces()));
            this.chunkdata = initialchunkdata;
        }

        internal void UpdateFrame(List<ChunkUpdate> chunkupdate)
        {
            foreach (ChunkUpdate update in chunkupdate)
            {
                if (!update.remove)
                {
                    //if (!chunkdata.Remove(update.coords))
                    //    throw new Exception("Chunk to remove wasn't found... But I'm sure it exists!");
                    chunkdata.SetChunk(update.chunk);
                }
                else
                {
                    // This is meant to be for unloading chunks
                    throw new NotImplementedException();
                }
            }
            
            if(chunkupdate.Count != 0)
            {
                GLError.PrintError("Pre pushing chunk data");
                VAO.PushVertexArray(constructor.GetVertexData(chunkdata.GenerateChunkFaces()));
                GLError.PrintError("Post pushing chunk data");
            }
        }

        public void RenderFrame(FrameEventArgs e, Matrix4 view)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            Matrix4 model = Matrix4.Identity;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 10000.0f);

            shader.Use();

            // Move this into general render class
            shader.UniformMat4("model", ref model);
            shader.UniformMat4("view", ref view);
            shader.UniformMat4("projection", ref perspective);

            VAO.BindVAO();

            GL.DrawArrays(PrimitiveType.Quads, 0, DRAW_LIMIT);
        }

        public void UpdateAspect(int Width, int Height)
        {
            this.width = Width;
            this.height = Height;
        }

        internal void Cleanup()
        {
            VAO.CleanUp();
            shader.Dispose();
        }
    }
}
