using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace UltraBrawl
{
    //input detection. Allows for controls to work independant of character.
    class PlayerController
    {

        //constants for all player characters
        public PlayerIndex pcPlayerNum;
        public List<Keys> pcPlayerKeys = new List<Keys>();

        // constructor
        public PlayerController(PlayerIndex playerIndex)
        {
            pcPlayerNum = playerIndex;
            if (playerIndex.Equals(PlayerIndex.Two))
            {
                pcPlayerKeys.Add(Keys.I);
                pcPlayerKeys.Add(Keys.K);
                pcPlayerKeys.Add(Keys.J);
                pcPlayerKeys.Add(Keys.L);
                pcPlayerKeys.Add(Keys.N);
                pcPlayerKeys.Add(Keys.O);
                pcPlayerKeys.Add(Keys.U);
            }
            else if (playerIndex.Equals(PlayerIndex.One))
            {
                pcPlayerKeys.Add(Keys.W);
                pcPlayerKeys.Add(Keys.S);
                pcPlayerKeys.Add(Keys.A);
                pcPlayerKeys.Add(Keys.D);
                pcPlayerKeys.Add(Keys.Space);
                pcPlayerKeys.Add(Keys.E);
                pcPlayerKeys.Add(Keys.Q);
            }
            GamePadState gamepadState = GamePad.GetState(pcPlayerNum);
        }
    }
}