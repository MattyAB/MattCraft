﻿using System;
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
                ClientUpdateFrameReturn update = client.OnUpdateFrame(e, getFrameArgs());
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
