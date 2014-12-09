﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UltraBrawl
{
    class GuileSonicBoom : AutomatedSprite
    {
        public GuileSonicBoom(Texture2D texture, Vector2 position, Boolean flipped)
            : base(new SpriteSheet(texture, new Point(4000, 4000), 1.0f), position,
            new CollisionOffset(50, 50, 50, 50), new Vector2(16f, 0), flipped)
        {
            if (flipped)
            {
                this.speed = speed * -1;
                this.velocity = new Vector2(-1, 0);
                this.position.X = position.X - 190;
            }
            else
            {
                this.speed = speed;
                this.velocity = new Vector2(1, 0);
                this.position.X = position.X + 20;
            }
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(150, 130);
            spriteSheet.addSegment(frameSize, new Point(0, 1), new Point(5, 1), 10);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
