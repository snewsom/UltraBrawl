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
    class Goku : PlayerCharacter
    {
        // constants for this particular sprite
        //static List<Texture2D> particleList;
        //static ParticleEngine2D particleEngine;
        static Point gokuNumberOfFrames = new Point(20, 20);
        static CollisionOffset gokuCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static CollisionOffset gokuHitboxOffset = new CollisionOffset(100, 10, 20, 100);
        static CollisionOffset gokuHitboxOffsetNotFlipped = new CollisionOffset(100, 10, 120, 20);
        static CollisionOffset gokuHitboxOffsetFlipped = new CollisionOffset(100, 10, 20, 120);
        static Vector2 gokuSpeed = new Vector2(128, 32);
        static Vector2 gokuFriction = new Vector2(0.8f, 1f);
        static Point gokuFrameSize = new Point(170, 170);
        



        // constructor
        public Goku(Texture2D image, SoundEffect sound1, SoundEffect sound2, PlayerIndex playerIndex, PlayerController playerController, Vector2 spawnLoc)
            : base(new SpriteSheet(image, gokuNumberOfFrames, 2.0f), spawnLoc, gokuCollisionOffset, gokuHitboxOffset, gokuHitboxOffsetFlipped, gokuHitboxOffsetNotFlipped, gokuSpeed, gokuFriction, sound1, sound2, gokuFrameSize, playerIndex, playerController)
        {
            if (playerIndex.ToString().Equals("Two") || playerIndex.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
                hitboxOffset = gokuHitboxOffsetFlipped;
            }
            
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
            base.knockDownEndFrame = 6;
    
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
            base.pcSegmentTimings.Add(140); //charging
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

            base.canSuper = true;
            CHARACTER_ID = 0;
            CHARACTER_NAME = "Goku";
            CHARACTER_DAMAGE = 1.2;
        }

        public override void charging()
        {

        }
        public override void chargedOne()
        {
            base.superLoopInstance.Play();
            base.isSuper = true;
        }
        public override void chargedTwo()
        {
            CHARACTER_DAMAGE *= 1.2;
            base.isSuper = true;
        }
    }
}

