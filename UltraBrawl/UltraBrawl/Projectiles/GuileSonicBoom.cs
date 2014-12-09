using System;
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
            : base(new SpriteSheet(texture, new Point(2000, 2000), 1.3f), position,
            new CollisionOffset(0, 100, 100, 0), new Vector2(8f, 1), flipped)
        {
            PROJECTILE_ID = 2;
            if (flipped)
            {
                this.speed.X = speed.X * -1;
                this.velocity = new Vector2(-14, 0);
                this.position.X = position.X - 130;

            }
            else
            {
                this.speed.X = speed.X;
                this.velocity = new Vector2(14, 0);
                this.position.X = position.X - 30;

            }
            this.position.Y = position.Y - 60;
            gravity = new Vector2(0, 0);
            Point frameSize = new Point(170, 170);
            currentFrame.Y = 1;
            spriteSheet.addSegment(frameSize, new Point(0, 1), new Point(7, 1), 15);
            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }
    }
}
