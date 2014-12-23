using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UltraBrawl
{
    class AirManHeavyGust : AutomatedSprite
    {
        public AirManHeavyGust(Texture2D texture, Vector2 position, Boolean flipped)
            : base(new SpriteSheet(texture, new Point(24, 0), 1.0f), position,
            new CollisionOffset(50, 50, 50, 50), new Vector2(8f, 1), flipped)
        {
            PROJECTILE_ID = 8;
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(-64, 0);
                this.position.X = position.X + 300;
              


            }
            else
            {
                this.speed.X = speed.X;
                this.velocity = new Vector2(64, 0);
                this.position.X = position.X-500;

            }
            this.position.Y = position.Y-50;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(170, 170);
            currentFrame.Y = 3;
            spriteSheet.addSegment(frameSize, new Point(0, 5), new Point(5, 5), 10);
            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
