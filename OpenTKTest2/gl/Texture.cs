using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpenTKTest2.gl
{
    class Texture
    {
        int Handle;

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            Use();

            Image<Rgba32> image = (Image<Rgba32>)Image.Load(path);

            image.Mutate(Matrix3x2Extensions => Matrix3x2Extensions.Flip(FlipMode.Vertical));

            Span<Rgba32> span;
            image.TryGetSinglePixelSpan(out span);

            Rgba32[] tempPixels = span.ToArray();

            List<byte> pixels = new List<byte>();

            foreach(Rgba32 p in tempPixels)
            {
                pixels.Add(p.R);
                pixels.Add(p.G);
                pixels.Add(p.B);
                pixels.Add(p.A);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, 
                PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        }

        public void Use()
        {
            //GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.ActiveTexture(TextureUnit.Texture0);
        }
    }
}
