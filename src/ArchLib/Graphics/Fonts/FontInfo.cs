using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArchLib.Utility.ObjectModel;

namespace ArchLib.Graphics.Fonts
{
    public struct FontInfo
    {
        public readonly String FontImagePath;
        public readonly String FontFace;
        public readonly Int32 LineHeight;
        public readonly Int32 BaseLine;
        public readonly Int32 ScaleWidth;
        public readonly Int32 ScaleHeight;
        public readonly ReadOnlyCollection<CharInfo> CharInfo;
        public readonly Dictionary<CharPair, Int32> Kernings;

        public FontInfo(String fontImagePath, String fontFace, 
            Int32 lineHeight, Int32 baseLine, 
            Int32 scaleWidth, Int32 scaleHeight, 
            ReadOnlyCollection<CharInfo> charInfo, Dictionary<CharPair, Int32> kernings)
            : this()
        {
            FontImagePath = fontImagePath;
            FontFace = fontFace;
            LineHeight = lineHeight;
            BaseLine = baseLine;
            ScaleWidth = scaleWidth;
            ScaleHeight = scaleHeight;
            CharInfo = charInfo;
            Kernings = kernings;
        }
    }
}
