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
    class Guile : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point guileNumberOfFrames = new Point(30, 20);
        static CollisionOffset guileCollisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset guileHitboxOffset = new CollisionOffset(100, 10, 40, 120);
        static CollisionOffset guileHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 40, 120);
        static CollisionOffset guileHitboxOffsetFlipped = new CollisionOffset(100, 10, 120, 40);
        static Vector2 guileSpeed = new Vector2(175, 32);
        static Vector2 guileFriction = new Vector2(0.8f, 1f);
        static Point guileFrameSize = new Point(170, 170);
        



        // constructor
        public Guile(Texture2D image, SoundEffect chargeSound, SoundEffect fireSound)
            : base(new SpriteSheet(image, guileNumberOfFrames, 2.0f), guileCollisionOffset, guileHitboxOffset, guileHitboxOffsetFlipped, guileHitboxOffsetNotFlipped, guileSpeed, guileFriction, guileFrameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
            this.fireSound = fireSound;
            pcSegmentEndings.Add(new Point(7, 0)); //idle
            pcSegmentEndings.Add(new Point(5, 1)); //running
            pcSegmentEndings.Add(new Point(3, 2)); //jumping
            pcSegmentEndings.Add(new Point(5, 3)); //jumpkick;
            pcSegmentEndings.Add(new Point(5, 4)); //punch
            pcSegmentEndings.Add(new Point(7, 5)); //kick
            pcSegmentEndings.Add(new Point(0, 6)); //block
            pcSegmentEndings.Add(new Point(1, 6)); //blockhit
            pcSegmentEndings.Add(new Point(3, 7)); //hit
            pcSegmentEndings.Add(new Point(12, 8)); //knockdown
            pcSegmentEndings.Add(new Point(8, 9)); //charging
            knockDownEndFrame = 7;
            fireChargeFrame = 2;

            pcSegmentTimings.Add(80); //idle
            pcSegmentTimings.Add(60); //running
            pcSegmentTimings.Add(120); //jumping
            pcSegmentTimings.Add(50); //jumpkick
            pcSegmentTimings.Add(30); //punch
            pcSegmentTimings.Add(40); //kick
            pcSegmentTimings.Add(50); //block
            pcSegmentTimings.Add(50); //blockhit
            pcSegmentTimings.Add(50); //hit
            pcSegmentTimings.Add(60); //knockdown
            pcSegmentTimings.Add(80); //charging
            setSegments();

            JKknockdown = true;
            canJumpKick = true;
            canFire = true;
            canSuper = false;
            CHARACTER_DAMAGE = 1.1;
            CHARACTER_ID = 3;
            CHARACTER_NAME = "Guile";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 700;

            chargeSoundInstance = chargeSound.CreateInstance();
            fireSoundInstance = fireSound.CreateInstance();
            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = guileHitboxOffsetFlipped;
            }
            else
            {
                effects = SpriteEffects.None;
                hitboxOffset = guileHitboxOffsetNotFlipped;
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
            chargePlayed = false;
            fireSoundInstance.Play();//will want to move this to a new method called cancelCharge so that it will finish if uninterrupted.
            isFire = true;
            hasFired = true;
        }

    }
}

