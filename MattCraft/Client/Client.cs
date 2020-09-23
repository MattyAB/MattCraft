using MattCraft.Server.World;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;

namespace MattCraft.Client
{
    public class Client
    {
        Render.Render render;
        Player player;

        public Client(int width, int height, Dictionary<int[], Chunk> initialchunkdata, Vector3 playerpos)
        {
            render = new Render.Render(width, height, initialchunkdata, playerpos);
            player = new Player(playerpos);
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            render.RenderFrame(e, player.GetViewMatrix());
        }

        public ClientUpdateFrameReturn OnUpdateFrame(FrameEventArgs e, ClientFrameUpdateArgs args)
        {
            ClientUpdateFrameReturn returner = new ClientUpdateFrameReturn();
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
                returner.exit = true;
            else
                returner.exit = false;

            if (args.focused)
            {
                returner.cursorVisible = false;
                returner.resetmouse = true;

                player.OnUpdateFrame(args);
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

    public struct ClientFrameUpdateArgs
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public bool cursorVisible;
        public bool focused;

        public Server.GameUpdate gameupdate;
    }

    public struct ClientUpdateFrameReturn
    {
        public bool exit;
        public bool resetmouse;
        public bool cursorVisible;
        public bool alterCursorVisible;

        public Server.GameUpdate gameupdate;
    }
}