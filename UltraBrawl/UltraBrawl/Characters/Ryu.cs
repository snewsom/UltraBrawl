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
    class Ryu : PlayerCharacter
    {
        // constants for this particular sprite
       // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point ryuNumberOfFrames = new Point(30, 20);
        static CollisionOffset ryuCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset ryuHitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset ryuHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset ryuHitboxOffsetFlipped = new CollisionOffset(100, 10, 100, 20);
        static Vector2 ryuSpeed = new Vector2(200, 32);
        static Vector2 ryuFriction = new Vector2(0.8f, 1f);
        static Point ryuFrameSize = new Point(170, 170);
        



        // constructor
        public Ryu(Texture2D image, SoundEffect chargeSound, SoundEffect sound2)
            : base(new SpriteSheet(image, ryuNumberOfFrames, 2.0f), ryuCollisionOffset, ryuHitboxOffset, ryuHitboxOffsetFlipped, ryuHitboxOffsetNotFlipped, ryuSpeed, ryuFriction, ryuFrameSize)
        {
            this.chargeSound = chargeSound;
            base.pcSegmentEndings.Add(new Point(6, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(9, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(16, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(5, 4)); //punch
            base.pcSegmentEndings.Add(new Point(7, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(2, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(9, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(12, 9)); //charging
            base.knockDownEndFrame = 7;
            fireChargeFrame = 2;

            base.pcSegmentTimings.Add(80); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(80); //jumping
            base.pcSegmentTimings.Add(15); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(70); //charging
            base.setSegments();

            JKvelocity = 400;
            chargeMax = 650;
            base.canSuper = false;
            JKknockdown = true;
            canFire = true;
            canJumpKick = true;
            CHARACTER_DAMAGE = 1.1;
            CHARACTER_ID = 2;
            CHARACTER_NAME = "Ryu";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;

            chargeSoundInstance = chargeSound.CreateInstance();
            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = ryuHitboxOffsetFlipped;
            }
            else
            {
                effects = SpriteEffects.None;
                hitboxOffset = ryuHitboxOffsetNotFlipped;
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
            chargeSoundInstance.Stop(true);//will want to move this to a new method called cancelCharge so that it will finish if uninterrupted.
            isFire = true;
            hasFired = true;
        }

    }
}

