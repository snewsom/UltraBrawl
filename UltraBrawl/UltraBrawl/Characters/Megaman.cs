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
    class Megaman : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point numFrames = new Point(30, 20);
        static CollisionOffset collisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset hitboxOffset = new CollisionOffset(100, 10, 40, 100);
        static CollisionOffset hitboxOffsetFlipped = new CollisionOffset(100, 10, 40, 100);
        static CollisionOffset hitboxOffsetNotFlipped = new CollisionOffset(100, 10, 100, 40);
        static Vector2 speed = new Vector2(200, 32);
        static Vector2 friction = new Vector2(0.8f, 1f);
        static Point frameSize = new Point(170, 170);
        



        // constructor
        public Megaman(Texture2D image, SoundEffect chargeSound, SoundEffect fireSound)
            : base(new SpriteSheet(image, numFrames, 2.0f), collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction, frameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
            this.fireSound = fireSound;
            base.pcSegmentEndings.Add(new Point(8, 0)); //idle
            base.pcSegmentEndings.Add(new Point(8, 1)); //running
            base.pcSegmentEndings.Add(new Point(7, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(7, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(4, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(2, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(1, 7)); //hit
            base.pcSegmentEndings.Add(new Point(10, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(16, 9)); //charging
            base.knockDownEndFrame = 7;
            fireChargeFrame = 12;
            fireFrame = 12;

            base.pcSegmentTimings.Add(90); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(50); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(100); //blockhit
            base.pcSegmentTimings.Add(200); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(30); //charging
            base.setSegments();

            JKvelocity = 200;
            JKknockdown = true;
            canJumpKick = true;
            canFire = true;
            canBlock = true;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 1;
            CHARACTER_NAME = "Megaman";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 400;

            fireSoundInstance = fireSound.CreateInstance();
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
            update = true;
            regenHitbox();
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
            chargeSoundInstance.Stop(true);
            fireSoundInstance.Play();
            isFire = true;
            hasFired = true;
        }
        public override void cancelCharge()
        {
            chargeSoundInstance.Stop(true);
        }

    }
}

