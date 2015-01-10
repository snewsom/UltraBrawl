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
    class Airman : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point numFrames = new Point(30, 20);
        static CollisionOffset collisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset AOEHitboxOffset = new CollisionOffset(0, 0, 0, 0);
        static CollisionOffset hitboxOffset = new CollisionOffset(100, 10, 10, 120);
        static CollisionOffset hitboxOffsetNotFlipped = new CollisionOffset(100, 10, 10, 120);
        static CollisionOffset hitboxOffsetFlipped = new CollisionOffset(100, 10, 120, 10);
        static Vector2 speed = new Vector2(200, 32);
        static Vector2 friction = new Vector2(0.8f, 1f);
        static Point frameSize = new Point(170, 170);
        



        // constructor
        public Airman(Texture2D image, SoundEffect chargeSound, SoundEffect superLoop)
            : base(new SpriteSheet(image, numFrames, 2.0f), collisionOffset, hitboxOffset, hitboxOffsetFlipped, hitboxOffsetNotFlipped, speed, friction, frameSize)
        {
            this.fireSound = superLoop;
            this.chargeSound = chargeSound;
            base.pcSegmentEndings.Add(new Point(1, 0)); //idle
            base.pcSegmentEndings.Add(new Point(22, 1)); //running
            base.pcSegmentEndings.Add(new Point(7, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(5, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(6, 4)); //punch
            base.pcSegmentEndings.Add(new Point(15, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(0, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(0, 7)); //hit
            base.pcSegmentEndings.Add(new Point(2, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(12, 9)); //charging
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
            base.knockDownEndFrame = 2;
            base.fireChargeFrame = 12;

            base.pcSegmentTimings.Add(100); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(50); //jumpkick
            base.pcSegmentTimings.Add(120); //punch
            base.pcSegmentTimings.Add(80); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(100); //blockhit
            base.pcSegmentTimings.Add(200); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(40); //charging
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

            canJumpKick = false;
            canAOE = true;
            canFire = true;
            CHARACTER_DAMAGE = 0.1;
            CHARACTER_ID = 8;
            CHARACTER_NAME = "Airman";
        }

        public override void heavyAttack()
        {
            

            if (isLGT)
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

        }

        public override void lightAttack()
        {
            isFire = true;
            hasFired = true;
           
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            chargeMax = 500;

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
            canFire = false;
            if (chargeSoundInstance.State == SoundState.Stopped && !chargePlayed)
            {
                chargeSoundInstance.Play();
                chargeSoundInstance.Volume = 0.5f;
                chargePlayed = true;
            }
            if (!isAOE)
            {
                isAOE = true;
                velocity.Y = 0;
                gravity = noGravity;
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
            gravity = defaultGravity;
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
            chargeSoundInstance.Stop(true);//will want to move this to a new method called cancelCharge so that it will finish if uninterrupted.
            gravity = defaultGravity;
            regenHitbox();
            isAOE = false;
        }
    }
        
}

