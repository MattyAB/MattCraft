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

        int width;
        int height;

        float deg = 30.0f;

        const int DRAW_LIMIT = 10000;

        public Render(int Width, int Height)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);

            TextureTiled blocktextures = new TextureTiled("../../terrain.png", 16, 16);

            NetConstructor constructor = new NetConstructor(blocktextures);

            World.Chunk chunk = new World.Chunk();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    for (int k = 0; k < 8; k++)
                        if(i+j+k < 10)
                            chunk.SetBlock(new Dirt(), i, j, k);
                        else if(i+j+k == 10)
                            chunk.SetBlock(new Snow(), i, j, k);

            chunk.SetBlock(new Air(), 2, 2, 2);

            GLError.PrintError();
            //VAO = new VertexArray(constructor.GetVertexData(faces));
            VAO = new VertexArray(constructor.GetVertexData(chunk.GetRenderFaces()));
            shader = new Shader("../../Shader/shader.vert", "../../Shader/shader.frag");

            this.width = Width;
            this.height = Height;

            GL.Enable(EnableCap.DepthTest);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SetTextureModes();

            //Texture texture = new Texture("../../TextureA.png");
            //texture.Use();

            float[,] initialdata = new float[,] 
                { { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },  { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f },  { 1.0f, 1.0f, 0.0f, 1.0f, 1.0f },  { 0.0f, 1.0f, 0.0f, 0.0f, 1.0f },
                  { 0.0f, 0.0f, -0.5f, 0.0f, 0.0f }, { 1.0f, 0.0f, -0.5f, 1.0f, 0.0f }, { 1.0f, 1.0f, -0.5f, 1.0f, 1.0f }, { 0.0f, 1.0f, -0.5f, 0.0f, 1.0f }};

            for (int i = 0; i < 4; i++)
            {
                float[] values = blocktextures.GetCornerLocation(243, i);
                for (int j = 0; j < 2; j++)
                {
                    initialdata[4 * j + i, 3] = values[0];
                    initialdata[4 * j + i, 4] = values[1];
                }
            }

            /**List<Face> faces = new List<Face>();
            for(int i = 0; i < 26; i++) 
                faces.Add(new Face(i, 0, 0, 0, 180));

            NetConstructor constructor = new NetConstructor(blocktextures);**/
            //chunk.SetBlock(new World.Blocks.Snow(), 0, 1, 2);

            //VAO.PushVertexArray(constructor.GetVertexData(chunk.GetRenderFaces()));
        }

        public void RenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(deg)) * Matrix4.CreateRotationX(0.2f);

            deg += 1.0f;

            Matrix4 view = Matrix4.CreateTranslation(0.0f, -1.0f, -20.0f);
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
