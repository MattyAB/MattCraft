using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
using MattCraft;
using MattCraft.Client;

namespace MattCraftClient
{
    class Window : GameWindow
    {
        public Window(int width, int height) : base(width, height, GraphicsMode.Default, "MattCraft")
        {
            
        }

        bool ingame = false;
        MattCraft.Client.Client client;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if(ingame)
                client.OnRenderFrame(e);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if(ingame)
            {
                UpdateFrameReturn update = client.OnUpdateFrame(e, getFrameArgs());
                handleFrameReturn(update);
            }
            else
            {
                KeyboardState input = Keyboard.GetState();
                if(input.IsKeyDown(Key.Space))
                {
                    OnClientLoad();
                }
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            client.OnUnload();

            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            if(ingame)
                client.UpdateAspect(e, Width, Height);
            base.OnResize(e);
        }

        void OnClientLoad()
        {
            client = new Client(Width, Height);
            ingame = true;
        }

        UpdateFrameArgs getFrameArgs()
        {
            UpdateFrameArgs args = new UpdateFrameArgs();

            args.height = Height;
            args.width = Width;
            args.x = Mouse.GetCursorState().X - (Width / 2 + X);
            args.y = Mouse.GetCursorState().Y - (Height / 2 + Y);
            args.cursorVisible = CursorVisible;
            args.focused = Focused;

            return args;
        }

        void handleFrameReturn(UpdateFrameReturn returned)
        {
            if (returned.exit)
                Exit();
            if (returned.alterCursorVisible)
            CursorVisible = returned.cursorVisible;

            if (returned.resetmouse)
            {
                Mouse.SetPosition(Width / 2 + X, Height / 2 + Y);
            }
        }
    }
}
