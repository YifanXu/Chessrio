using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public struct Coords
    {
        public int x, y;

        public Coords (int p1, int p2)
        {
            x = p1;
            y = p2;
        }
        
        public bool IsValid
        {
            get
            {
                return x >= 0 && x <= 7 && y >= 0 && y <= 7;
            }
        }
    }
}
