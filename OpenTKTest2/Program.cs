using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest2
{
    class Program
    {
        static void Main(string[] args)
        { 
            using(Game game = new Game(1920, 1080, "Matt's Test"))
            {
                game.Run(60.0);
            }
        }
    }
}
