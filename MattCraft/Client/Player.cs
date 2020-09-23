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

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                camera.SetCameraPos(value);
                position = value;
            }
        }
        private Vector3 position;

        private float xrot;
        private float yrot;

        float MOUSE_SENSITIVITY = 100f;

        public Player(Vector3 position)
        {
            this.camera = new Camera(position);
            this.Position = position;
        }

        public void OnUpdateFrame(ClientFrameUpdateArgs args)
        {
            HandleInput(args.x / MOUSE_SENSITIVITY, args.y / MOUSE_SENSITIVITY);

            KeyboardState input = Keyboard.GetState();

            float moveval = 0.1f;
            if (input.IsKeyDown(Key.ControlLeft))
                moveval = moveval * 3;

            if (input.IsKeyDown(Key.W))
                Position = Position + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(0f, 0f, -moveval));
            if (input.IsKeyDown(Key.S))
                Position = Position + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(0f, 0f, moveval));
            if (input.IsKeyDown(Key.A))
                Position = Position + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(-moveval, 0f, 0f));
            if (input.IsKeyDown(Key.D))
                Position = Position + (Matrix3.CreateRotationY(camera.GetXRot()) * new Vector3(moveval, 0f, 0f));
            if (input.IsKeyDown(Key.Space))
                Position = Position + new Vector3(0f, moveval, 0f);
            if (input.IsKeyDown(Key.ShiftLeft))
                Position = Position + new Vector3(0f, -moveval, 0f);
        }

        public void HandleInput(float deltax, float deltay)
        {
            float smalldelta = 0.01f;

            xrot += deltax;
            yrot += deltay;
            if (yrot < 0 + smalldelta)
                yrot = 0 + smalldelta;
            if (yrot > Math.PI - smalldelta)
                yrot = (float)Math.PI - smalldelta;

            camera.SetCameraTarget(camera.GetCameraPos() + Matrix3.CreateRotationY(xrot) * Matrix3.CreateRotationX(yrot) * new Vector3(0, 1, 0));

            camera.CalcViewMat();
        }

        public Matrix4 GetViewMatrix()
        {
            return camera.GetViewMat();
        }
    }
}
