using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest2.Render
{
    class TextureTiled : Texture
    {
        int width;
        int height;

        public TextureTiled(string path, int width, int height) : base(path)
        {
            this.width = width;
            this.height = height;
        }

        public float[] GetCornerLocation(int texnumber, int corner)
        {
            int x = texnumber % width;
            int y = texnumber / width;

            bool xplus = false;
            bool yplus = false;

            switch(corner)
            {
                case 0: // TOP LEFT
                    break;
                case 1: // TOP RIGHT
                    xplus = true;
                    break;
                case 2: // BOTTOM RIGHT
                    xplus = true;
                    yplus = true;
                    break;
                case 3: // BOTTOM LEFT
                    yplus = true;
                    break;
            }

            if (xplus)
            {
                if (x == width)
                    x = 0;
                else
                    x += 1;
            }
            if (yplus)
            {
                y += 1;
            }

            float[] result = new float[] { (float)x / (float)width, (float)y / (float)height};
            return result;
        }
    }
}
