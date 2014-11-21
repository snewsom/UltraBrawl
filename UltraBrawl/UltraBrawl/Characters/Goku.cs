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
        static Point gokuNumberOfFrames = new Point(20, 20);
        static CollisionOffset gokuCollisionOffset = new CollisionOffset(80, 1, 50, 50);
        static Vector2 gokuSpeed = new Vector2(128, 32);
        static Vector2 gokuFriction = new Vector2(0.8f, 1f);
        static Point gokuFrameSize = new Point(170, 170);
        



        // constructor
        public Goku(Texture2D image, SoundEffect sound1, SoundEffect sound2, PlayerIndex playerIndex, PlayerController playerController, Vector2 spawnLoc)
            : base(new SpriteSheet(image, gokuNumberOfFrames, 2.0f), spawnLoc, gokuCollisionOffset, gokuSpeed, gokuFriction, sound1, sound2, gokuFrameSize, playerIndex, playerController)
        {
            if (playerIndex.ToString().Equals("Two") || playerIndex.ToString().Equals("Four"))
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            base.pcSegmentEndings.Add(new Point(14, 0)); //idle
            base.pcSegmentEndings.Add(new Point(5, 1)); //running
            base.pcSegmentEndings.Add(new Point(9, 2)); //jumping
            base.pcSegmentEndings.Add(new Point(14, 3)); //jumpkick
            base.pcSegmentEndings.Add(new Point(3, 4)); //punch
            base.pcSegmentEndings.Add(new Point(6, 5)); //kick
            base.pcSegmentEndings.Add(new Point(0, 6)); //block
            base.pcSegmentEndings.Add(new Point(2, 6)); //blockhit
            base.pcSegmentEndings.Add(new Point(2, 7)); //hit
            base.pcSegmentEndings.Add(new Point(13, 8)); //knockdown
            base.pcSegmentEndings.Add(new Point(18, 9)); //charging
            base.pcSegmentEndings.Add(new Point(4, 10)); //superIdle
            base.pcSegmentEndings.Add(new Point(5, 11)); //superRunning
            base.pcSegmentEndings.Add(new Point(9, 12)); //superJumping
            base.pcSegmentEndings.Add(new Point(13, 13)); //superJumpkicking
            base.pcSegmentEndings.Add(new Point(3, 14)); //superPunch
            base.pcSegmentEndings.Add(new Point(6, 15)); //superKick
            base.pcSegmentEndings.Add(new Point(0, 16)); //superBlock
            base.pcSegmentEndings.Add(new Point(2, 16)); //superBlockhit
            base.pcSegmentEndings.Add(new Point(2, 17)); //superHit
            base.setSegments();

            CHARACTER_ID = 0;
            CHARACTER_NAME = "Goku";
        }

        public override void charging()
        {

        }


        public override void charged()
        {
            base.superLoopInstance.Play();
            base.isSuper = true;
        }

    }
}

