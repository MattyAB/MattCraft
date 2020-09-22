using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MattCraft.Client.Render;

namespace MattCraft.Server.World
{
    public class Chunk
    {
        Block[,,] blockdata;

        public Chunk(Block[,,] blockdata)
        {
            this.blockdata = blockdata;

            AssertBlockData();
        }

        public Chunk()
        {
            Block[,,] blockdata = new Block[16, 16, 16];
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                    for (int k = 0; k < 16; k++)
                        blockdata[i, j, k] = new Blocks.Air();
            this.blockdata = blockdata;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return blockdata[x, y, z];
        }

        public void SetBlock(Block block, int x, int y, int z)
        {
            blockdata[x, y, z] = block;
        }

        public List<Face> GetRenderFaces()
        {
            List<Face> faces = new List<Face>();

            for(int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        if(!blockdata[i,j,k].Transparent())
                        {
                            if (i != 0)
                            {
                                if (blockdata[i - 1, j, k].Transparent())
                                    faces.Add(new Face(i, j, k, 0, blockdata[i, j, k].GetTexNo(0)));
                            }
                            else
                                faces.Add(new Face(i, j, k, 0, blockdata[i, j, k].GetTexNo(0)));
                            if (i != 15)
                            {
                                if (blockdata[i + 1, j, k].Transparent())
                                    faces.Add(new Face(i + 1, j, k, 0, blockdata[i, j, k].GetTexNo(1)));
                            }
                            else
                                faces.Add(new Face(i + 1, j, k, 0, blockdata[i, j, k].GetTexNo(1)));

                            if (j != 0)
                            {
                                if (blockdata[i, j - 1, k].Transparent())
                                    faces.Add(new Face(i, j, k, 1, blockdata[i, j, k].GetTexNo(2)));
                            }
                            else
                                faces.Add(new Face(i, j, k, 1, blockdata[i, j, k].GetTexNo(2)));
                            if (j != 15)
                            {
                                if (blockdata[i, j + 1, k].Transparent())
                                    faces.Add(new Face(i, j + 1, k, 1, blockdata[i, j, k].GetTexNo(3)));
                            }
                            else
                                faces.Add(new Face(i, j + 1, k, 1, blockdata[i, j, k].GetTexNo(3)));

                            if (k != 0)
                            {
                                if (blockdata[i, j, k - 1].Transparent())
                                    faces.Add(new Face(i, j, k, 2, blockdata[i, j, k].GetTexNo(4)));
                            }
                            else
                                faces.Add(new Face(i, j, k, 2, blockdata[i, j, k].GetTexNo(4)));
                            if (k != 15)
                            {
                                if (blockdata[i, j, k + 1].Transparent())
                                    faces.Add(new Face(i, j, k + 1, 2, blockdata[i, j, k].GetTexNo(5)));
                            }
                            else
                                faces.Add(new Face(i, j, k + 1, 2, blockdata[i, j, k].GetTexNo(5)));
                        }
                    }
                }
            }

            return faces;
        }

        private void AssertBlockData()
        {
            foreach (Block block in blockdata)
            {
                if (block == null)
                    throw new Exception("Chunk contains null block!");
            }

            if (blockdata.GetLength(0) != 16 | blockdata.GetLength(1) != 16 | blockdata.GetLength(2) != 16)
                throw new Exception("Chunk contains block data of the wrong length: " + blockdata.GetLength(0) + ", " + blockdata.GetLength(1) + ", " + blockdata.GetLength(2));
        }
    }
}
