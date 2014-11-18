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
        Vector2 gravity = new Vector2(0, 0.8f);
        Vector2 friction = new Vector2(0.8f, 1f);
        protected bool onGround = false;
        Vector2 oldPosition = new Vector2(-1, -1);

        public AutomatedSprite(SpriteSheet spriteSheet, Vector2 position, CollisionOffset collionOffset, Vector2 speed)
            : base(spriteSheet, position, collionOffset)
        {
            this.speed = speed;
            this.velocity = new Vector2(-1, 0);
        }

        public override Vector2 direction
        {
            get
            {
                /* do we need to flip the image? */
                if (velocity.X < 0)
                    effects = SpriteEffects.FlipHorizontally;
                else if (velocity.X > 0)
                    effects = SpriteEffects.None;

                return speed;
            }
        }

        public override void Collision(Sprite otherSprite)
        {
            // Platform platform = (Platform)otherSprite;
            System.Type type = otherSprite.GetType();

            if (type.ToString().Equals("GameOne.Platform"))
            {
                Platform platform = (Platform)otherSprite;
                if (velocity.Y > 0 && position.Y > platform.collisionRect.Top - this.spriteSheet.scale * (this.spriteSheet.currentSegment.frameSize.Y - this.collisionOffset.south) && this.collisionRect.Bottom + platform.collisionRect.Top > 2)
                {
                    velocity.Y = -1;
                    onGround = true;
                    position.Y = platform.collisionRect.Top - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.Y - collisionOffset.south) + 1;
                }

            }

            else if (type.ToString().Equals("GameOne.VegetaPlayer") || type.ToString().Equals("GameOne.GokuPlayer"))
            {
                Random random = new Random();
                position.X = (float) random.Next(1048);
                position.Y = 0;
            }

        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // physics

            //velocity += direction * speed;
            //velocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //velocity *= friction;
            //position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity += direction;
            velocity += gravity;
            velocity *= friction;

            // Is the sprite standing on something
            onGround = position.Y == oldPosition.Y;
            oldPosition = position;

          //  if (onGround == true)
            //    velocity.Y = 0;

            position += velocity;
            //System.Diagnostics.Debug.WriteLine(direction + ":" + position + ":" + velocity);
            if (position.X < -collisionOffset.east)
            {
                speed *= -1;
                position.X = -collisionOffset.east;
            }
            if (position.Y < -collisionOffset.north)
                position.Y = -collisionOffset.north;
            // If sprite is off the screen, move it back within the game window
            if (position.X > clientBounds.Width - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.X - collisionOffset.west))
            {
                speed *= -1;
                position.X = clientBounds.Width - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.X - collisionOffset.west);
            }
            //System.Diagnostics.Debug.WriteLine(spriteSheet.currentSegment.frameSize + "");
            if (position.Y > clientBounds.Height - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.Y - collisionOffset.south))
            {
                velocity.Y = 0;
                onGround = true;
                position.Y = clientBounds.Height - spriteSheet.scale * (spriteSheet.currentSegment.frameSize.Y - collisionOffset.south);
            }
            else
            {
                onGround = false;
            }


            base.Update(gameTime, clientBounds);
        }
    }
}
