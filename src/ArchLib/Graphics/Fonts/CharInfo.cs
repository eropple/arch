using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ArchLib.Graphics.Fonts
{
    public struct CharInfo
    {
        public readonly Int32 XAdvance;
        public readonly Rectangle SourceRectangle;
        public readonly Rectangle DestinationRectangle;

        public CharInfo(Int32 xAdvance, Rectangle sourceRectangle, Rectangle destinationRectangle) : this()
        {
            this.XAdvance = xAdvance;
            this.SourceRectangle = sourceRectangle;
            this.DestinationRectangle = destinationRectangle;
        }
    }
}
