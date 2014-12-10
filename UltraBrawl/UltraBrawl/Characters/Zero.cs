﻿using System;
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
            : base(new SpriteSheet(image, zeroNumberOfFrames, 2.0f), zeroCollisionOffset, zeroHitboxOffset, zeroHitboxOffsetFlipped, zeroHitboxOffsetNotFlipped, zeroSpeed, zeroFriction, sound1, sound2, zeroFrameSize)
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
            controller = preset.controller;
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
            smash = true;
        }


        public override void chargedOne()
        {
        }
        public override void chargedTwo()
        {
            smash = false;
        }

    }
}

