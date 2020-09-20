using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Server.World
{
    abstract class Block
    {
        int id;
        int texID;
        bool transparent;

        public Block(int id, int texID, bool transparent)
        {
            this.id = id;
            this.texID = texID;
            this.transparent = transparent;
        }

        public bool Transparent()
        {
            return transparent;
        }

        virtual public int GetTexNo(int faceno)
        {
            return texID;
        }

        public int ID()
        {
            return id;
        }
    }
}
