using System;
using System.Collections.Generic;

namespace ArchLib.Graphics.Fonts
{
    public struct CharPair
    {
        public readonly Int32 First;
        public readonly Int32 Second;

        public CharPair(Int32 first, Int32 second) : this()
        {
            First = first;
            Second = second;
        }
    }
}
