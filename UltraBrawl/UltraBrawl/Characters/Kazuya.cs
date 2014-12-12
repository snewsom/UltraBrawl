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
    class Kazuya : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point numFrames = new Point(30, 20);
        static CollisionOffset collisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset hitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset AOEHitboxOffset = new CollisionOffset(0, 0, 0, 0);
        static CollisionOffset hitboxOffsetNotFlipped = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset hitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 20);
        static Vector2 speed = new Vector2(128, 32);
        static Vector2 friction = new Vector2(0.8f, 1f);
        static Point frameSize = new Point(170, 170);
        



        // constructor
        public Kazuya(Texture2D image, SoundEffect chargeSound, SoundEffect sound2)
            : base(new SpriteSheet(image, numFrames, 2.0f), collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction, frameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
            base.pcSegmentEndings.Add(new Point(6, 0)); //idlel
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(5, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(9, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(7, 4)); //punch
            base.pcSegmentEndings.Add(new Point(7, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(1, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(21, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(17, 9)); //charging
            base.knockDownEndFrame = 9;
            //

            base.pcSegmentTimings.Add(100); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(40); //jumpkick
            base.pcSegmentTimings.Add(35); //punch
            base.pcSegmentTimings.Add(45); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(80); //charging
            base.setSegments();

            chargeMax = 1400;
            canJumpKick = true;
            canAOE = true;
            CHARACTER_DAMAGE = 1.2;
            CHARACTER_ID = 6;
            CHARACTER_NAME = "Kazuya";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;

            chargeSoundInstance = chargeSound.CreateInstance();
            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = hitboxOffsetFlipped;
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
            if (!isAOE)
            {
                velocity.Y = 0;
                isAOE = true;
                hitboxOffset = AOEHitboxOffset;
                regenHitbox();
            }
            else
            {
                spamTimer = System.Environment.TickCount + 5000;
            }
        }


        public override void chargedOne()
        {
        }
        public override void chargedTwo()
        {
            if (effects == SpriteEffects.FlipHorizontally)
            {
                hitboxOffset = hitboxOffsetFlipped;
            }
            else
            {
                hitboxOffset = hitboxOffsetNotFlipped;
            }
            regenHitbox();
            isAOE = false;
        }

        public override void cancelCharge()
        {
            if (effects == SpriteEffects.FlipHorizontally)
            {
                hitboxOffset = hitboxOffsetFlipped;
            }
            else
            {
                hitboxOffset = hitboxOffsetNotFlipped;
            }
            chargeSoundInstance.Stop(true);
            regenHitbox();
            isAOE = false;
        }

    }
}

