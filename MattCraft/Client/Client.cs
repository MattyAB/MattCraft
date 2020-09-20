using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;

namespace MattCraft.Client
{
    public class Client : GameWindow
    {
        Render.Render render;

        public Client(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            render = new Render.Render(Width, Height);

            render.SetCameraPos(new Vector3(2, 2, 2));

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            render.RenderFrame(e);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        int currentcentrex;
        int currentcentrey;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            MouseState state = Mouse.GetState();

            int centrex = Width / 2 + X;
            int centrey = Height / 2 + Y;

            Console.WriteLine("location:" + X + ", " + Y);
            Console.WriteLine("mouse:" + state.X + ", " + state.Y);

            if (Focused)
            {
                if (CursorVisible)
                    CursorVisible = false;
                //render.PushMouseState(state.X - currentcentrex, state.Y - currentcentrey);
                render.PushMouseState(state.X, state.Y);
                Mouse.SetPosition(centrex, centrey);
                currentcentrex = Mouse.GetState().X;
                currentcentrey = Mouse.GetState().Y;
            }
            else
                if (CursorVisible)
                CursorVisible = true;

            base.OnUpdateFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            render.Cleanup();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            render.updateAspect(Width, Height);
            base.OnResize(e);
        }
    }
}