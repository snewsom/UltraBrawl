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
    class Zero : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point zeroNumberOfFrames = new Point(30, 20);
        static CollisionOffset zeroCollisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset zeroHitboxOffset = new CollisionOffset(100, 10, 15, 100);
        static CollisionOffset zeroHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 15, 100);
        static CollisionOffset zeroHitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 15);
        static Vector2 zeroSpeed = new Vector2(320, 32);
        static Vector2 zeroFriction = new Vector2(0.75f, 1f);
        static Point zeroFrameSize = new Point(170, 170);
        



        // constructor
        public Zero(Texture2D image, SoundEffect sound1, SoundEffect sound2)
            : base(new SpriteSheet(image, zeroNumberOfFrames, 2.0f), zeroCollisionOffset, zeroHitboxOffset, zeroHitboxOffsetFlipped, zeroHitboxOffsetNotFlipped, zeroSpeed, zeroFriction, zeroFrameSize)
        {
            base.pcSegmentEndings.Add(new Point(7, 0)); //idle
            base.pcSegmentEndings.Add(new Point(9, 1)); //running
            base.pcSegmentEndings.Add(new Point(5, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(7, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(9, 4)); //punch
            base.pcSegmentEndings.Add(new Point(7, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(1, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(7, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(20, 9)); //charging
            base.knockDownEndFrame = 4;

            base.pcSegmentTimings.Add(70); //idle
            base.pcSegmentTimings.Add(35); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(40); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(50); //charging
            base.setSegments();

            JKvelocity = 800;
            canJumpKick = true;
            canSmash = true;
            canSuper = false;
            CHARACTER_DAMAGE = 1.1;
            CHARACTER_ID = 5;
            CHARACTER_NAME = "Zero";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 1200;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = zeroHitboxOffsetFlipped;
            }
            regenHitbox();
            update = true;
        }

        public override void charging()
        {
            if (!isSmash)
            {
                isSmash = true;
            }
            else
            {
                spamTimer = System.Environment.TickCount + 3000;
            }
        }


        public override void chargedOne()
        {
        }
        public override void chargedTwo()
        {
            isSmash = false;
        }

    }
}

