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
    class Lyndis : PlayerCharacter
    {
        // constants for this particular sprite
        //static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point lyndisNumberOfFrames = new Point(30, 20);
        static CollisionOffset lyndisCollisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset lyndisHitboxOffset = new CollisionOffset(100, 10, 10, 100);
        static CollisionOffset lyndisHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 10, 100);
        static CollisionOffset lyndisHitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 10);
        static Vector2 lyndisSpeed = new Vector2(200, 32);
        static Vector2 lyndisFriction = new Vector2(0.8f, 1f);
        static Point lyndisFrameSize = new Point(170, 170);
        



        // constructor
        public Lyndis(Texture2D image, SoundEffect sound1, SoundEffect sound2)
            : base(new SpriteSheet(image, lyndisNumberOfFrames, 2.0f), lyndisCollisionOffset, lyndisHitboxOffset, lyndisHitboxOffsetFlipped, lyndisHitboxOffsetNotFlipped, lyndisSpeed, lyndisFriction, sound1, sound2, lyndisFrameSize)
        {
            base.pcSegmentEndings.Add(new Point(9, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(8, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(9, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(5, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(0, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(2, 7)); //hit
            base.pcSegmentEndings.Add(new Point(14, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(21, 9)); //charging
            base.pcSegmentEndings.Add(new Point(4, 10)); //superIdle
            base.pcSegmentEndings.Add(new Point(5, 11)); //superRunning
            base.pcSegmentEndings.Add(new Point(9, 12)); //superJumping
            base.pcSegmentEndings.Add(new Point(13, 13)); //superJumpkicking
            base.pcSegmentEndings.Add(new Point(3, 14)); //superPunch
            base.pcSegmentEndings.Add(new Point(6, 15)); //superKick
            base.pcSegmentEndings.Add(new Point(0, 16)); //superBlock
            base.pcSegmentEndings.Add(new Point(2, 16)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(2, 17)); //superHit
            base.pcSegmentEndings.Add(new Point(2, 16)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(2, 17)); //superHit
            base.knockDownEndFrame = 8;
            fireChargeFrame = 18;

            base.pcSegmentTimings.Add(70); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(30); //jumpkick
            base.pcSegmentTimings.Add(50); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(20); //charging
            base.pcSegmentTimings.Add(50); //superIdle
            base.pcSegmentTimings.Add(80); //superRunning
            base.pcSegmentTimings.Add(120); //superJumping
            base.pcSegmentTimings.Add(40); //superJumpkicking
            base.pcSegmentTimings.Add(40); //superPunch
            base.pcSegmentTimings.Add(40); //superKick
            base.pcSegmentTimings.Add(50); //superBlock
            base.pcSegmentTimings.Add(100); //superBlockhit
            base.pcSegmentTimings.Add(100); //superHit
            base.pcSegmentTimings.Add(100); //superBlockhit
            base.pcSegmentTimings.Add(100); //superHit
            base.setSegments();

            JKknockdown = true;
            canJumpKick = true;
            canFire = true;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 7;
            CHARACTER_NAME = "Lyndis";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            controller = preset.controller;
            chargeMax = 500;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = lyndisHitboxOffsetFlipped;
            }
            else
            {
                effects = SpriteEffects.None;
                hitboxOffset = lyndisHitboxOffsetNotFlipped;
            }
            regenHitbox();
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
            fire = true;
            hasFired = true;
        }

    }
}

