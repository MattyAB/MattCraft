using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using MattCraft.Client.Render;
using MattCraft.Server.World;

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

        private float xrot = 0;
        private float yrot = 0;

        float MOUSE_SENSITIVITY = 500f;

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
                Position = Position + (Matrix3.CreateRotationY(xrot) * new Vector3(0f, 0f, -moveval));
            if (input.IsKeyDown(Key.S))
                Position = Position + (Matrix3.CreateRotationY(xrot) * new Vector3(0f, 0f, moveval));
            if (input.IsKeyDown(Key.A))
                Position = Position + (Matrix3.CreateRotationY(xrot) * new Vector3(-moveval, 0f, 0f));
            if (input.IsKeyDown(Key.D))
                Position = Position + (Matrix3.CreateRotationY(xrot) * new Vector3(moveval, 0f, 0f));
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

        public int[] GetLookingAt(Dictionary<int[], Chunk> localchunks)
        {
            Vector3 ViewDirection = camera.GetDirectionMatrix();

            int xpolarity = (ViewDirection.X >= 0) ? 1 : -1;
            int ypolarity = (ViewDirection.Y >= 0) ? 1 : -1;
            int zpolarity = (ViewDirection.Z >= 0) ? 1 : -1;

            // X direction
            bool xfinished = false;
            int[] xcoords = new int[3];
            float xdist = 1000000;
            for (int xpos = (xpolarity == -1) ? (int)Math.Floor(position.X) : (int)Math.Floor(position.X + 1); Math.Abs(xpos - position.X) <= Math.Abs(ViewDirection.X * 10) && xfinished == false; xpos += xpolarity)
            {
                float distance = (xpos - position.X) / ViewDirection.X;
                if (distance < 20)
                {
                    Vector3 newpos = new Vector3(xpos, position.Y + distance * ViewDirection.Y, position.Z + distance * ViewDirection.Z);

                    Vector3 blocklocationpos = new Vector3((xpolarity == 1) ? newpos.X : newpos.X - 1, newpos.Y, newpos.Z);

                    int[] chunkcoords = new int[] { (int)Floor(blocklocationpos).X / 16, (int)Floor(blocklocationpos).Y / 16, (int)Floor(blocklocationpos).Z / 16 };
                    int[] blockcoords = new int[] { (int)Floor(blocklocationpos).X % 16, (int)Floor(blocklocationpos).Y % 16, (int)Floor(blocklocationpos).Z % 16 };
                    if (blockcoords[0] < 0)
                        blockcoords[0] = 16 + blockcoords[0];
                    if (blockcoords[1] < 0)
                        blockcoords[1] = 16 + blockcoords[1];
                    if (blockcoords[2] < 0)
                        blockcoords[2] = 16 + blockcoords[2];

                    foreach (KeyValuePair<int[], Chunk> chunkpair in localchunks)
                    {
                        // TODO: Implement IEqualityComparer to compare chunk keys properly
                        if (chunkpair.Key[0] == chunkcoords[0] &&
                            chunkpair.Key[1] == chunkcoords[1] &&
                            chunkpair.Key[2] == chunkcoords[2])
                        {
                            Chunk chunk = chunkpair.Value;
                            Block block = chunk.GetBlock(blockcoords);
                            if (!block.Transparent())
                            {
                                xfinished = true;
                                xcoords[0] = (int)blocklocationpos.X;
                                xcoords[1] = (int)Math.Floor(blocklocationpos.Y);
                                xcoords[2] = (int)Math.Floor(blocklocationpos.Z);
                                xdist = distance;
                            }
                        }
                    }
                }
            }

            // Y direction
            bool yfinished = false;
            int[] ycoords = new int[3];
            float ydist = 1000000;
            for (int ypos = (ypolarity == -1) ? (int)Math.Floor(position.Y) : (int)Math.Floor(position.Y + 1); Math.Abs(ypos - position.Y) <= Math.Abs(ViewDirection.Y * 10) && yfinished == false; ypos += ypolarity)
            {
                float distance = (ypos - position.Y) / ViewDirection.Y;
                if (distance < 20)
                {
                    Vector3 newpos = new Vector3(position.X + distance * ViewDirection.X, ypos, position.Z + distance * ViewDirection.Z);

                    Vector3 blocklocationpos = new Vector3(newpos.X, (ypolarity == 1) ? newpos.Y : newpos.Y - 1, newpos.Z);

                    int[] chunkcoords = new int[] { (int)Floor(blocklocationpos).X / 16, (int)Floor(blocklocationpos).Y / 16, (int)Floor(blocklocationpos).Z / 16 };
                    int[] blockcoords = new int[] { (int)Floor(blocklocationpos).X % 16, (int)Floor(blocklocationpos).Y % 16, (int)Floor(blocklocationpos).Z % 16 };
                    if (blockcoords[0] < 0)
                        blockcoords[0] = 16 + blockcoords[0];
                    if (blockcoords[1] < 0)
                        blockcoords[1] = 16 + blockcoords[1];
                    if (blockcoords[2] < 0)
                        blockcoords[2] = 16 + blockcoords[2];

                    foreach (KeyValuePair<int[], Chunk> chunkpair in localchunks)
                    {
                        // TODO: Implement IEqualityComparer to compare chunk keys properly
                        if (chunkpair.Key[0] == chunkcoords[0] &&
                            chunkpair.Key[1] == chunkcoords[1] &&
                            chunkpair.Key[2] == chunkcoords[2])
                        {
                            Chunk chunk = chunkpair.Value;
                            Block block = chunk.GetBlock(blockcoords);
                            if (!block.Transparent())
                            {
                                yfinished = true;
                                ycoords[0] = (int)Math.Floor(blocklocationpos.X);
                                ycoords[1] = (int)blocklocationpos.Y;
                                ycoords[2] = (int)Math.Floor(blocklocationpos.Z);
                                ydist = distance;
                            }
                        }
                    }
                }
            }

            // Z direction
            bool zfinished = false;
            int[] zcoords = new int[3];
            float zdist = 1000000;
            for (int zpos = (zpolarity == -1) ? (int)Math.Floor(position.Z) : (int)Math.Floor(position.Z + 1); Math.Abs(zpos - position.Z) <=  Math.Abs(ViewDirection.Z * 10) && zfinished == false; zpos += zpolarity)
            {
                //float zpos = (float)Math.Floor(position.Z + i / ViewDirection.Z);
                float distance = (zpos - position.Z) / ViewDirection.Z;
                if (distance < 20)
                {
                    Vector3 newpos = new Vector3(position.X + distance * ViewDirection.X, position.Y + distance * ViewDirection.Y, zpos);

                    Vector3 blocklocationpos = new Vector3(newpos.X, newpos.Y, (zpolarity == 1) ? newpos.Z : newpos.Z - 1);
                    
                    int[] chunkcoords = new int[] { (int)Floor(blocklocationpos).X / 16, (int)Floor(blocklocationpos).Y / 16, (int)Floor(blocklocationpos).Z / 16 };
                    int[] blockcoords = new int[] { (int)Floor(blocklocationpos).X % 16, (int)Floor(blocklocationpos).Y % 16, (int)Floor(blocklocationpos).Z % 16 };
                    if (blockcoords[0] < 0)
                        blockcoords[0] = 16 + blockcoords[0];
                    if (blockcoords[1] < 0)
                        blockcoords[1] = 16 + blockcoords[1];
                    if (blockcoords[2] < 0)
                        blockcoords[2] = 16 + blockcoords[2];

                    foreach (KeyValuePair<int[], Chunk> chunkpair in localchunks)
                    {
                        // TODO: Implement IEqualityComparer to compare chunk keys properly
                        if (chunkpair.Key[0] == chunkcoords[0] &&
                            chunkpair.Key[1] == chunkcoords[1] &&
                            chunkpair.Key[2] == chunkcoords[2])
                        {
                            Chunk chunk = chunkpair.Value;
                            Block block = chunk.GetBlock(blockcoords);
                            if (!block.Transparent())
                            {
                                zfinished = true;
                                zcoords[0] = (int)Math.Floor(blocklocationpos.X);
                                zcoords[1] = (int)Math.Floor(blocklocationpos.Y);
                                zcoords[2] = (int)blocklocationpos.Z; 
                                zdist = distance;
                            }
                        }
                    }
                }
            }

            int[] coords;
            if (xdist <= ydist && xdist <= zdist)
            {
                coords = xcoords;
                Console.WriteLine("X");
            }
            else if (ydist <= zdist && ydist <= xdist)
            {
                coords = ycoords;
                Console.WriteLine("Y");
            }
            else
            {
                coords = zcoords;
                Console.WriteLine("Z");
            }


            // The distance between this coordinate and the next integer boundary
            float xboundary = (Position.X - Floor(Position).X);
            xboundary = (xpolarity == 1) ? 1 - xboundary : xboundary;
            float yboundary = (Position.Y - Floor(Position).Y);
            yboundary = (ypolarity == 1) ? 1 - yboundary : yboundary;
            float zboundary = (Position.Z - Floor(Position).Z);
            zboundary = (zpolarity == 1) ? 1 - zboundary : zboundary;

            Console.WriteLine("X " + xcoords[0] + ", " + xcoords[1] + ", " + xcoords[2]);
            Console.WriteLine("Y " + ycoords[0] + ", " + ycoords[1] + ", " + ycoords[2]);
            Console.WriteLine("Z " + zcoords[0] + ", " + zcoords[1] + ", " + zcoords[2]);


            //return new int[] { 1, 0, 0 };
            return coords;
        }

        public Vector3 Floor(Vector3 vec)
        {
            return new Vector3((float)Math.Floor(vec.X),
                (float)Math.Floor(vec.Y),
                (float)Math.Floor(vec.Z));
        }

        public Matrix4 GetViewMatrix()
        {
            return camera.GetViewMat();
        }
    }
}




