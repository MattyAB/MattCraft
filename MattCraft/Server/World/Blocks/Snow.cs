using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattCraft.Server.World.Blocks
{
    class Snow : Block
    {
        public Snow() : base(2, 180, false)
        {

        }

        public override int GetTexNo(int faceno)
        {
            if (faceno == 3)
                return 178;
            else if (faceno == 2)
                return 242;
            else return base.GetTexNo(faceno);
        }
    }
}
