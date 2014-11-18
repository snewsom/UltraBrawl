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
        static Point gokuNumberOfFrames = new Point(18, 9);
        static CollisionOffset gokuCollisionOffset = new CollisionOffset(30, 1, 40, 20);
        static Vector2 gokuSpeed = new Vector2(64, 32);
        static Vector2 gokuFriction = new Vector2(0.8f, 1f);
        static Point gokuFrameSize = new Point(130, 130);
        



        // constructor
        public Goku(Texture2D image, SoundEffect sound1, SoundEffect sound2, PlayerIndex playerIndex, PlayerController playerController)
            : base(new SpriteSheet(image, gokuNumberOfFrames, 2.0f),
           new Vector2(600, 0), gokuCollisionOffset, gokuSpeed, gokuFriction, sound1, sound2, gokuFrameSize, playerIndex, playerController)
        {
            base.pcSegmentEndings.Add(new Point(17, 0));
            base.pcSegmentEndings.Add(new Point(5, 1));
            base.pcSegmentEndings.Add(new Point(9, 2));
            base.pcSegmentEndings.Add(new Point(14, 3));
            base.pcSegmentEndings.Add(new Point(18, 4));
            base.pcSegmentEndings.Add(new Point(4, 5));
            base.pcSegmentEndings.Add(new Point(5, 6));
            base.pcSegmentEndings.Add(new Point(9, 7));
            base.pcSegmentEndings.Add(new Point(13, 8));
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

