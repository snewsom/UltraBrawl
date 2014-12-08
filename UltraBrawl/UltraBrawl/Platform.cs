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
    class Platform : Sprite
    {
        static Point platformFrameSize = new Point(400, 40);

        public Platform(Texture2D image, Vector2 position)
            : base(new SpriteSheet(image,new Point(1, 1),0.5f), new Vector2(600, 600),new CollisionOffset(0, 0, 30, 30))
        {
            spriteSheet = new SpriteSheet(image, new Point(1,1), 0.8f);
            spriteSheet.addSegment(platformFrameSize, new Point(0, 0), new Point(18, 0), 50);
            spriteSheet.setCurrentSegment(0);
            this.position = position;
        }

        public override void Collision(Sprite otherSprite)
        {
           
         
        }

        public override Vector2 direction
        {
            get
            {
                return Vector2.Zero;
            }
        }

        /*
         * Update the sprite.
         */
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {


            // Update the sprite (base class)
            base.Update(gameTime, clientBounds);

        }


    }
}
