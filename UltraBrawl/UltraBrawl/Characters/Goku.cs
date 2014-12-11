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
    class Goku : PlayerCharacter
    {
        // constants for this particular sprite
        //static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point gokuNumberOfFrames = new Point(20, 22);
        static CollisionOffset gokuCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset gokuHitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset gokuHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset gokuHitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 20);
        static Vector2 gokuSpeed = new Vector2(180, 32);
        static Vector2 gokuFriction = new Vector2(0.8f, 1f);
        static Point gokuFrameSize = new Point(170, 170);
        private SoundEffect superChargeSound;
        



        // constructor
        public Goku(Texture2D image, SoundEffect chargeSound, SoundEffect superLoop, SoundEffect superChargeSound, SoundEffect fireSound)
            : base(new SpriteSheet(image, gokuNumberOfFrames, 2.0f), gokuCollisionOffset, gokuHitboxOffset, gokuHitboxOffsetFlipped, gokuHitboxOffsetNotFlipped, gokuSpeed, gokuFriction, gokuFrameSize)
        {
            hasChargeSound = true;
            this.chargeSound = chargeSound;
            this.superLoop = superLoop;
            this.fireSound = fireSound;
            this.superChargeSound = superChargeSound;
            //if the player cansuper it MUST be declared begore the segments are set.
            canSuper = true;
            base.pcSegmentEndings.Add(new Point(14, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(9, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(14, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(3, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(1, 7)); //blockhit
            base.pcSegmentEndings.Add(new Point(1, 8)); //hit
            base.pcSegmentEndings.Add(new Point(13, 9)); //knockdown
            base.pcSegmentEndings.Add(new Point(18, 10)); //charging
            base.pcSegmentEndings.Add(new Point(4, 11)); //superIdle
            base.pcSegmentEndings.Add(new Point(5, 12)); //superRunning
            base.pcSegmentEndings.Add(new Point(9, 13)); //superJumping
            base.pcSegmentEndings.Add(new Point(13, 14)); //superJumpkicking
            base.pcSegmentEndings.Add(new Point(3, 15)); //superPunch
            base.pcSegmentEndings.Add(new Point(6, 16)); //superKick
            base.pcSegmentEndings.Add(new Point(0, 17)); //superBlock
            base.pcSegmentEndings.Add(new Point(1, 18)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(1, 19)); //superHit
            base.pcSegmentEndings.Add(new Point(12, 20)); //superKnockDown
            base.pcSegmentEndings.Add(new Point(11, 21)); //superCharge
            base.knockDownEndFrame = 6;
            base.fireChargeFrame = 3;
            base.fireFrame = 6;
    
            base.pcSegmentTimings.Add(50); //idle
            base.pcSegmentTimings.Add(80); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(30); //jumpkick
            base.pcSegmentTimings.Add(60); //punch
            base.pcSegmentTimings.Add(60); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(100); //blockhit
            base.pcSegmentTimings.Add(100); //hit
            base.pcSegmentTimings.Add(40); //knockdown
            base.pcSegmentTimings.Add(80); //charging
            base.pcSegmentTimings.Add(50); //superIdle
            base.pcSegmentTimings.Add(80); //superRunning
            base.pcSegmentTimings.Add(120); //superJumping
            base.pcSegmentTimings.Add(30); //superJumpkicking
            base.pcSegmentTimings.Add(40); //superPunch
            base.pcSegmentTimings.Add(40); //superKick
            base.pcSegmentTimings.Add(50); //superBlock
            base.pcSegmentTimings.Add(100); //superBlockhit
            base.pcSegmentTimings.Add(100); //superHit
            base.pcSegmentTimings.Add(40); //superKnockDown
            base.pcSegmentTimings.Add(70); //superCharge
            base.setSegments();

            JKknockdown = true;
            canJumpKick = true;
            CHARACTER_ID = 0;
            CHARACTER_NAME = "Goku";
            CHARACTER_DAMAGE = 1.2;
        }
        public override void projTimer()
        {
            if(pauseTime < disableTimer){
                disableProjectile = true;
                isBeam = false;
            }
        }
        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;

            chargeSoundInstance = chargeSound.CreateInstance();
            fireSoundInstance = fireSound.CreateInstance();
            superLoopInstance = superLoop.CreateInstance();
            superLoopInstance.IsLooped = true;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = gokuHitboxOffsetFlipped;
            }
            else
            {
                hitboxOffset = gokuHitboxOffsetNotFlipped;
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
            }
            if (!isBeam && isSuper)
            {
                isBeam = true;
            }
            else if (isBeam)
            {
                chargePlayed = true;
                spamTimer = System.Environment.TickCount + 2000;
            }
        }
        public override void chargedOne()
        {
            if (canSuper)
            {
                base.superLoopInstance.Play();
            }
            disableProjectile = false;
        }
        public override void chargedTwo()
        {
            if (canFire)
            {
                isFire = true;
                hasFired = true;
                disableTimer = System.Environment.TickCount + 1000;
                fireSoundInstance.Play();
            }
            else
            {
                canBeam = true;
                isSuper = true;
                CHARACTER_DAMAGE *= 1.5;
                canSuper = false;
                canFire = true;
                chargeMax = 700;
                knockDownEndFrame = 5;
                chargeSoundInstance.Stop(true);
                chargeSoundInstance = superChargeSound.CreateInstance();
            }
        }
    }
}

