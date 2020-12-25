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
        WorldRender worldRender;
        BlockViewRender blockViewRender;

        public Render(int Width, int Height, Dictionary<int[], Chunk> initialchunkdata, Vector3 playerpos)
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);

            SetTextureModes();

            worldRender = new WorldRender(Width, Height, initialchunkdata, playerpos);
            blockViewRender = new BlockViewRender(Width, Height, initialchunkdata, playerpos);
        }

        internal void RenderFrame(FrameEventArgs e, Matrix4 view)
        {
            worldRender.RenderFrame(e, view);
            blockViewRender.RenderFrame(e, view);
        }

        public void UpdateFrame(FrameEventArgs e, ClientFrameUpdateArgs args)
        {
            blockViewRender.UpdateFrame(e, args);
        }

        public void UpdateAspect(int Width, int Height)
        {
            worldRender.UpdateAspect(Width, Height);
            blockViewRender.UpdateAspect(Width, Height);
        }

        public void Cleanup()
        {
            worldRender.Cleanup();
            blockViewRender.Cleanup();
        }

        public void SetTextureModes()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            float[] borderColor = { 1.0f, 0.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GLError.PrintError("Post Texture Mode setup");
        }
    }
}
