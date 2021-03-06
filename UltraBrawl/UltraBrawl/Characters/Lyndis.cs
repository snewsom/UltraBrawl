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
    class Lyndis : PlayerCharacter
    {
        // constants for this particular sprite
        //static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point numFrames = new Point(30, 20);
        static CollisionOffset collisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset hitboxOffset = new CollisionOffset(100, 10, 10, 100);
        static CollisionOffset hitboxOffsetNotFlipped = new CollisionOffset(100, 10, 10, 100);
        static CollisionOffset hitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 10);
        static Vector2 speed = new Vector2(200, 32);
        static Vector2 friction = new Vector2(0.8f, 1f);
        static Point frameSize = new Point(170, 170);
        



        // constructor
        public Lyndis(Texture2D image, SoundEffect chargeSound, SoundEffect sound2)
            : base(new SpriteSheet(image, numFrames, 2.0f), collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction, frameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
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
            base.knockDownEndFrame = 8;
            fireChargeFrame = 18;
            fireFrame = 18;

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
            base.setSegments();

            canJumpSpecial = true;
            JKknockdown = true;
            canJumpKick = true;
            canFire = true;
            canBlock = true;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 7;
            CHARACTER_NAME = "Lyndis";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 400;

            chargeSoundInstance = chargeSound.CreateInstance();
            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = hitboxOffsetFlipped;
            }
            else
            {
                effects = SpriteEffects.None;
                hitboxOffset = hitboxOffsetNotFlipped;
            }
            regenHitbox();
            update = true;
        }

        public override void charging()
        {
            if (chargeSoundInstance.State == SoundState.Stopped && !chargePlayed)
            {
                chargeSoundInstance.Play();
                chargeSoundInstance.Volume = 0.5f;
                chargePlayed = true;
            }
        }


        public override void chargedOne()
        {
        }
        public override void chargedTwo()
        {
            isFire = true;
            hasFired = true;
        }
        public override void cancelCharge()
        {
            chargeSoundInstance.Stop(true);
        }
    }
}

