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
            : base(new SpriteSheet(texture, new Point(0, 3), 1.0f), position,
            new CollisionOffset(20, 200, 0, 0), new Vector2(8f, 1), flipped)
        {
            PROJECTILE_ID = 0;
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(0, 0);
                this.effects = SpriteEffects.FlipHorizontally;
                this.position.X = position.X - 2000;
                
            } 
            else
            {
                
                this.speed.X = speed.X;
                this.velocity = new Vector2(0, 0);
                this.position.X = position.X - 40;
               
            }
            this.position.Y = position.Y - 60;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(2000, 170);
            currentFrame.Y = 0;
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 2), 30);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
