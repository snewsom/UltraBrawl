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
        static Point kazuyaNumberOfFrames = new Point(30, 20);
        static CollisionOffset kazuyaCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset kazuyaHitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset kazuyaHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 120, 20);
        static CollisionOffset kazuyaHitboxOffsetFlipped = new CollisionOffset(100, 10, 20, 120);
        static Vector2 kazuyaSpeed = new Vector2(60, 32);
        static Vector2 kazuyaFriction = new Vector2(0.8f, 1f);
        static Point kazuyaFrameSize = new Point(170, 170);
        



        // constructor
        public Kazuya(Texture2D image, SoundEffect sound1, SoundEffect sound2)
            : base(new SpriteSheet(image, kazuyaNumberOfFrames, 2.0f), kazuyaCollisionOffset, kazuyaHitboxOffset, kazuyaHitboxOffsetFlipped, kazuyaHitboxOffsetNotFlipped, kazuyaSpeed, kazuyaFriction, sound1, sound2, kazuyaFrameSize)
        {
            base.pcSegmentEndings.Add(new Point(6, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(9, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(9, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(7, 4)); //punch
            base.pcSegmentEndings.Add(new Point(7, 5)); //kick
            base.pcSegmentEndings.Add(new Point(3, 6)); //block
            base.pcSegmentEndings.Add(new Point(0, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(3, 7)); //hit
            base.pcSegmentEndings.Add(new Point(21, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(13, 9)); //charging
            base.pcSegmentEndings.Add(new Point(4, 10)); //superIdle
            base.pcSegmentEndings.Add(new Point(5, 11)); //superRunning
            base.pcSegmentEndings.Add(new Point(9, 12)); //superJumping
            base.pcSegmentEndings.Add(new Point(13, 13)); //superJumpkicking
            base.pcSegmentEndings.Add(new Point(3, 14)); //superPunch
            base.pcSegmentEndings.Add(new Point(6, 15)); //superKick
            base.pcSegmentEndings.Add(new Point(0, 16)); //superBlock
            base.pcSegmentEndings.Add(new Point(2, 16)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(2, 17)); //superHit
            base.knockDownEndFrame = 5;

            base.pcSegmentTimings.Add(70); //idle
            base.pcSegmentTimings.Add(40); //running
            base.pcSegmentTimings.Add(120); //jumping
            base.pcSegmentTimings.Add(40); //jumpkick
            base.pcSegmentTimings.Add(30); //punch
            base.pcSegmentTimings.Add(40); //kick
            base.pcSegmentTimings.Add(50); //block
            base.pcSegmentTimings.Add(50); //blockhit
            base.pcSegmentTimings.Add(50); //hit
            base.pcSegmentTimings.Add(60); //knockdown
            base.pcSegmentTimings.Add(60); //charging
            base.pcSegmentTimings.Add(50); //superIdle
            base.pcSegmentTimings.Add(80); //superRunning
            base.pcSegmentTimings.Add(120); //superJumping
            base.pcSegmentTimings.Add(40); //superJumpkicking
            base.pcSegmentTimings.Add(40); //superPunch
            base.pcSegmentTimings.Add(40); //superKick
            base.pcSegmentTimings.Add(50); //superBlock
            base.pcSegmentTimings.Add(100); //superBlockhit
            base.pcSegmentTimings.Add(100); //superHit
            base.setSegments();

            base.canSuper = false;
            CHARACTER_DAMAGE = 1;
            CHARACTER_ID = 6;
            CHARACTER_NAME = "Kazuya";
        }

        public override void spawn(PlayerPreset preset)
        {
            position = preset.spawn;
            pcPlayerNum = preset.index;
            controller = preset.controller;

            if (preset.index.ToString().Equals("Two") || preset.index.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = kazuyaHitboxOffsetFlipped;
            }
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
        }

    }
}

