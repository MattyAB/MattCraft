using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest2.Render
{
    public class Render
    {
        gl.VertexArray VAO;
        gl.Shader shader;

        int width;
        int height;

        float deg = 30.0f;

        public Render(int Width, int Height)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);

            VAO = new gl.VertexArray();
            shader = new gl.Shader("../../Shader/shader.vert", "../../Shader/shader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SetTextureModes();

            gl.Texture texture = new gl.Texture("../../TextureA.png");
            texture.Use();
        }

        public void RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(deg));

            deg += 1.0f;

            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 100.0f);
            
            shader.UniformMat4("model", ref model);
            shader.UniformMat4("view", ref view);
            shader.UniformMat4("projection", ref perspective);

            shader.Use();
            VAO.BindVAO();

            GL.DrawArrays(PrimitiveType.Quads, 0, 8);
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
        }
    }
}
