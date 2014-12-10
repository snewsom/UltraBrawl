using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UltraBrawl
{
    class GokuKamehameha : AutomatedSprite
    {
        public GokuKamehameha(Texture2D texture, Vector2 position, Boolean flipped)
            : base(new SpriteSheet(texture, new Point(24, 0), 1.0f), position,
            new CollisionOffset(0, 100, 50, 50), new Vector2(8f, 1), flipped)
        {
            PROJECTILE_ID = 0;
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(-32, 0);
                this.position.X = position.X - 100;
                
            } 
            else
            {
                this.speed.X = speed.X;
                this.velocity = new Vector2(32, 0);
                this.position.X = position.X - 40;
               
            }
            this.position.Y = position.Y - 60;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(170, 170);
            currentFrame.Y = 4;
            spriteSheet.addSegment(frameSize, new Point(0, 4), new Point(2, 4), 10);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
