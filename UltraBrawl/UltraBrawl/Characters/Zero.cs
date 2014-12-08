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
        static CollisionOffset zeroHitboxOffset = new CollisionOffset(100, 10, 40, 100);
        static CollisionOffset zeroHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 120, 40);
        static CollisionOffset zeroHitboxOffsetFlipped = new CollisionOffset(100, 10, 40, 120);
        static Vector2 zeroSpeed = new Vector2(300, 32);
        static Vector2 zeroFriction = new Vector2(0.8f, 1f);
        static Point zeroFrameSize = new Point(170, 170);
        



        // constructor
        public Zero(Texture2D image, SoundEffect sound1, SoundEffect sound2)
            : base(new SpriteSheet(image, zeroNumberOfFrames, 2.0f), zeroCollisionOffset, zeroHitboxOffset, zeroHitboxOffsetFlipped, zeroHitboxOffsetNotFlipped, zeroSpeed, zeroFriction, sound1, sound2, zeroFrameSize)
        {
            base.pcSegmentEndings.Add(new Point(4, 0)); //idle
            base.pcSegmentEndings.Add(new Point(11, 1)); //running
            base.pcSegmentEndings.Add(new Point(10, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(16, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(16, 4)); //punch
            base.pcSegmentEndings.Add(new Point(16, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(1, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(2, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(4, 0)); //"charging"
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
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(25); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(100); //blockhit
            base.pcSegmentTimings.Add(200); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(70); //charging
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

            canSuper = false;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 1;
            CHARACTER_NAME = "Zero";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            controller = preset.controller;
            chargeMax = 800;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = zeroHitboxOffsetFlipped;
            }
            update = true;
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

