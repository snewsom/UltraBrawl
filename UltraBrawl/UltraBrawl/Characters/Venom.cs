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
using System.Diagnostics;

namespace UltraBrawl
{
    class Venom : PlayerCharacter
    {
        // constants for this particular sprite
        // static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point venomNumberOfFrames = new Point(30, 20);
        static CollisionOffset venomCollisionOffset = new CollisionOffset(80, 1, 60, 60);
        static CollisionOffset AOEHitboxOffset = new CollisionOffset(0, 0, 0, 0);
        static CollisionOffset venomHitboxOffset = new CollisionOffset(100, 10, 10, 120);
        static CollisionOffset venomHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 10, 120);
        static CollisionOffset venomHitboxOffsetFlipped = new CollisionOffset(100, 10, 120, 10);
        static Vector2 venomSpeed = new Vector2(90, 32);
        static Vector2 venomFriction = new Vector2(0.8f, 1f);
        static Point venomFrameSize = new Point(170, 170);




        // constructor
        public Venom(Texture2D image, SoundEffect sound1, SoundEffect sound2)
            : base(new SpriteSheet(image, venomNumberOfFrames, 2.0f), venomCollisionOffset, venomHitboxOffset, venomHitboxOffsetFlipped, venomHitboxOffsetNotFlipped, venomSpeed, venomFriction, sound1, sound2, venomFrameSize)
        {
            base.pcSegmentEndings.Add(new Point(12, 0)); //idle
            base.pcSegmentEndings.Add(new Point(9, 1)); //running
            base.pcSegmentEndings.Add(new Point(8, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(6, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(6, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(6, 6)); //block
            base.pcSegmentEndings.Add(new Point(6, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(2, 7)); //hit
            base.pcSegmentEndings.Add(new Point(13, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(22, 9)); //charging
            base.knockDownEndFrame = 7;

            base.pcSegmentTimings.Add(70); //idle
            base.pcSegmentTimings.Add(60); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(50); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(50); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(60); //blockhit
            base.pcSegmentTimings.Add(60); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(60); //charging
            base.setSegments();

            canJumpSpecial = true;
            canAOE = true;
            canSmash = true;
            CHARACTER_DAMAGE = 1.7;
            CHARACTER_ID = 4;
            CHARACTER_NAME = "Venom";
        }

        public override void spawn(PlayerPreset preset)
        {

            position = preset.spawn;
            pcPlayerNum = preset.index;
            CHARACTER_NAME = "Venom";
            chargeMax = 1100;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = venomHitboxOffsetFlipped;
            }
            else
            {
                effects = SpriteEffects.None;
                hitboxOffset = venomHitboxOffsetNotFlipped;
            }
            update = true;
            regenHitbox();
        }

        public override void charging()
        {
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
                    hitboxOffset = venomHitboxOffsetFlipped;
                }
                else
                {
                    hitboxOffset = venomHitboxOffsetNotFlipped;
                }
                gravity = defaultGravity;
                regenHitbox();
                isAOE = false;
        }

    }
}
