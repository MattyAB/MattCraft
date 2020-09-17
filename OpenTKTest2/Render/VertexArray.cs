using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace OpenTKTest2.Render
{
    class VertexArray
    {
        int maxvertices = 100;

        int VAO;
        int VBO;

        public VertexArray()
        {
            GenVertexBuffer();

            VAO = GL.GenBuffer();
            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, maxvertices * 5 * sizeof(float), new float[0,0], BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        public void PushVertexArray(float[,] data)
        {
            GL.BindVertexArray(VAO);
            GL.BufferSubData<float>(BufferTarget.ArrayBuffer, (IntPtr)0, sizeof(float) * data.Length, data);
        }

        void GenVertexBuffer()
        {
            VBO = GL.GenBuffer();
        }

        public void BindVAO()
        {
            GL.BindVertexArray(VAO);
        }

        public void CleanUp()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
        }
    }
}
