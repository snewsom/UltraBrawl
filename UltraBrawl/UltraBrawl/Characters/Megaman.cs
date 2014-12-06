using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
* Represents a GlitchPlayer.
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace UltraBrawl
{
    class Megaman : PlayerCharacter
    {
        // constants for this particular sprite
        static Point megamanNumberOfFrames = new Point(30, 20);
        static CollisionOffset megamanCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset megamanHitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset megamanHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 120, 20);
        static CollisionOffset megamanHitboxOffsetFlipped = new CollisionOffset(100, 10, 20, 120);
        static Vector2 megamanSpeed = new Vector2(160, 32);
        static Vector2 megamanFriction = new Vector2(0.8f, 1f);
        static Point megamanFrameSize = new Point(170, 170);
        



        // constructor
        public Megaman(Texture2D image, SoundEffect sound1, SoundEffect sound2, PlayerIndex playerIndex, PlayerController playerController, Vector2 spawnLoc)
            : base(new SpriteSheet(image, megamanNumberOfFrames, 2.0f), spawnLoc, megamanCollisionOffset, megamanHitboxOffset, megamanHitboxOffsetFlipped, megamanHitboxOffsetNotFlipped, megamanSpeed, megamanFriction, sound1, sound2, megamanFrameSize, playerIndex, playerController)
        {
            if (playerIndex.ToString().Equals("Two") || playerIndex.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                megamanHitboxOffset = megamanHitboxOffsetFlipped;
            }
            base.pcSegmentEndings.Add(new Point(4, 0)); //idle
            base.pcSegmentEndings.Add(new Point(8, 1)); //running
            base.pcSegmentEndings.Add(new Point(7, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(7, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(4, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(2, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(1, 7)); //hit
            base.pcSegmentEndings.Add(new Point(10, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(18, 9)); //charging
            base.pcSegmentEndings.Add(new Point(4, 10)); //superIdle
            base.pcSegmentEndings.Add(new Point(5, 11)); //superRunning
            base.pcSegmentEndings.Add(new Point(9, 12)); //superJumping
            base.pcSegmentEndings.Add(new Point(13, 13)); //superJumpkicking
            base.pcSegmentEndings.Add(new Point(3, 14)); //superPunch
            base.pcSegmentEndings.Add(new Point(6, 15)); //superKick
            base.pcSegmentEndings.Add(new Point(0, 16)); //superBlock
            base.pcSegmentEndings.Add(new Point(2, 16)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(2, 17)); //superHit
            base.knockDownEndFrame = 7;

            base.pcSegmentTimings.Add(70); //idle
            base.pcSegmentTimings.Add(80); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(50); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(100); //blockhit
            base.pcSegmentTimings.Add(200); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(60); //charging
            base.pcSegmentTimings.Add(50); //superIdle
            base.pcSegmentTimings.Add(80); //superRunning
            base.pcSegmentTimings.Add(120); //superJumping
            base.pcSegmentTimings.Add(40); //superJumpkicking
            base.pcSegmentTimings.Add(40); //superPunch
            base.pcSegmentTimings.Add(40); //superKick
            base.pcSegmentTimings.Add(50); //superBlock
            base.pcSegmentTimings.Add(100); //superBlockhit
            base.pcSegmentTimings.Add(100); //superHit
            base.setSegments();

            base.canSuper = false;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 1;
            CHARACTER_NAME = "Megaman";
        }

        public override void charging()
        {

        }


        public override void chargedOne()
        {
        }
        public override void chargedTwo()
        {
        }

    }
}

