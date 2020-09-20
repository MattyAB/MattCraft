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
            using (Window window = new Window(800, 600))
            {
                window.Run(60.0);
            }
        }
    }
}
