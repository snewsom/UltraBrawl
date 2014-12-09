using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UltraBrawl
{
    class LyndisArrow : AutomatedSprite
    {
        public LyndisArrow(Texture2D texture, Vector2 position, Boolean flipped)
            : base(new SpriteSheet(texture, new Point(24, 0), 1.0f), position,
            new CollisionOffset(10, 160, 70, 70), new Vector2(20f, 1), flipped)
        {
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(-64, 0);
                this.position.X = position.X - 190;
                
            } 
            else
            {
                this.speed.X = speed.X;
                this.velocity = new Vector2(64, 0);
                this.position.X = position.X + 30;
               
            }
            this.position.Y = position.Y - 60;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(170, 170);
            currentFrame.Y = 3;
            spriteSheet.addSegment(frameSize, new Point(0, 3), new Point(0, 3), 10);
            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
