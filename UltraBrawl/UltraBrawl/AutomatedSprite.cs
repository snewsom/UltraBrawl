using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UltraBrawl
{
    class AutomatedSprite : Sprite
    {
        protected Vector2 speed;
        protected Vector2 velocity;
        public Vector2 gravity = new Vector2(0, 0.8f);
        Vector2 friction = new Vector2(0.8f, 1f);
        public bool disable = false;
        Vector2 oldPosition = new Vector2(-1, -1);

        public AutomatedSprite(SpriteSheet spriteSheet, Vector2 position, CollisionOffset collisionOffset, Vector2 speed, Boolean flipped)
            : base(spriteSheet, position, collisionOffset)
        {
        }

        public override Vector2 direction
        {
            get
            {
                if (velocity.X < 0)
                    effects = SpriteEffects.FlipHorizontally;
                else if (velocity.X > 0)
                    effects = SpriteEffects.None;

                return speed;
            }
        }

        public override void Collision(Sprite otherSprite)
        {
            if (otherSprite.checkChar())
            {
                PlayerCharacter otherPlayer = (PlayerCharacter) otherSprite;
                otherPlayer.getHit(effects, 3, 1.5);
                disable = true;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // physics

            velocity += direction;
            velocity += gravity;
            velocity *= friction;

            position += velocity;

            if (position.X < -collisionOffset.east)
            {
                disable = true;
            }
            if (position.X > clientBounds.Width - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.X - collisionOffset.west))
            {
                disable = true;
            }


            base.Update(gameTime, clientBounds);
        }
    }
}
