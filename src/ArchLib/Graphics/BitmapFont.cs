using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ArchLib.Graphics.Fonts;
using ArchLib.Graphics.Fonts.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    public class BitmapFont : IReloadable, IDisposable
    {
        // TODO: refactor this class to use TextureRegions
        private readonly FontInfo _fontInfo;
        public readonly Texture2D BackingTexture;
        public readonly Int32 ScaleFactor;

        /// <summary>
        /// The ratio of ScaleFactor / Arch.Scaling.ScaleFactor. Will be used for determining
        /// destination rectangles.
        /// </summary>
        protected readonly Single ScaleRatio;

        public readonly Int32 StringHeight;

        public BitmapFont(Texture2D backingTexture, Int32 scaleFactor, FontInfo fontInfo)
        {
            _fontInfo = fontInfo;
            BackingTexture = backingTexture;
            ScaleFactor = scaleFactor;

            ScaleRatio = (float)Arch.Scaling.ScaleFactor / (float)ScaleFactor;

            StringHeight = _fontInfo.LineHeight/ScaleFactor;
        }

        [Obsolete]
        public int MeasureStringHeight()
        {
            return StringHeight;
        }

        public int MeasureStringWidth(string text)
        {
            int x = 0;
            int length = text.Length;

            for (int i = 0; i < length; i++)
            {

                char ch0 = text[i];
                char ch1 = (i + 1) < length ? text[i + 1] : '\0';
                Debug.Assert(ch0 < 256, "UNICODE character in the string");
                Debug.Assert(ch1 < 256, "UNICODE character in the string");

                CharInfo chi = _fontInfo.CharInfo[ch0];
                CharPair chPair = new CharPair(ch0, ch1);
                int kerning = 0;

                if (_fontInfo.Kernings.ContainsKey(chPair))
                {
                    kerning = _fontInfo.Kernings[chPair];
                }

                x += chi.XAdvance;
                x += kerning;
            }
            return x / ScaleFactor;
        }


        public void DrawString(SpriteBatch sb, string text, int xPos, int yPos, Color color)
        {
            Vector2 translatedPosition = Arch.Scaling.VirtualCoordsToScaled(new Vector2(xPos, yPos));
            int x = (Int32)translatedPosition.X;
            int y = ((Int32)translatedPosition.Y) - _fontInfo.BaseLine;

            int length = text.Length;

            for (int i = 0; i < length; i++)
            {

                char ch0 = text[i];
                char ch1 = (i + 1) < length ? text[i + 1] : '\0';
                //Debug.Assert(ch0<256, "UNICODE character in the string");
                //Debug.Assert(ch1<256, "UNICODE character in the string");

                CharInfo chi = _fontInfo.CharInfo[ch0];
                CharPair chPair = new CharPair(ch0, ch1);
                int kerning = 0;

                if (_fontInfo.Kernings.ContainsKey(chPair))
                {
                    kerning = _fontInfo.Kernings[chPair];
                }

                Rectangle dstRect = chi.DestinationRectangle;
                Rectangle srcRect = chi.SourceRectangle;
                dstRect.X += x;
                dstRect.Y += y;
                x += chi.XAdvance;
                x += kerning;

                if (ScaleFactor == Arch.Scaling.ScaleFactor)
                {
                    sb.Draw(BackingTexture, dstRect, srcRect, color);
                }
                else
                {
                    throw new InvalidOperationException("Bitmap fonts can't be scaled; both @2x and regular versions are needed.");
                }
            }
        }

        public void Reload()
        {
            // TODO: implement asset reloading for Android
        }

        public void Dispose()
        {
            BackingTexture.Dispose();
        }


        public static BitmapFont LoadFromKey(String key)
        {
            String pathBase = Path.Combine(Arch.Options.ContentRoot, "Fonts", key);

            if (Arch.Scaling.ScaleFactor == 2)
            {
                String retinaPath = pathBase + "@2x.fnt";
                if (File.Exists(retinaPath))
                {
                    return FontProcessor.Process(FontLoader.Load(retinaPath), 2);
                }
            }
            else if (Arch.Scaling.ScaleFactor == 1)
            {
                String normalPath = pathBase + ".fnt";
                if (File.Exists(normalPath))
                {
                    return FontProcessor.Process(FontLoader.Load(normalPath), 1);
                }
            }

            return null;
        }
    }
}
