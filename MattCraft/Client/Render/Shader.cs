using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace MattCraft.Client.Render
{
    class Shader
    {
        int Handle;

        int VertexShader;
        int FragmentShader;

        public Shader(string vertpath, string fragpath)
        {
            string VertexShaderSource;

            using (StreamReader reader = new StreamReader(vertpath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragpath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            GLError.PrintError("Post shader init");
        }

        internal void UniformMat4(string name, ref Matrix4 perspective)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(Handle, name), false, ref perspective);
        }

        public void Use()
        {
            GLError.PrintError("Pre shader using");
            GL.UseProgram(Handle);
            GLError.PrintError("Post shader using");
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                GLError.PrintError("Post shader deletion");

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
            GLError.PrintError("Post shader decomp");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GLError.PrintError("Post shader supression");
        }
    }
}
