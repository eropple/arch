using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics.Fonts.Loaders
{
    public static class FontProcessor
    {
        public static BitmapFont Process(FontFile input, Int32 scaleFactor)
        {
            var kernings = new Dictionary<CharPair, Int32>();
            var charInfo = new CharInfo[256];

            if (input.FileInfo.Unicode != 0)
                throw new FontLoadException("Unicode not supported (remove the Unicode flag).");

            if (input.Pages.Count != 1)
                throw new FontLoadException("ArchLib font loader only supports one image page per font.");

            String fontImagePath = Path.Combine(Arch.Options.ContentRoot, "Textures", "Fonts",
                                                Path.GetFileName(input.Pages[0].File));

            String fontFace = input.FileInfo.Face;
            Int32 baseLine = input.Common.Base;
            Int32 lineHeight = input.Common.LineHeight;
            Int32 scaleWidth = input.Common.ScaleW;
            Int32 scaleHeight = input.Common.ScaleH;

            foreach(FontChar ch in input.Chars)
            {
                if (ch.ID > 255)
                    throw new FontLoadException("Unicode not supported (character id > 255 found).");

                charInfo[ch.ID] = new CharInfo(ch.XAdvance, new Rectangle(ch.X, ch.Y, ch.Width, ch.Height),
                                               new Rectangle(ch.XOffset, ch.YOffset, ch.Width, ch.Height));
            }

            foreach (FontKerning fk in input.Kernings)
            {
                kernings.Add(new CharPair(fk.First, fk.Second), fk.Amount);
            }

            var info = new FontInfo(fontImagePath, fontFace, lineHeight, baseLine, scaleWidth, scaleHeight,
                new ReadOnlyCollection<CharInfo>(charInfo), kernings);

            Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(fontImagePath));

            return new BitmapFont(tex, scaleFactor, info);
        }
    }
}
