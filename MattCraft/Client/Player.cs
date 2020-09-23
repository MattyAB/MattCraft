using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using MattCraft.Client.Render;

namespace MattCraft.Client
{
    class Player
    {
        Camera camera;

        Vector3 position;

        float MOUSE_SENSITIVITY = 100f;

        public Player(Vector3 position)
        {
            this.camera = new Camera(position);
            this.position = position;
        }

        public void OnUpdateFrame(ClientFrameUpdateArgs args)
        {
            camera.HandleInput(args.x / MOUSE_SENSITIVITY, args.y / MOUSE_SENSITIVITY);

            KeyboardState input = Keyboard.GetState();

            float moveval = 0.1f;
            if (input.IsKeyDown(Key.ControlLeft))
                moveval = moveval * 3;

            if (input.IsKeyDown(Key.W))
                camera.SetCameraPos(camera.GetCameraPos() + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(0f, 0f, -moveval)));
            if (input.IsKeyDown(Key.S))
                camera.SetCameraPos(camera.GetCameraPos() + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(0f, 0f, moveval)));
            if (input.IsKeyDown(Key.A))
                camera.SetCameraPos(camera.GetCameraPos() + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(-moveval, 0f, 0f)));
            if (input.IsKeyDown(Key.D))
                camera.SetCameraPos(camera.GetCameraPos() + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(moveval, 0f, 0f)));
            if (input.IsKeyDown(Key.Space))
                camera.SetCameraPos(camera.GetCameraPos() + new Vector3(0f, moveval, 0f));
            if (input.IsKeyDown(Key.ShiftLeft))
                camera.SetCameraPos(camera.GetCameraPos() + new Vector3(0f, -moveval, 0f));
        }

        public Matrix4 GetViewMatrix()
        {
            return camera.GetViewMat();
        }
    }
}
