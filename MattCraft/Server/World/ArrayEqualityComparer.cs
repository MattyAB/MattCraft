using System.Collections.Generic;

namespace MattCraft.Server.World
{
    public class ArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            int result = 17;
            for(int i = 0; i < obj.Length; i++)
            {
                result = result * 23 + obj[i];
            }
            return result;
        }
    }
}