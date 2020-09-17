using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest2.Render
{
    class NetConstructor
    {
        TextureTiled blocktextures;

        public NetConstructor(TextureTiled blocktextures)
        {
            this.blocktextures = blocktextures;
        }

        public float[,] GetVertexData(List<Face> faces)
        {
            float[,] data = new float[faces.Count * 4, 5];

            int faceno = 0;

            foreach(Face face in faces)
            {
                int directiona = 0;
                int directionb = 1;

                switch (face.direction)
                {
                    case 0:
                        directiona = 2;
                        directionb = 1;
                        break;
                    case 1:
                        directiona = 2;
                        directionb = 0;
                        break;
                    case 2:
                        directiona = 0;
                        directionb = 1;
                        break;
                }

                for (int i = 0; i < 4; i++)
                {
                    float[] vertex = new float[] { face.x, face.y, face.z, 0, 0 };

                    if (i == 1 | i == 2)
                        vertex[directiona] += 1;
                    if (i == 2 | i == 3)
                        vertex[directionb] += 1;

                    float[] texturevalues = blocktextures.GetCornerLocation(face.texno, i);
                    vertex[3] = texturevalues[0];
                    vertex[4] = texturevalues[1];

                    for (int j = 0; j < 5; j++)
                        data[faceno * 4 + i, j] = vertex[j];
                }

                faceno += 1;
            }

            return data;
        }
    }

    class Face
    {
        public int x;
        public int y;
        public int z;
        public int direction;
        public int texno;

        // 0 is X. 1 is Y. 2 is Z.

        public Face(int x, int y, int z, int direction, int texno)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.direction = direction;
            this.texno = texno;
        }
    }
}
