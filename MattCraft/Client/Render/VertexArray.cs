using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MattCraft.Client.Render
{
    class VertexArray
    {
        int VAO;
        int VBO;

        public VertexArray()
        {
            GenVertexBuffer();

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GLError.PrintError("Post VAO Creation");
        }

        public void PushVertexArray(float[,] data)
        {
            GLError.PrintError("testtest");
            GL.BindVertexArray(VAO);
            GLError.PrintError("Post vao creation");

            
            int returner;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out returner);
            Console.WriteLine(returner);

            GLError.PrintError("Pre buffer data sub");
            GL.BufferSubData<float>(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * data.Length, data);
            GLError.PrintError("Post buffer data sub");
        }

        const int MAX_VERTICES = 100000;
        public void SetupWorldRender()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GLError.PrintError("Pre buffer initialisation");
            GL.BufferData<float>(BufferTarget.ArrayBuffer, MAX_VERTICES * 5 * sizeof(float), new float[0, 0], BufferUsageHint.DynamicDraw);
            GLError.PrintError("Post buffer initialisation");

            // Location
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Texture
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GLError.PrintError("Post world render setup");
        }

        public void SetupBlockViewRender()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GLError.PrintError("Pre buffer initialisation");
            GL.BufferData<float>(BufferTarget.ArrayBuffer, 8 * 3 * sizeof(float), new float[0, 0], BufferUsageHint.StreamDraw);
            GLError.PrintError("Post buffer initialisation");

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GLError.PrintError("Post blockview render setup");
        }

        void GenVertexBuffer()
        {
            VBO = GL.GenBuffer();
            GLError.PrintError("Post vbo generation");
        }

        public void BindVAO()
        {
            GLError.PrintError("Pre vao binding");
            GL.BindVertexArray(VAO);
            GLError.PrintError("Post vao binding");
        }

        public void CleanUp()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
            GLError.PrintError("Post vbo cleanup");
        }
    }
}
