using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MattCraft.Client.Render
{
    class Camera
    {
        Vector3 cameraPos;
        Vector3 cameraTarget;
        Vector3 cameraDirection;
        Vector3 up = new Vector3(0f, 1f, 0f);

        Matrix4 view;

        float xrot = 143.6f % (float)(Math.PI*2);
        float yrot = (float)(Math.PI / 2);

        public Camera()
        {
            cameraPos = new Vector3(-10f, -10f, -10f);
            cameraTarget = new Vector3(0f, 0f, 0f);

            CalcViewMat();
        }

        public void CalcViewMat()
        {
           // cameraDirection = Vector3.Normalize(cameraPos - cameraTarget); // This is actually the opposite direction to the camera

           // Vector3 cameraRight = Vector3.Normalize(Vector3.Cross(up, cameraDirection)); // This is our camera's x-axis
           // Vector3 cameraUp = Vector3.Cross(cameraDirection, cameraRight); // This is our camera's y-axis

            view = Matrix4.LookAt(cameraPos, cameraTarget, up);
        }

        public void UpdatePosTarget(Vector3 cameraPos, Vector3 cameraTarget)
        {
            this.cameraPos = cameraPos;
            this.cameraTarget = cameraTarget;

            CalcViewMat();
        }

        public void SetCameraPos(Vector3 pos)
        {
            this.cameraPos = pos;
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

            Console.WriteLine(yrot);

            cameraTarget = cameraPos + Matrix3.CreateRotationY(xrot) * Matrix3.CreateRotationX(yrot) * new Vector3(0, 1, 0);

            CalcViewMat();
        }

        public Matrix4 GetViewMat()
        {
            return view;
        }
    }
}
