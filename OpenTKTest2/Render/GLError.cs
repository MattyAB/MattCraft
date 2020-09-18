using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace OpenTKTest2.Render
{
    class GLError
    {
        public static void PrintError()
        {
            ErrorCode error = GL.GetError();

            if (error == ErrorCode.NoError)
            { 
                
            }
            else
                Console.WriteLine(error);
        }
    }
}
