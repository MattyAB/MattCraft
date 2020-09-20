using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;

namespace MattCraft.Client
{
    public class Client
    {
        Render.Render render;

        public Client(int width, int height)
        {
            render = new Render.Render(width, height);

            render.SetCameraPos(new Vector3(2, 2, 2));
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            render.RenderFrame(e);
        }

        int currentcentrex;
        int currentcentrey;
        public UpdateFrameReturn OnUpdateFrame(FrameEventArgs e, UpdateFrameArgs args)
        {
            UpdateFrameReturn returner = new UpdateFrameReturn();
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                returner.exit = true;
            }
            else
                returner.exit = false;

            MouseState state = Mouse.GetState();

            //Console.WriteLine("mouse:" + args.x + ", " + args.y);

            if (args.focused)
            {
                returner.cursorVisible = false;
                //render.PushMouseState(state.X - currentcentrex, state.Y - currentcentrey);
                render.PushMouseState(args.x, args.y);
                
                returner.resetmouse = true;
                currentcentrex = Mouse.GetState().X;
                currentcentrey = Mouse.GetState().Y;
            }
            else
            {
                returner.cursorVisible = true;
                returner.resetmouse = false;
            }

            returner.alterCursorVisible = returner.cursorVisible ^ args.cursorVisible;

            return returner;
        }

        public void OnUnload()
        {
            render.Cleanup();
        }

        public void UpdateAspect(EventArgs e, int width, int height)
        {
            render.updateAspect(width, height);
        }
    }

    public struct UpdateFrameArgs
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public bool cursorVisible;
        public bool focused;
    }

    public struct UpdateFrameReturn
    {
        public bool exit;
        public bool resetmouse;
        public bool cursorVisible;
        public bool alterCursorVisible;
    }
}