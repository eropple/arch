using System;
using System.Collections.Generic;

namespace ArchLib.Graphics.Fonts.Loaders
{
    public class FontLoadException : Exception
    {
        internal FontLoadException(String message)
            : base(message)
        {
        }
    }
}
