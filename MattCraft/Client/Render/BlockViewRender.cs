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
    // Renders the wireframe of currently viewed block.

    class BlockViewRender
    {
        VertexArray VAO;
        Shader shader;
        TextureTiled blocktextures;
        NetConstructor constructor;

        int width;
        int height;

        const int DRAW_LIMIT = 100000;

        int tempwirelocation = 0;
        double temptime = 0;

        public BlockViewRender(int Width, int Height, Dictionary<int[], Chunk> initialchunkdata, Vector3 playerpos)
        {
            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray();
            VAO.SetupWorldRender();
            shader = new Shader("../../../MattCraft/Client/Shader/worldshader.vert", "../../../MattCraft/Client/Shader/wireshader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GLError.PrintError("Post blockview depth test enabling");

            blocktextures = new TextureTiled("../../../MattCraft/Client/terrain.png", 16, 16);

            constructor = new NetConstructor(blocktextures);

            int[] indices = new int[]
            {
                0, 1,
                0, 2,
                1, 3,
                2, 3,

                4, 5,
                4, 6,
                5, 7,
                6, 7,

                0, 4,
                1, 5,
                2, 6,
                3, 7
            };

            // This is only needed once - the order of elements never changes.
            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
            GLError.PrintError("Post wire ebo creation");

            VAO.PushVertexArray(blocktowiredata(0, 0, 0));
        }

        public float[,] blocktowiredata(int x, int y, int z)
        {
            //float[,] data = new float[8, 5];
            List<int[]> data = new List<int[]>();

            for(int delx = 0; delx <= 1; delx++)
                for (int dely = 0; dely <= 1; dely++)
                    for (int delz = 0; delz <= 1; delz++)
                    {
                        data.Add(new int[] { x + delx, y + dely, z + delz, 0, 0 });       
                    }

            float[,] returndata = new float[8, 5];

            for(int i = 0; i < data.Count; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    returndata[i, j] = (float)data[i][j];
                }
            }

            return returndata;
        }

        // TODO: maybe some of the maths in here could be centralised so the matrix shit only has to be done once?
        public void RenderFrame(FrameEventArgs e, Matrix4 view)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.Identity;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 10000.0f);

            shader.UniformMat4("model", ref model);
            shader.UniformMat4("view", ref view);
            shader.UniformMat4("projection", ref perspective);


            // THIS IS WHAT CAUSES THE PROBLEM
            //shader.Use();
            VAO.BindVAO();

            GL.DrawElements(PrimitiveType.Lines, 24, DrawElementsType.UnsignedInt, 0);
            GLError.PrintError("Post wire drawing");
        }

        public void UpdateFrame(FrameEventArgs e, ClientFrameUpdateArgs args)
        {
            temptime += e.Time;
            if(temptime >= 1)
            {
                tempwirelocation += 1;
                temptime = 0;
                VAO.PushVertexArray(blocktowiredata(tempwirelocation, 0, 0));
            }
        }

        public void UpdateLocation(int x, int y, int z)
        {

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
