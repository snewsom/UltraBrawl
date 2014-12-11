using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace UltraBrawl
{
    class PlayerPreset
    {
        public PlayerIndex index;
        public Vector2 spawn;

        public PlayerPreset(PlayerIndex index, Vector2 spawn)
        {
            this.index = index;
            this.spawn = spawn;
        }
    }
}