/**
 * 
 * 

            // 10 is our view distance.
            for (int i = 0; i < 1; i++)
            {
                float xdist = i + xboundary;
                Vector3 xsearch = position + new Vector3(xdist * xpolarity, (ViewDirection.Y / ViewDirection.X) * xdist * ypolarity, (ViewDirection.Z / ViewDirection.X) * xdist * zpolarity);
                vectors.Add(xsearch);
                //Console.WriteLine(xsearch);

                if (vectors[vectors.Count - 1].X - Math.Floor(vectors[vectors.Count - 1].X) == 0f |
                    vectors[vectors.Count - 1].Y - Math.Floor(vectors[vectors.Count - 1].Y) == 0f |
                    vectors[vectors.Count - 1].Z - Math.Floor(vectors[vectors.Count - 1].Z) == 0f)
                {

                }
                else
                {
                    throw new Exception("Vector stored with no integer value: " + vectors[vectors.Count - 1]);
                }



                int[] chunkcoords = new int[] { (int)Floor(xsearch).X / 16, (int)Floor(xsearch).Y / 16, (int)Floor(xsearch).Z / 16 };
                int[] blockcoords = new int[] { (int)Floor(xsearch).X % 16, (int)Floor(xsearch).Y % 16, (int)Floor(xsearch).Z % 16 };
                if (blockcoords[0] < 0)
                    blockcoords[0] = 16 + blockcoords[0];
                if (blockcoords[1] < 0)
                    blockcoords[1] = 16 + blockcoords[1];
                if (blockcoords[2] < 0)
                    blockcoords[2] = 16 + blockcoords[2];

                //Console.WriteLine(chunkcoords[0] + ", " + chunkcoords[1] + ", " + chunkcoords[2]);

                foreach(KeyValuePair<int[], Chunk> chunkpair in localchunks)
                {
                    // TODO: Implement IEqualityComparer to compare chunk keys properly
                    if (chunkpair.Key[0] == chunkcoords[0] &&
                        chunkpair.Key[1] == chunkcoords[1] &&
                        chunkpair.Key[2] == chunkcoords[2])
                    {
                        Chunk chunk = chunkpair.Value;
                        Block block = chunk.GetBlock(blockcoords);
                        if (!block.Transparent())
                            Console.WriteLine("This one!! " + (xsearch - position).Length);

                    }
                }


                /**
                Vector3 difference;
                if (distancex <= distancey && distancex <= distancez)
                    difference = Vector3.UnitX * xpolarity;
                else if (distancey <= distancex && distancey <= distancez)
                    difference = Vector3.UnitY * ypolarity;
                else
                    difference = Vector3.UnitZ * zpolarity;

                Vector3 closest;
                if (distancex <= distancey && distancex <= distancez)
                    closest = attemptx;
                else if (distancey <= distancex && distancey <= distancez)
                    closest = attempty;
                else
                    closest = attemptz;

                if()**/