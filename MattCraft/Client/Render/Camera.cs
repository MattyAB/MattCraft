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
        Vector3 up = new Vector3(0f, 1f, 0f);

        Matrix4 view;

        public Camera(Vector3 cameraPos)
        {
            this.cameraPos = cameraPos;
            cameraTarget = new Vector3(0f, 0f, 0f);

            CalcViewMat();
        }

        public void CalcViewMat()
        {
            view = Matrix4.LookAt(cameraPos, cameraTarget, up);
        }

        public void UpdatePosTarget(Vector3 cameraPos, Vector3 cameraTarget)
        {
            this.cameraPos = cameraPos;
            this.cameraTarget = cameraTarget;

            CalcViewMat();
        }

        public void SetCameraTarget(Vector3 cameraTarget)
        {
            this.cameraTarget = cameraTarget;
        }

        public void SetCameraPos(Vector3 pos)
        {
            this.cameraPos = pos;
        }

        public Vector3 GetCameraPos()
        {
            return cameraPos;
        }

        public Matrix4 GetViewMat()
        {
            return view;
        }

        internal Vector3 GetDirectionMatrix()
        {
            return cameraTarget - cameraPos;
        }
    }
}
