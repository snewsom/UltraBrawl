using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltraBrawl
{
    class CursorLoc
    {
        public int currentItemX;
        public int currentItemY;

        public CursorLoc()
        {
            currentItemX = 0;
            currentItemY = 0;
        }

        public void resetLoc()
        {
            currentItemX = 0;
            currentItemY = 0;
        }
    }
}
