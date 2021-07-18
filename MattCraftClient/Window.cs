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
using MattCraft.Server;

namespace MattCraftClient
{
    class Window : GameWindow
    {
        public Window(int width, int height) : base(width, height, GraphicsMode.Default, "MattCraft")
        {
            
        }

        bool ingame = false;
        MattCraft.Client.Client client;
        MattCraft.Server.Server server;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Development thing so that we go straight into game.
            OnClientLoad();

            // So that the mouse is already normalised and doesn't move the camera a bunch at the start. also this doesn't work ...
            Mouse.SetPosition(Width / 2 + X, Height / 2 + Y);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if(ingame)
                client.OnRenderFrame(e);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }


        Vector3 previouspos;
        int[] lookingat = new int[] { 0, 0, 0, 0 };
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if(ingame)
            {
                MouseState mousestate = Mouse.GetState();
                GameUpdate serverupdate = server.UpdateServer(e, previouspos, mousestate, lookingat);
                ClientUpdateFrameReturn update = client.OnUpdateFrame(e, getFrameArgs(), serverupdate);
                previouspos = update.gameupdate.playerpos;
                lookingat = client.GetLookingAt();
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
            server = new Server();
            client = new Client(Width, Height, server.GetFullChunkData(), server.GetPlayerPos());
            ingame = true;
        }

        ClientFrameUpdateArgs getFrameArgs()
        {
            ClientFrameUpdateArgs args = new ClientFrameUpdateArgs();

            args.height = Height;
            args.width = Width;
            args.x = Mouse.GetCursorState().X - (Width / 2 + X);
            args.y = Mouse.GetCursorState().Y - (Height / 2 + Y);
            args.cursorVisible = CursorVisible;
            args.focused = Focused;

            return args;
        }

        void handleFrameReturn(ClientUpdateFrameReturn returned)
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
