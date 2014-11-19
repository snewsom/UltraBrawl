using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UltraBrawl
{
    class SpriteSheet
    {
        public Texture2D texture;
        public Point sheetSize;
        public float scale;
        public SpriteSheetSegment[] segments = new SpriteSheetSegment[17];
        public SpriteSheetSegment currentSegment;
        private int numSegments;

        public SpriteSheet(Texture2D texture, Point sheetSize, float scale)
        {
            this.texture = texture;
            this.sheetSize = sheetSize;
            this.scale = scale;
            numSegments = 0;
        }

        public void addSegment(Point frameSize, Point startFrame, Point endFrame,
            int millisecondsPerFrame)
        {
            segments[numSegments] = new SpriteSheetSegment(frameSize, startFrame, endFrame, 
                millisecondsPerFrame);
            ++numSegments;
            System.Diagnostics.Debug.WriteLine("number of segments is " + numSegments);
        }

        public void setCurrentSegment(int which)
        {
            System.Diagnostics.Debug.WriteLine("segment number " + which);
            currentSegment = segments[which];
        }
    }
}
