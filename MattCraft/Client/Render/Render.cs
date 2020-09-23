using OpenTK;
using OpenTK.Graphics.OpenGL;
using MattCraft.Server.World.Blocks;
using MattCraft.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Client.Render
{
    public class Render
    {
        VertexArray VAO;
        Shader shader;
        TextureTiled blocktextures;
        NetConstructor constructor;
        Camera camera;

        int width;
        int height;

        const int DRAW_LIMIT = 100000;

        public Render(int Width, int Height, Dictionary<int[], Chunk> initialchunkdata, Vector3 playerpos)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);

            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray();
            shader = new Shader("../../../MattCraft/Client/Shader/shader.vert", "../../../MattCraft/Client/Shader/shader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SetTextureModes();

            blocktextures = new TextureTiled("../../../MattCraft/Client/terrain.png", 16, 16);

            constructor = new NetConstructor(blocktextures);

            camera = new Camera(playerpos);

            VAO.PushVertexArray(constructor.GetVertexData(GenerateChunkFaces(initialchunkdata)));
        }

        List<Face> GenerateChunkFaces(Dictionary<int[], Chunk> initialchunkdata)
        {
            List<Face> faces = new List<Face>();

            foreach(KeyValuePair<int[], Chunk> chunk in initialchunkdata)
            {
                List<Face> newfaces = chunk.Value.GetRenderFaces();
                for(int i = 0; i < newfaces.Count; i++)
                {
                    newfaces[i].x += 16 * chunk.Key[0];
                    newfaces[i].y += 16 * chunk.Key[1];
                    newfaces[i].z += 16 * chunk.Key[2];
                    for(int j = 0; j < faces.Count; j++)
                    {
                        if(newfaces[i].x == faces[j].x &&
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

        internal void PushMouseState(int dx, int dy)
        {
            camera.HandleInput((float)dx / 500f, (float)dy / 500f);
        }

        public void SetCameraPos(Vector3 pos)
        {
            camera.SetCameraPos(pos);
        }

        public Vector3 GetCameraPos()
        {
            return camera.GetCameraPos();
        }

        public float GetXRot()
        {
            return camera.GetXRot();
        }

        public void RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.Identity;

            Matrix4 view = camera.GetViewMat();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 10000.0f);
            
            shader.UniformMat4("model", ref model);
            shader.UniformMat4("view", ref view);
            shader.UniformMat4("projection", ref perspective);

            shader.Use();
            VAO.BindVAO();

            GL.DrawArrays(PrimitiveType.Quads, 0, DRAW_LIMIT);
        }

        internal void Cleanup()
        {
            VAO.CleanUp();
            shader.Dispose();
        }

        public void updateAspect(int Width, int Height)
        {
            this.width = Width;
            this.height = Height;
        }

        public void SetTextureModes()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            float[] borderColor = { 1.0f, 0.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }
    }
}
