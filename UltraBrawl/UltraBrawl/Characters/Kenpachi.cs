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
    class Kenpachi : PlayerCharacter
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
        public Kenpachi(Texture2D image, SoundEffect chargeSound, SoundEffect sound2)
            : base(new SpriteSheet(image, zeroNumberOfFrames, 2.0f), zeroCollisionOffset, zeroHitboxOffset, zeroHitboxOffsetFlipped, zeroHitboxOffsetNotFlipped, zeroSpeed, zeroFriction, zeroFrameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
            base.pcSegmentEndings.Add(new Point(7, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(5, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(5, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(6, 4)); //punch
            base.pcSegmentEndings.Add(new Point(5, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(1, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(7, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(20, 9)); //charging
            base.knockDownEndFrame = 4;

            base.pcSegmentTimings.Add(100); //idle
            base.pcSegmentTimings.Add(70); //running
            base.pcSegmentTimings.Add(100); //jumping
            base.pcSegmentTimings.Add(50); //jumpkick
            base.pcSegmentTimings.Add(50); //punch
            base.pcSegmentTimings.Add(50); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(50); //charging
            base.setSegments();

            JKvelocity = 500;
            canSpecial = false;
            canJumpKick = true;
            canSmash = true;
            canSuper = false;
            CHARACTER_DAMAGE = 1.5;
            CHARACTER_ID = 9;
            CHARACTER_NAME = "Kenpachi";
        }

        public override void heavyAttack()
        {
            if (effects == SpriteEffects.None)
            {
               velocity.X += 300;
            }
            else
            {
                velocity.X -= 300;
            }
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 1200;

            chargeSoundInstance = chargeSound.CreateInstance();
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
            if (chargeSoundInstance.State == SoundState.Stopped && !chargePlayed)
            {
                chargeSoundInstance.Play();
                chargeSoundInstance.Volume = 0.5f;
                chargePlayed = true;
            }
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

        public override void cancelCharge()
        {
            chargeSoundInstance.Stop(true);
            isSmash = false;
        }

    }
}

