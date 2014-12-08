using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UltraBrawl
{
    class MegamanBuster : AutomatedSprite
    {
        public MegamanBuster(Texture2D texture, Vector2 position, Boolean flipped)
            : base(new SpriteSheet(texture, new Point(24, 0), 1.0f), position,
            new CollisionOffset(50, 50, 50, 50), new Vector2(8f, 1), flipped)
        {
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(-16, 0);
                this.position.X = position.X - 190;
                
            } 
            else
            {
                this.speed.X = speed.X;
                this.velocity = new Vector2(16, 0);
                this.position.X = position.X + 20;
               
            }
            this.position.Y = position.Y - 60;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(150, 130);
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 0), 10);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
