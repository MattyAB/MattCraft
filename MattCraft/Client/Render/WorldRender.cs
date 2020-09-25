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

        int width;
        int height;

        const int DRAW_LIMIT = 100000;

        public WorldRender(int Width, int Height, Dictionary<int[], Chunk> initialchunkdata, Vector3 playerpos)
        {
            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray();
            shader = new Shader("../../../MattCraft/Client/Shader/shader.vert", "../../../MattCraft/Client/Shader/shader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            blocktextures = new TextureTiled("../../../MattCraft/Client/terrain.png", 16, 16);

            constructor = new NetConstructor(blocktextures);

            //render = new render(playerpos);

            VAO.PushVertexArray(constructor.GetVertexData(GenerateChunkFaces(initialchunkdata)));
        }

        List<Face> GenerateChunkFaces(Dictionary<int[], Chunk> initialchunkdata)
        {
            List<Face> faces = new List<Face>();

            foreach (KeyValuePair<int[], Chunk> chunk in initialchunkdata)
            {
                List<Face> newfaces = chunk.Value.GetRenderFaces();
                for (int i = 0; i < newfaces.Count; i++)
                {
                    newfaces[i].x += 16 * chunk.Key[0];
                    newfaces[i].y += 16 * chunk.Key[1];
                    newfaces[i].z += 16 * chunk.Key[2];
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

        public void RenderFrame(FrameEventArgs e, Matrix4 view)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.Identity;

            //Matrix4 view = render.GetViewMat();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 10000.0f);

            shader.UniformMat4("model", ref model);
            shader.UniformMat4("view", ref view);
            shader.UniformMat4("projection", ref perspective);

            shader.Use();
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
