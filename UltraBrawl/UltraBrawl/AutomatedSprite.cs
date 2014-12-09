using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace UltraBrawl
{
    class AutomatedSprite : Sprite
    {
        protected Vector2 speed;
        protected Vector2 velocity;
        public Vector2 gravity = new Vector2(0.1f, 0.8f);
        Vector2 friction = new Vector2(1f, 1f);
        public bool disable = false;
        Vector2 oldPosition = new Vector2(-1, -1);
        public int PROJECTILE_ID;
        public PlayerCharacter myOwner;

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
                PlayerCharacter otherPlayer = (PlayerCharacter)otherSprite;
                if (otherPlayer != myOwner)
                {
                    if (otherPlayer.currentHealth > 0)
                    {
                        if (PROJECTILE_ID == 4 || PROJECTILE_ID == 2)
                        {
                            otherPlayer.getHit(effects, 1, 1);
                        }
                        else if(PROJECTILE_ID == 1){
                            otherPlayer.getHit(effects, 1, 1);
                        }
                        else {
                            otherPlayer.getHit(effects, 3, 1);
                        }
                        disable = true;
                    }
                }
                Debug.WriteLine(otherSprite.GetType());
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
