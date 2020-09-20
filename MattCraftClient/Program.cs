using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MattCraft;

using OpenTK;

namespace MattCraftClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(MattCraft.Client.Client client = new MattCraft.Client.Client(800, 600, "MattCraft"))
            {
                client.Run(60.0);
            }
        }
    }
}
