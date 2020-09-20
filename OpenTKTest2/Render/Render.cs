using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTKTest2.World.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest2.Render
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

        const int DRAW_LIMIT = 10000;

        public Render(int Width, int Height)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);

            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray();
            shader = new Shader("../../Shader/shader.vert", "../../Shader/shader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SetTextureModes();

            blocktextures = new TextureTiled("../../terrain.png", 16, 16);

            constructor = new NetConstructor(blocktextures);

            World.Chunk chunk = new World.Chunk();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    for (int k = 0; k < 8; k++)
                        if (i + j + k < 10)
                            chunk.SetBlock(new Dirt(), i, j, k);
                        else if (i + j + k == 10)
                            chunk.SetBlock(new Snow(), i, j, k);

            chunk.SetBlock(new Air(), 2, 2, 2);

            camera = new Camera();

            VAO.PushVertexArray(constructor.GetVertexData(chunk.GetRenderFaces()));
        }

        internal void PushMouseState(int dx, int dy)
        {
            camera.HandleInput((float)dx / 100f, -(float)dy / 100f);
        }

        public void SetCameraPos(Vector3 pos)
        {
            camera.SetCameraPos(pos);
        }

        public void RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.Identity;

            Vector3 initialpos = new Vector3(-5, 2, -5);
            //camera.UpdatePosTarget(initialpos, initialpos + Vector3.UnitZ + Vector3.UnitX);
            //camera.HandleInput(0, 0);

            Matrix4 view = camera.GetViewMat();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 100.0f);
            
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
